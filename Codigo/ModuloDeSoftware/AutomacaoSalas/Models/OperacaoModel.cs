
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class OperacaoModel
    {
        public const int OPERACAO_LIGAR = 1;
        public const int OPERACAO_DESLIGAR = 2;

        public int Id { get; set; }

        [Display(Name = "Título")]
        [StringLength(50, ErrorMessage = "Máximo são 50 caracteres")]
        public string Titulo { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(200, ErrorMessage = "Máximo são 200 caracteres")]
        public string Descricao { get; set; }

    }
}
