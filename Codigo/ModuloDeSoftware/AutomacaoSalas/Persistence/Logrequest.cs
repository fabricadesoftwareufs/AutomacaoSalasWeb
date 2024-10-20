using System;
using System.Collections.Generic;

namespace Persistence;

public partial class Logrequest
{
    public int Id { get; set; }

    public string Ip { get; set; } = null!;

    public string Url { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Input { get; set; } = null!;

    public string StatusCode { get; set; } = null!;

    public string Origin { get; set; } = null!;
}
