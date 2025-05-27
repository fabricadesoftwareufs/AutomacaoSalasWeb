using Model;
using Newtonsoft.Json;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class MonitoramentoService : IMonitoramentoService
    {
        private readonly SalasDBContext _context;
        private const string AC_ON = "AC-ON";
        private const string L_ON = "LZ-ON";
        private const string AC_OFF = "AC-OFF";
        private const string L_OFF = "LZ-OFF";

        public MonitoramentoService(SalasDBContext context)
        {
            _context = context;
        }

        public List<MonitoramentoModel> GetAll() => _context.Monitoramentos.Select(m => new MonitoramentoModel { Id = m.Id, IdEquipamento = m.IdEquipamento, Estado = Convert.ToBoolean(m.Estado) }).ToList();

        public MonitoramentoModel GetById(int id) => _context.Monitoramentos.Where(m => m.Id == id).Select(m => new MonitoramentoModel { Id = m.Id, IdEquipamento = m.IdEquipamento, Estado = Convert.ToBoolean(m.Estado) }).FirstOrDefault();

        public MonitoramentoModel GetByIdEquipamento(int idEquipamento) => _context.Monitoramentos.Where(m => m.IdEquipamento == idEquipamento).Select(m => new MonitoramentoModel { Id = m.Id, IdEquipamento = m.IdEquipamento, Estado = Convert.ToBoolean(m.Estado) }).FirstOrDefault();

        public List<MonitoramentoModel> GetByIdSala(uint idSala)
        {

            var monitoramentos = new List<MonitoramentoModel>(from m in _context.Monitoramentos
                                                              join e in _context.Equipamentos on m.IdEquipamento equals e.Id
                                                              where e.IdSala == idSala
                                                              select new MonitoramentoModel
                                                              {
                                                                  Id = m.Id,
                                                                  Estado = Convert.ToBoolean(m.Estado),
                                                                  IdEquipamento = m.IdEquipamento,
                                                                  IdEquipamentoNavigation = new EquipamentoModel { Id = e.Id, TipoEquipamento = e.TipoEquipamento, Sala = e.IdSala },
                                                              });


            return monitoramentos;
        }

        public MonitoramentoModel GetByIdSalaAndTipoEquipamento(uint idSala, string tipoEquipamento)
        {
            var moni = _context.Monitoramentos.ToList();
            var equip = _context.Equipamentos.ToList();
            var monitoramentos = (from m in moni
                                  join e in equip on m.IdEquipamento equals e.Id
                                  where e.IdSala == idSala && tipoEquipamento.ToUpper().Equals(e.TipoEquipamento.Trim().ToUpper())
                                  select new MonitoramentoModel
                                  {
                                      Id = m.Id,
                                      Estado = Convert.ToBoolean(m.Estado),
                                      IdEquipamento = m.IdEquipamento,
                                      IdEquipamentoNavigation = new EquipamentoModel { Id = e.Id, TipoEquipamento = e.TipoEquipamento, Sala = e.IdSala },
                                  }
                                  ).FirstOrDefault();


            return monitoramentos;
        }


        public bool Insert(MonitoramentoModel model)
        {
            try
            {
                if (GetByIdEquipamento(model.IdEquipamento) != null)
                    return true;

                _context.Add(SetEntity(model));
                return _context.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //TODO: Ajeitar esse método depois
        public bool MonitorarSala(uint idUsuario, MonitoramentoViewModel monitoramento)
        {
            var _equipamentoSala = new EquipamentoService(_context);
            var equipamento = _equipamentoSala.GetByIdEquipamento(monitoramento.EquipamentoId);

            if (equipamento != null)
            {
                if (monitoramento.SalaParticular)
                {
                    var _salaParticular = new SalaParticularService(_context);
                    if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, equipamento.Sala) == null)
                        throw new ServiceException("Ocorreu um problema e o monitoramento não pôde ser concluído. Por favor, tente novamente mais tarde.");
                }
                else
                {
                    var _horarioSalaService = new HorarioSalaService(_context);
                    if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, equipamento.Sala))
                        throw new ServiceException("Você não está no horário reservado para realizar o monitoramento desta sala.");
                }

                var monitoramentoModel = new MonitoramentoModel
                {
                    IdEquipamento = monitoramento.EquipamentoId,
                    Estado = monitoramento.Estado,
                    Id = monitoramento.Id,
                    SalaParticular = monitoramento.SalaParticular
                };

                if (!EnviarComandosMonitoramento(monitoramentoModel))
                    throw new ServiceException("Houve inconsistências no cadastro da solicitação de Monitoramento. Verifique os dados do equipamento e hardware responsáveis e tente novamente.");

                Update(monitoramentoModel);
            }

            return true;
        }

        public bool MonitorarEquipamento(uint idUsuario, MonitoramentoModel model)
        {
            var _equipamentoSala = new EquipamentoService(_context);
            var equipamento = _equipamentoSala.GetByIdEquipamento(model.IdEquipamento);

            if ((bool)model.SalaParticular)
            {
                var _salaParticular = new SalaParticularService(_context);
                if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, equipamento.Sala) == null)
                    throw new ServiceException("Ocorreu um problema e o monitoramento não pôde ser concluído. Por favor, tente novamente mais tarde.");
            }
            else
            {
                var _horarioSalaService = new HorarioSalaService(_context);
                if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, equipamento.Sala))
                    throw new ServiceException("Você não está no horário reservado para monitorar essa sala!");
            }

            if (!EnviarComandosMonitoramento(model))
                throw new ServiceException("Ocorreu um problema e o monitoramento não pôde ser concluído. Por favor, tente novamente mais tarde.");

            return Update(model);
        }

        public bool Update(MonitoramentoModel model)
        {
            SetEntity(model); // A entidade já será atualizada ou adicionada ao contexto
            return _context.SaveChanges() == 1;


        }
        //TODO: Ajeitar esse método depois
        private bool EnviarComandosMonitoramento(MonitoramentoModel monitoramento)
        {
            try
            {
                var modelDesatualizado = GetById(monitoramento.Id);
                bool comandoEnviadoComSucesso = true;

                if (monitoramento.Estado != modelDesatualizado.Estado)
                {
                    var _hardwareDeSalaService = new HardwareDeSalaService(_context);
                    var _equipamentoServiceService = new EquipamentoService(_context);
                    var equipamento = _equipamentoServiceService.GetByIdEquipamento(monitoramento.IdEquipamento);

                    string tipoEquipamento = string.Empty, operacao = string.Empty, retornoEsperado = string.Empty;

                    if (equipamento.TipoEquipamento.Equals(EquipamentoModel.TIPO_CONDICIONADOR))
                    {
                        var _codigosInfravermelhoService = new CodigoInfravermelhoService(_context);
                        var idOperacao = (bool)monitoramento.Estado ? OperacaoModel.OPERACAO_LIGAR : OperacaoModel.OPERACAO_DESLIGAR;
                        var codigosInfravermelho = _codigosInfravermelhoService.GetByIdOperacaoAndIdModeloEquipamento(equipamento.Id, idOperacao);

                        if (codigosInfravermelho == null)
                            return false;

                        tipoEquipamento = EquipamentoModel.TIPO_CONDICIONADOR;
                        operacao = codigosInfravermelho.Codigo.Replace(" ","");
                        retornoEsperado = (bool)monitoramento.Estado ? AC_ON : AC_OFF;
                    }
                    else
                    {
                        tipoEquipamento = EquipamentoModel.TIPO_LUZES;
                        operacao = monitoramento.Estado.ToString();
                        retornoEsperado = (bool)monitoramento.Estado ? L_ON : L_OFF;
                    }

                    var hardwareAtuador = _hardwareDeSalaService.GetById(equipamento.HardwareDeSala.GetValueOrDefault(0));
                    var hardwareDeSala = _hardwareDeSalaService.GetControladorByIdSala(equipamento.Sala);

                    var mensagem = JsonConvert.SerializeObject(
                          new
                          {
                              type = tipoEquipamento,
                              acting = monitoramento.Estado.ToString(),
                              code = operacao,
                              uuid = hardwareAtuador.Uuid
                          });

                    var solicitacaoModel = new SolicitacaoModel
                    {
                        DataSolicitacao = DateTime.Now,
                        IdHardware = hardwareDeSala.Id,
                        IdHardwareAtuador = hardwareAtuador.Id,
                        Payload = mensagem,
                        TipoSolicitacao = GetTipoSolicitacao(tipoEquipamento)
                    };

                    var _solicitacaService = new SolicitacacaoService(_context);

                    var solicitacao = _solicitacaService.GetByIdHardwareAtuador(hardwareAtuador.Id).FirstOrDefault();

                    if (solicitacao != null)
                    {
                        solicitacao.DataFinalizacao = DateTime.UtcNow;
                        _solicitacaService.Update(solicitacao);
                    }

                    comandoEnviadoComSucesso = _solicitacaService.Insert(solicitacaoModel);
                }

                return comandoEnviadoComSucesso;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public string GetTipoSolicitacao(string operacao)
        {
            return operacao switch
            {
                EquipamentoModel.TIPO_CONDICIONADOR => SolicitacaoModel.MONITORAMENTO_AR_CONDICIONADO,
                EquipamentoModel.TIPO_LUZES => SolicitacaoModel.MONITORAMENTO_LUZES,
                _ => SolicitacaoModel.ATUALIZAR_RESERVAS,
            };
        }

        private Monitoramento SetEntity(MonitoramentoModel model)
        {
            var monitoramentoAtual = _context.Monitoramentos.FirstOrDefault(m => m.Id == model.Id);

            if (monitoramentoAtual == null)
            {
                // Se não existe, cria um novo
                return new Monitoramento
                {
                    Id = model.Id,
                    IdEquipamento = model.IdEquipamento,
                    IdOperacao = 1, // Operação padrão (ligar)
                    IdUsuario = 2,
                    DataHora =  DateTime.Now,
                    Estado = (sbyte)(model.Estado ? 1 : 0)
                };
            }
            else
            {
                // Se já existe, atualiza a entidade existente
                monitoramentoAtual.IdEquipamento = model.IdEquipamento;
                monitoramentoAtual.IdOperacao = monitoramentoAtual.IdOperacao == 1 ? 2 : 1;
                monitoramentoAtual.DataHora = DateTime.Now;
                if(monitoramentoAtual.IdOperacao == 1)
                {
                    monitoramentoAtual.Estado = 1;
                }
                else
                {
                    monitoramentoAtual.Estado = 0;
                }

                return monitoramentoAtual;
            }
        }
    }
}
