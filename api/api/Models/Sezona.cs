using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Sezona
{
    public int IdSezona { get; set; }

    public int Rok { get; set; }

    public string? Nazev { get; set; }

    public virtual ICollection<Zapa> Zapas { get; set; } = new List<Zapa>();

    public virtual ICollection<Tym> IdTyms { get; set; } = new List<Tym>();
}
