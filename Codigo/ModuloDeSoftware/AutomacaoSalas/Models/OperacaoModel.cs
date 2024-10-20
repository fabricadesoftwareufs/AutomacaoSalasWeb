
namespace Model
{
    public class OperacaoModel
    {
        public const int OPERACAO_LIGAR = 1;
        public const int OPERACAO_DESLIGAR = 2;

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

    }
}
