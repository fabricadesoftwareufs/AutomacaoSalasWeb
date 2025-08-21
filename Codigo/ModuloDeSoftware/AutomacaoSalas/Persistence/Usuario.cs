﻿using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Usuario
{
    public uint Id { get; set; }

    public string Cpf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public DateTime? DataNascimento { get; set; }

    public virtual ICollection<Horariosala> Horariosalas { get; set; } = new List<Horariosala>();

    public virtual ICollection<Monitoramento> Monitoramentos { get; set; } = new List<Monitoramento>();

    public virtual ICollection<Planejamento> Planejamentos { get; set; } = new List<Planejamento>();

    public virtual ICollection<Salaparticular> Salaparticulars { get; set; } = new List<Salaparticular>();

    public virtual ICollection<Usuarioorganizacao> Usuarioorganizacaos { get; set; } = new List<Usuarioorganizacao>();
}
