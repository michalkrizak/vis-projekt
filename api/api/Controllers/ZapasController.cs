using Microsoft.AspNetCore.Mvc;
using api.BLL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using api.Models;

namespace api.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class ZapasController : ControllerBase
    {
        private readonly IZapasService _zapasService;

        public ZapasController(IZapasService zapasService)
        {
            _zapasService = zapasService;
        }

        [HttpPost("vloz-sestavu")]
        public async Task<IActionResult> VlozSestavu([FromBody] VlozSestavuRequest request)
        {
            try
            {

                await _zapasService.VlozSestavuAsync(
                    request.IdZapas,
                    request.IdTymDomaci,
                    request.IdTymHost,
                    request.Sestava
                );

                return Ok(new { Message = "Sestava úspěšně uložena." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("vloz-sestavu-tsql")]
        public async Task<IActionResult> VlozSestavuPresTsql([FromBody] VlozSestavuRequest request)
        {
            try
            {
                await _zapasService.VlozSestavuTsqlAsync(request.IdZapas, request.IdTymDomaci, request.IdTymHost, request.Sestava);
                return Ok(new { message = "Sestava úspěšně vložena přes T-SQL proceduru." });
            }
            catch (SqlException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }


    // DTO pro příjem celé žádosti z frontendu/postmana
    public class VlozSestavuRequest
    {
        public int IdZapas { get; set; }
        public int IdTymDomaci { get; set; }
        public int IdTymHost { get; set; }
        public List<HracSestavaRequest> Sestava { get; set; }
    }

    public class HracSestavaRequest
    {
        public int IdHrac { get; set; }
        public int IdTym { get; set; }
        public bool JeKapitan { get; set; }
        public bool JeLibero { get; set; }
        public bool Hraje { get; set; }
    }
}
