using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Operacao
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descricao { get; set; }

    public virtual ICollection<Codigoinfravermelho> Codigoinfravermelhos { get; set; } = new List<Codigoinfravermelho>();
}
