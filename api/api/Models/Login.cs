using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Login
{
    public int IdUzivatel { get; set; }

    public string Jmeno { get; set; } = null!;

    public string Prijmeni { get; set; } = null!;

    public string Heslo { get; set; } = null!;
}
