using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;

namespace Service
{
    public class LogRequestService : ILogRequestService
    {
        private readonly SalasDBContext _context;
        public LogRequestService(SalasDBContext context)
        {
            _context = context;
        }
        public bool Insert(LogRequestModel logRequestModel)
        {
            try
            {
                var entity = new Logrequest();
                _context.Logrequests.Add(SetEntity(logRequestModel));
                return _context.SaveChanges() == 1;
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
                Origin = model.Origin,
            };
            return entity;
        }
    }
}
