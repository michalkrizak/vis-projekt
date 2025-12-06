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

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
    {
        // Kontrola, zda uživatel již existuje
        var userExists = await _loginDao.UserExistsAsync(
            registerRequest.Jmeno, 
            registerRequest.Prijmeni
        );

        if (userExists)
        {
            throw new InvalidOperationException("Uživatel s tímto jménem a příjmením již existuje.");
        }

        // Validace délky hesla
        if (string.IsNullOrWhiteSpace(registerRequest.Heslo) || registerRequest.Heslo.Length < 6)
        {
            throw new ArgumentException("Heslo musí mít alespoň 6 znaků.");
        }

        // Vytvoření nového uživatele
        var newUser = await _loginDao.CreateUserAsync(
            registerRequest.Jmeno,
            registerRequest.Prijmeni,
            registerRequest.Heslo
        );

        return new LoginResponseDto
        {
            IdUzivatel = newUser.IdUzivatel,
            Jmeno = newUser.Jmeno,
            Prijmeni = newUser.Prijmeni,
            Message = "Registrace proběhla úspěšně."
        };
    }
}
