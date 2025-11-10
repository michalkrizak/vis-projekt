using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Tym
{
    public int IdTym { get; set; }

    public string Nazev { get; set; } = null!;

    public string? Stadion { get; set; }

    public string? Trener { get; set; }

    public DateOnly? DatumZalozeni { get; set; }

    public virtual ICollection<Hrac> Hracs { get; set; } = new List<Hrac>();

    public virtual ICollection<SestavaZapasu> SestavaZapasus { get; set; } = new List<SestavaZapasu>();

    public virtual ICollection<Zapa> ZapaIdTym1Navigations { get; set; } = new List<Zapa>();

    public virtual ICollection<Zapa> ZapaIdTym2Navigations { get; set; } = new List<Zapa>();

    public virtual ICollection<Zapa> ZapaVitezNavigations { get; set; } = new List<Zapa>();

    public virtual ICollection<Sezona> IdSezonas { get; set; } = new List<Sezona>();
}
