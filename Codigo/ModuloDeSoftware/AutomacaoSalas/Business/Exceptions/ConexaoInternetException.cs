using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Exceptions
{
    public class ConexaoInternetException : Exception
    {
        public ConexaoInternetException() : base() { }
        public ConexaoInternetException(string message) : base(message) { }
    }
}
