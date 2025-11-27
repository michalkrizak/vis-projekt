using api.BLL.Interfaces;
using api.DAL.Interfaces;
using api.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace api.BLL.Services;

public class LoginService : ILoginService
{
    private readonly ILoginDao _loginDao;

    public LoginService(ILoginDao loginDao)
    {
        _loginDao = loginDao;
    }

    public async Task<LoginResponseDto> AuthenticateAsync(LoginRequestDto loginRequest)
    {
        var user = await _loginDao.GetUserByCredentialsAsync(
            loginRequest.Jmeno, 
            loginRequest.Prijmeni, 
            loginRequest.Heslo
        );

        if (user == null)
        {
            throw new UnauthorizedAccessException("Neplatné přihlašovací údaje.");
        }

        return new LoginResponseDto
        {
            IdUzivatel = user.IdUzivatel,
            Jmeno = user.Jmeno,
            Prijmeni = user.Prijmeni,
            Message = "Přihlášení proběhlo úspěšně."
        };
    }
}
