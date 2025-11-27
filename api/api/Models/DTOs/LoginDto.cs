namespace api.Models.DTOs;

public class LoginRequestDto
{
    public string Jmeno { get; set; } = null!;
    public string Prijmeni { get; set; } = null!;
    public string Heslo { get; set; } = null!;
}

public class LoginResponseDto
{
    public int IdUzivatel { get; set; }
    public string Jmeno { get; set; } = null!;
    public string Prijmeni { get; set; } = null!;
    public string Message { get; set; } = null!;
}
