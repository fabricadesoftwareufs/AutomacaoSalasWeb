using System;

namespace Model
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Senha { get; set; }
        public int TipoUsuarioId { get; set; }
    }
}
