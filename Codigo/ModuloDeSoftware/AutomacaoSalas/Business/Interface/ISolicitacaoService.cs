﻿using Model;
using Model.AuxModel;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ISolicitacaoService
    {
        List<SolicitacaoModel> GetAll();
        SolicitacaoModel GetById(int id);
        List<SolicitacaoModel> GetByIdHardware(uint idHardware, string tipo, bool todos = false);
        public List<SolicitacaoModel> GetByIdHardwareAtuador(uint idHardwareAtuador);

        bool Insert(SolicitacaoModel entity);
        bool Remove(int id);
        bool Update(SolicitacaoModel entity);
    }
}
