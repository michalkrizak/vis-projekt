using Microsoft.AspNetCore.Mvc;
using api.BLL.Interfaces;
using api.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var response = await _loginService.AuthenticateAsync(loginRequest);
                
                // Uložení do SESSION (druhý způsob persistence kromě SQL databáze)
                HttpContext.Session.SetString("UserId", response.IdUzivatel.ToString());
                HttpContext.Session.SetString("Username", response.Jmeno);
                HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                var response = await _loginService.RegisterAsync(registerRequest);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Smazání session dat
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("session-info")]
        public IActionResult GetSessionInfo()
        {
            // Přečtení dat ze session
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("Username");
            var loginTime = HttpContext.Session.GetString("LoginTime");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "No active session" });
            }

            return Ok(new
            {
                userId,
                username,
                loginTime,
                sessionId = HttpContext.Session.Id
            });
        }
    }
}
