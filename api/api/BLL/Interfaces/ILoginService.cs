using api.Models.DTOs;
using System.Threading.Tasks;

namespace api.BLL.Interfaces;

public interface ILoginService
{
    Task<LoginResponseDto> AuthenticateAsync(LoginRequestDto loginRequest);
}
