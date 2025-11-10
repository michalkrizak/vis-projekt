using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Zapa
{
    public int IdZapas { get; set; }

    public DateOnly Datum { get; set; }

    public int IdTym1 { get; set; }

    public int IdTym2 { get; set; }

    public int IdSezona { get; set; }

    public int? SkoreTym1 { get; set; }

    public int? SkoreTym2 { get; set; }

    public int? Vitez { get; set; }

    public virtual Sezona IdSezonaNavigation { get; set; } = null!;

    public virtual Tym IdTym1Navigation { get; set; } = null!;

    public virtual Tym IdTym2Navigation { get; set; } = null!;

    public virtual ICollection<SestavaZapasu> SestavaZapasus { get; set; } = new List<SestavaZapasu>();

    public virtual Tym? VitezNavigation { get; set; }
}
