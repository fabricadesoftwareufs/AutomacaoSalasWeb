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
        private readonly SalasUfsDbContext _context;
        private const string AC_ON = "AC-ON";
        private const string L_ON = "LZ-ON";
        private const string AC_OFF = "AC-OFF";
        private const string L_OFF = "LZ-OFF";
        private const string NOT_AVALIABLE = "NOT-AVALIABLE";

        public MonitoramentoService(SalasUfsDbContext context)
        {
            _context = context;
        }

        public List<MonitoramentoModel> GetAll() => _context.Monitoramento.Select(m => new MonitoramentoModel { Id = m.Id, EquipamentoId = m.Equipamento, Estado = Convert.ToBoolean(m.Estado) }).ToList();

        public MonitoramentoModel GetById(int id) => _context.Monitoramento.Where(m => m.Id == id).Select(m => new MonitoramentoModel { Id = m.Id, EquipamentoId = m.Equipamento, Estado = Convert.ToBoolean(m.Estado) }).FirstOrDefault();

        public MonitoramentoModel GetByIdEquipamento(int idEquipamento) => _context.Monitoramento.Where(m => m.Equipamento == idEquipamento).Select(m => new MonitoramentoModel { Id = m.Id, EquipamentoId = m.Equipamento, Estado = Convert.ToBoolean(m.Estado) }).FirstOrDefault();

        public List<MonitoramentoModel> GetByIdSala(int idSala)
        {

            var monitoramentos = new List<MonitoramentoModel>(from m in _context.Monitoramento
                                                              join e in _context.Equipamento on m.Equipamento equals e.Id
                                                              where e.Sala == idSala
                                                              select new MonitoramentoModel
                                                              {
                                                                  Id = m.Id,
                                                                  Estado = Convert.ToBoolean(m.Estado),
                                                                  EquipamentoId = m.Equipamento,
                                                                  EquipamentoNavigation = new EquipamentoModel { Id = e.Id, TipoEquipamento = e.TipoEquipamento, Sala = e.Sala },
                                                              });

             
            return monitoramentos;
        }

        public List<MonitoramentoViewModel> GetByIdSalaAndTipoEquipamento(int idSala, string tipoEquipamento)
        {
            var moni = _context.Monitoramento.ToList();
            var equip = _context.Equipamento.ToList();
            var hard = _context.Hardwaredesala.ToList();

            var monitoramentos = (from m in moni
                                  join e in equip on m.Equipamento equals e.Id
                                  join h in hard on e.HardwareDeSala.Value equals h.Id
                                  where e.Sala == idSala && tipoEquipamento.ToUpper().Equals(e.TipoEquipamento.Trim().ToUpper())
                                  select new MonitoramentoViewModel
                                  {
                                      Id = m.Id,
                                      Estado = Convert.ToBoolean(m.Estado),
                                      EquipamentoId = m.Equipamento,
                                      Uuid = h.Uuid,
                                      ModeloEquipamento = e.Modelo
                                  }).ToList();


            return monitoramentos;
        }


        public bool Insert(MonitoramentoModel model)
        {
            try
            {
                if (GetByIdEquipamento(model.EquipamentoId) != null)
                    return true;

                _context.Add(SetEntity(model));
                return _context.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool MonitorarSala(int idUsuario, MonitoramentoViewModel monitoramento)
        {
            var _equipamentoSala = new EquipamentoService(_context);
            var equipamento = _equipamentoSala.GetByIdEquipamento(monitoramento.EquipamentoId);

            if (equipamento != null)
            {
                if (monitoramento.SalaParticular)
                {
                    var _salaParticular = new SalaParticularService(_context);
                    if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, equipamento.Sala) == null)
                        throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");
                }
                else
                {
                    var _horarioSalaService = new HorarioSalaService(_context);
                    if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, equipamento.Sala))
                        throw new ServiceException("Você não está no horário reservado para monitorar essa sala!");
                }

                var monitoramentoModel = new MonitoramentoModel
                {
                     EquipamentoId = monitoramento.EquipamentoId,
                     Estado = monitoramento.Estado,
                     Id = monitoramento.Id,
                     SalaParticular = monitoramento.SalaParticular
                };

                if (!EnviarComandosMonitoramento(monitoramentoModel))
                    throw new ServiceException("Houveram inconsistências no cadastro da solicitação de Monitoramento, confirme os dados do equioamento e hardware responsável e tente novamente!");

                Update(monitoramentoModel);
            }

            return true;
        }

        public bool MonitorarEquipamento(int idUsuario, MonitoramentoModel model)
        {
            var _equipamentoSala = new EquipamentoService(_context);
            var equipamento = _equipamentoSala.GetByIdEquipamento(model.EquipamentoId);

            if (model.SalaParticular)
            {
                var _salaParticular = new SalaParticularService(_context);
                if (_salaParticular.GetByIdUsuarioAndIdSala(idUsuario, equipamento.Sala) == null)
                    throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");
            }
            else
            {
                var _horarioSalaService = new HorarioSalaService(_context);
                if (!_horarioSalaService.VerificaSeEstaEmHorarioAula(idUsuario, equipamento.Sala))
                    throw new ServiceException("Você não está no horário reservado para monitorar essa sala!");
            }

            if (!EnviarComandosMonitoramento(model))
                throw new ServiceException("Houve um problema e o monitoramento não pode ser finalizado, por favor tente novamente mais tarde!");

            return Update(model);
        }

        public bool Update(MonitoramentoModel model)
        {
            try
            {
                _context.Update(SetEntity(model));
                return _context.SaveChanges() == 1;
            }
            catch (Exception)
            {
                throw new ServiceException("Houve um problema ao tentar fazer monitoramento da sala, por favor tente novamente em alguns minutos!");
            }
        }

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
                    var equipamento = _equipamentoServiceService.GetByIdEquipamento(monitoramento.EquipamentoId);

                    string tipoEquipamento = string.Empty, operacao = string.Empty, retornoEsperado = string.Empty;

                    if (equipamento.TipoEquipamento.Equals(EquipamentoModel.TIPO_CONDICIONADOR))
                    {
                        var _codigosInfravermelhoService = new CodigoInfravermelhoService(_context);
                        var idOperacao = monitoramento.Estado ? OperacaoModel.OPERACAO_LIGAR : OperacaoModel.OPERACAO_DESLIGAR;
                        var codigosInfravermelho = _codigosInfravermelhoService.GetByIdOperacaoAndIdEquipamento(equipamento.Id, idOperacao);

                        if (codigosInfravermelho == null)
                            return false;

                        tipoEquipamento = EquipamentoModel.TIPO_CONDICIONADOR;
                        operacao = codigosInfravermelho.Codigo.Replace(" ","");
                        retornoEsperado = monitoramento.Estado ? AC_ON : AC_OFF;
                    }
                    else
                    {
                        tipoEquipamento = EquipamentoModel.TIPO_LUZES;
                        operacao = monitoramento.Estado.ToString();
                        retornoEsperado = monitoramento.Estado ? L_ON : L_OFF;
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
                        Payload = mensagem,
                        TipoSolicitacao = GetTipoSolicitacao(tipoEquipamento)
                    };

                    var _solicitacaService = new SolicitacacaoService(_context);

                    var solicitacao = _solicitacaService.GetByIdHardware(hardwareDeSala.Id, GetTipoSolicitacao(tipoEquipamento)).FirstOrDefault();

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
            return new Monitoramento
            {
                Id = model.Id,
                Estado = Convert.ToByte(model.Estado),
                Equipamento = model.EquipamentoId
            };
        }
    }
}
