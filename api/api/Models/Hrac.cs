using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Hrac
{
    public int IdHrac { get; set; }

    public string Jmeno { get; set; } = null!;

    public string Prijmeni { get; set; } = null!;

    public string? Pozice { get; set; }

    public int? IdTym { get; set; }

    public virtual Tym? IdTymNavigation { get; set; }

    public virtual ICollection<SestavaZapasu> SestavaZapasus { get; set; } = new List<SestavaZapasu>();
}
