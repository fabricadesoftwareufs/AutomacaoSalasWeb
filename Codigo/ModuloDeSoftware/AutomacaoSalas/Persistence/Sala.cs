using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Sala
{
    public uint Id { get; set; }

    public string Titulo { get; set; } = null!;

    public uint IdBloco { get; set; }

    public virtual ICollection<Conexaointernetsala> Conexaointernetsalas { get; set; } = new List<Conexaointernetsala>();

    public virtual ICollection<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();

    public virtual ICollection<Hardwaredesala> Hardwaredesalas { get; set; } = new List<Hardwaredesala>();

    public virtual ICollection<Horariosala> Horariosalas { get; set; } = new List<Horariosala>();

    public virtual Bloco IdBlocoNavigation { get; set; } = null!;

    public virtual ICollection<Planejamento> Planejamentos { get; set; } = new List<Planejamento>();

    public virtual ICollection<Salaparticular> Salaparticulars { get; set; } = new List<Salaparticular>();
}
