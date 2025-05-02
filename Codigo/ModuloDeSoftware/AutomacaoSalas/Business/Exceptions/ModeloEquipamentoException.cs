using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Exceptions
{
    internal class ModeloEquipamentoException : Exception
    {
        public ModeloEquipamentoException() : base("Erro no Modelo do Equipamento.")
        {
        }
        public ModeloEquipamentoException(string message) : base(message)
        {
        }
        public ModeloEquipamentoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
