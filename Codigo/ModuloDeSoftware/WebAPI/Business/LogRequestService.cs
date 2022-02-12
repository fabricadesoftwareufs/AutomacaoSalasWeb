using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class LogRequestService : ILogRequestService
    {
        private readonly str_dbContext _context;
        public LogRequestService(str_dbContext context)
        {
            _context = context;
        }
        public bool Insert(LogRequestModel logRequestModel)
        {
            try
            {
                var entity = new Logrequest();
                _context.Add(SetEntity(logRequestModel));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e) { throw e; }
        }

        private static Logrequest SetEntity(LogRequestModel model)
        {
            Logrequest entity = new Logrequest
            {
                Id = model.Id,
                Ip = model.Ip,
                Url = model.Url,
                Date = model.Date,
                StatusCode = model.StatusCode,
                Input = model.Input,
            };
            return entity;
        }
    }
}
