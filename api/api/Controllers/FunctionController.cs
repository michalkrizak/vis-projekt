using Microsoft.AspNetCore.Mvc;
using api.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using api.Models;

namespace api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class FunctionController : ControllerBase { 

        private readonly VolejbalContext _context;

        public FunctionController(VolejbalContext context)
        {
        _context = context;
        }


        [HttpGet("tymy")]
        public async Task<IActionResult> GetTymy()
        {
            var tymy = await _context.Tyms
                .Select(t => new { idTym = t.IdTym, nazev = t.Nazev })
                .ToListAsync();

            return Ok(tymy);
        }

        [HttpGet("zapasy")]
        public async Task<IActionResult> GetZapasy()
        {
            var zapasy = await _context.Zapas
                .Include(z => z.IdTym1Navigation)
                .Include(z => z.IdTym2Navigation)
                .Select(z => new {
                    idZapas = z.IdZapas,
                    domaci = z.IdTym1Navigation.Nazev,
                    host = z.IdTym2Navigation.Nazev,
                    datum = z.Datum,
                    idTym1 = z.IdTym1,
                    idTym2 = z.IdTym2
                }).ToListAsync();

            return Ok(zapasy);
        }

        [HttpGet("hraci/byTym/{idTym}")]
        public async Task<IActionResult> GetHraciByTym(int idTym)
        {
            var hraci = await _context.Hracs
                .Where(h => h.IdTym == idTym)
                .Select(h => new { idHrac = h.IdHrac, jmeno = h.Jmeno })
                .ToListAsync();

            return Ok(hraci);
        }
    }
}
