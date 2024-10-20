using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Organizacao
{
    public uint Id { get; set; }

    public string Cnpj { get; set; } = null!;

    public string RazaoSocial { get; set; } = null!;

    public virtual ICollection<Bloco> Blocos { get; set; } = new List<Bloco>();

    public virtual ICollection<Usuarioorganizacao> Usuarioorganizacaos { get; set; } = new List<Usuarioorganizacao>();
}
