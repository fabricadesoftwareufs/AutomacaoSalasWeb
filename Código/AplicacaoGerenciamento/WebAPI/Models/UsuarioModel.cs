using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        [Required]
        public string Senha { get; set; }
        public int TipoUsuarioId { get; set; }
    }
}
