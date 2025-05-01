using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Exceptions
{
    public class MarcaEquipamentoException : Exception
    {

        public MarcaEquipamentoException() : base("Erro na Marca do Equipamento.")
        {
        }

        public MarcaEquipamentoException(string message) : base(message)
        {
        }

        public MarcaEquipamentoException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
