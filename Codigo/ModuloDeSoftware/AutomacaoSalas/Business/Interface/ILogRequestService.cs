using Model.ViewModel;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ILogRequestService
    {
        bool Insert(LogRequestModel entity);

    }
}
