using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModel
{
    public class ConexaoInternetViewModel
    {
        public ConexaoInternetViewModel()
        {
        }

        [Display(Name = "Código")]
        public uint Id { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        public string ConfirmarSenha { get; set; }

        public uint IdBloco { get; set; }

        public List<BlocoViewModel> Blocos { get; set; }
    }
}