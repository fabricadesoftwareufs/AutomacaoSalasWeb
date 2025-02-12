using System;

namespace Service.Exceptions
{
    public class ConexaoInternetException : Exception
    {
        public ConexaoInternetException() : base("Erro na conexão de internet.")
        {
        }

        public ConexaoInternetException(string message) : base(message)
        {
        }

        public ConexaoInternetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}