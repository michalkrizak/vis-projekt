using System;
using System.Collections.Generic;

namespace api.Models;

public partial class SestavaZapasu
{
    public int IdZapas { get; set; }

    public int IdHrac { get; set; }

    public int IdTym { get; set; }

    public string? Pozice { get; set; }

    public bool? JeKapitan { get; set; }

    public bool? JeLibero { get; set; }

    public int? Sety { get; set; }

    public int? Body { get; set; }

    public int? Esa { get; set; }

    public virtual Hrac IdHracNavigation { get; set; } = null!;

    public virtual Tym IdTymNavigation { get; set; } = null!;

    public virtual Zapa IdZapasNavigation { get; set; } = null!;
}
