using Microsoft.AspNetCore.Mvc;
using api.BLL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using api.Models;
using api.Models.DTOs;

namespace api.Controllers
{
    [Route("api/zapas")]
    [ApiController]
    public class ZapasController : ControllerBase
    {
        private readonly IZapasService _zapasService;

        public ZapasController(IZapasService zapasService)
        {
            _zapasService = zapasService;
        }

        // F1: GetSeasons
        [HttpGet("seasons")]
        public async Task<IActionResult> GetSeasons()
        {
            try
            {
                var seasons = await _zapasService.GetSeasonsAsync();
                return Ok(seasons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F2: GetTeams
        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams([FromQuery] int? idSezona)
        {
            try
            {
                var teams = idSezona.HasValue 
                    ? await _zapasService.GetTeamsBySeasonAsync(idSezona.Value)
                    : await _zapasService.GetTeamsAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F3: FindMatch
        [HttpPost("find")]
        public async Task<IActionResult> FindMatches([FromBody] ZapasFilterDto filter)
        {
            try
            {
                var matches = await _zapasService.FindMatchesAsync(filter);
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F4: CreateMatch
        [HttpPost("create")]
        public async Task<IActionResult> CreateMatch([FromBody] CreateZapasDto createDto)
        {
            try
            {
                var match = await _zapasService.CreateMatchAsync(createDto);
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F6: GetMatchDetails
        [HttpGet("{idZapas}")]
        public async Task<IActionResult> GetMatchDetails(int idZapas)
        {
            try
            {
                var match = await _zapasService.GetMatchDetailsAsync(idZapas);
                if (match == null)
                    return NotFound(new { error = "Zápas nebyl nalezen." });
                
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F7: SaveMatch
        [HttpPut("save")]
        public async Task<IActionResult> SaveMatch([FromBody] UpdateZapasDto updateDto)
        {
            try
            {
                await _zapasService.SaveMatchAsync(updateDto);
                return Ok(new { message = "Zápas byl úspěšně uložen." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F8: GetPlayersByTeam
        [HttpGet("players/{idTym}")]
        public async Task<IActionResult> GetPlayersByTeam(int idTym)
        {
            try
            {
                var players = await _zapasService.GetPlayersByTeamAsync(idTym);
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F9a: GetMatchLineups
        [HttpGet("{idZapas}/lineups")]
        public async Task<IActionResult> GetMatchLineups(int idZapas)
        {
            try
            {
                var lineups = await _zapasService.GetMatchLineupsAsync(idZapas);
                return Ok(lineups);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F9: SaveMatchLineups (existing vloz-sestavu)
        [HttpPost("lineups")]
        public async Task<IActionResult> SaveMatchLineups([FromBody] VlozSestavuRequest request)
        {
            try
            {
                await _zapasService.VlozSestavuAsync(
                    request.IdZapas,
                    request.IdTymDomaci,
                    request.IdTymHost,
                    request.Sestava
                );

                return Ok(new { message = "Sestava úspěšně uložena." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // F10: DeleteMatch
        [HttpDelete("{idZapas}")]
        public async Task<IActionResult> DeleteMatch(int idZapas)
        {
            try
            {
                await _zapasService.DeleteMatchAsync(idZapas);
                return Ok(new { message = "Zápas byl úspěšně smazán." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Legacy endpoint - kept for backward compatibility
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

        // Legacy endpoint - kept for backward compatibility
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
