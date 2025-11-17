using api.BLL.Interfaces;
using api.Controllers;
using api.DAL.Interfaces;
using api.Models;
using api.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace api.BLL.Services
{
    public class ZapasService : IZapasService
    {
        private readonly VolejbalContext _context;
        private readonly ISestavaZapasuDao _sestavaDao;
        private readonly IZapasDao _zapasDao;
        private readonly ISezonaDao _sezonaDao;
        private readonly ITymDao _tymDao;
        private readonly IHracDao _hracDao;

        public ZapasService(
            VolejbalContext context, 
            ISestavaZapasuDao sestavaDao,
            IZapasDao zapasDao,
            ISezonaDao sezonaDao,
            ITymDao tymDao,
            IHracDao hracDao)
        {
            _context = context;
            _sestavaDao = sestavaDao;
            _zapasDao = zapasDao;
            _sezonaDao = sezonaDao;
            _tymDao = tymDao;
            _hracDao = hracDao;
        }

        public async Task VlozSestavuAsync(int idZapas, int idTymDomaci, int idTymHost, List<HracSestavaRequest> sestava)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validace počtu hráčů
                ValidujPocetHracu(sestava, idTymDomaci, idTymHost);
                
                // Validace kapitánů
                ValidujKapitany(sestava, idTymDomaci, idTymHost);
                
                // Validace liberos
                ValidujLiberos(sestava, idTymDomaci, idTymHost);

                // Zpracování aktivních hráčů (hraje == true)
                foreach (var hrac in sestava.Where(s => s.Hraje))
                {
                    var existujici = await _context.SestavaZapasus
                        .FirstOrDefaultAsync(sz => sz.IdZapas == idZapas && sz.IdHrac == hrac.IdHrac);

                    if (existujici != null)
                    {
                        // UPDATE
                        existujici.IdTym = hrac.IdTym;
                        existujici.JeKapitan = hrac.JeKapitan;
                        existujici.JeLibero = hrac.JeLibero;
                    }
                    else
                    {
                        // INSERT
                        _context.SestavaZapasus.Add(new SestavaZapasu
                        {
                            IdZapas = idZapas,
                            IdHrac = hrac.IdHrac,
                            IdTym = hrac.IdTym,
                            JeKapitan = hrac.JeKapitan,
                            JeLibero = hrac.JeLibero
                        });
                    }
                }

                // DELETE neaktivních hráčů (hraje == false)
                var neaktivniIds = sestava.Where(s => !s.Hraje).Select(s => s.IdHrac).ToList();

                var neaktivni = await _context.SestavaZapasus
                    .Where(sz => sz.IdZapas == idZapas && neaktivniIds.Contains(sz.IdHrac))
                    .ToListAsync();

                _context.SestavaZapasus.RemoveRange(neaktivni);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task VlozSestavuTsqlAsync(int idZapas, int idTymDomaci, int idTymHost, List<HracSestavaRequest> sestava)
        {
            var connection = _context.Database.GetDbConnection();
            await using var command = connection.CreateCommand();

            command.CommandText = "VlozSestavu";
            command.CommandType = CommandType.StoredProcedure;

            // Parametry
            command.Parameters.Add(new SqlParameter("@IdZapas", idZapas));
            command.Parameters.Add(new SqlParameter("@IdTymDomaci", idTymDomaci));
            command.Parameters.Add(new SqlParameter("@IdTymHost", idTymHost));

            // Table-Valued Parameter
            var table = new DataTable();
            table.Columns.Add("IdHrac", typeof(int));
            table.Columns.Add("IdTym", typeof(int));
            table.Columns.Add("Hraje", typeof(int));
            table.Columns.Add("JeKapitan", typeof(int));
            table.Columns.Add("JeLibero", typeof(int));

            foreach (var hrac in sestava)
            {
                table.Rows.Add(
                    hrac.IdHrac,
                    hrac.IdTym,
                    hrac.Hraje ? 1 : 0,
                    hrac.JeKapitan ? 1 : 0,
                    hrac.JeLibero ? 1 : 0
                );
            }

            var sestavaParam = new SqlParameter("@Sestava", table)
            {
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.SestavaHraceTyp"
            };
            command.Parameters.Add(sestavaParam);

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }

        private void ValidujPocetHracu(List<HracSestavaRequest> sestava, int idTymDomaci, int idTymHost)
        {
            if (sestava.Count(s => s.IdTym == idTymDomaci && s.Hraje) < 6)
                throw new Exception("Domácí tým má méně než 6 hrajících hráčů.");

            if (sestava.Count(s => s.IdTym == idTymHost && s.Hraje) < 6)
                throw new Exception("Hostující tým má méně než 6 hrajících hráčů.");
        }

        private void ValidujKapitany(List<HracSestavaRequest> sestava, int idTymDomaci, int idTymHost)
        {
            if (sestava.Count(s => s.IdTym == idTymDomaci && s.JeKapitan) != 1)
                throw new Exception("Domácí tým musí mít právě jednoho kapitána.");

            if (sestava.Count(s => s.IdTym == idTymHost && s.JeKapitan) != 1)
                throw new Exception("Hostující tým musí mít právě jednoho kapitána.");
        }

        private void ValidujLiberos(List<HracSestavaRequest> sestava, int idTymDomaci, int idTymHost)
        {
            if (sestava.Count(s => s.IdTym == idTymDomaci && s.JeLibero) > 2)
                throw new Exception("Domácí tým má více než 2 libero.");

            if (sestava.Count(s => s.IdTym == idTymHost && s.JeLibero) > 2)
                throw new Exception("Hostující tým má více než 2 libero.");
        }

        // New methods implementation
        public async Task<IEnumerable<Sezona>> GetSeasonsAsync()
        {
            return await _sezonaDao.GetAllAsync();
        }

        public async Task<IEnumerable<Tym>> GetTeamsAsync()
        {
            return await _tymDao.GetAllAsync();
        }

        public async Task<IEnumerable<Tym>> GetTeamsBySeasonAsync(int idSezona)
        {
            return await _tymDao.GetTeamsBySeasonAsync(idSezona);
        }

        public async Task<IEnumerable<ZapasDto>> FindMatchesAsync(ZapasFilterDto filter)
        {
            DateOnly? datum = null;
            if (!string.IsNullOrEmpty(filter.Datum))
            {
                datum = DateOnly.Parse(filter.Datum);
            }

            var zapasy = await _zapasDao.FindMatchesAsync(datum, filter.IdSezona, filter.IdTym1, filter.IdTym2);

            return zapasy.Select(z => new ZapasDto
            {
                IdZapas = z.IdZapas,
                Datum = z.Datum.ToString("yyyy-MM-dd"),
                IdTym1 = z.IdTym1,
                IdTym2 = z.IdTym2,
                IdSezona = z.IdSezona,
                SkoreTym1 = z.SkoreTym1,
                SkoreTym2 = z.SkoreTym2,
                Vitez = z.Vitez,
                NazevTym1 = z.IdTym1Navigation?.Nazev,
                NazevTym2 = z.IdTym2Navigation?.Nazev,
                NazevSezona = z.IdSezonaNavigation?.Nazev,
                NazevVitez = z.VitezNavigation?.Nazev
            });
        }

        public async Task<ZapasDto> CreateMatchAsync(CreateZapasDto createDto)
        {
            var datum = DateOnly.Parse(createDto.Datum);
            
            // Get next available ID
            var maxId = await _context.Zapas.MaxAsync(z => (int?)z.IdZapas) ?? 0;
            var newId = maxId + 1;

            var newZapas = new Zapa
            {
                IdZapas = newId,
                Datum = datum,
                IdSezona = createDto.IdSezona,
                IdTym1 = createDto.IdTym1,
                IdTym2 = createDto.IdTym2
            };

            await _zapasDao.CreateAsync(newZapas);

            var created = await _zapasDao.GetByIdWithDetailsAsync(newId);

            return new ZapasDto
            {
                IdZapas = created.IdZapas,
                Datum = created.Datum.ToString("yyyy-MM-dd"),
                IdTym1 = created.IdTym1,
                IdTym2 = created.IdTym2,
                IdSezona = created.IdSezona,
                SkoreTym1 = created.SkoreTym1,
                SkoreTym2 = created.SkoreTym2,
                Vitez = created.Vitez,
                NazevTym1 = created.IdTym1Navigation?.Nazev,
                NazevTym2 = created.IdTym2Navigation?.Nazev,
                NazevSezona = created.IdSezonaNavigation?.Nazev,
                NazevVitez = created.VitezNavigation?.Nazev
            };
        }

        public async Task<ZapasDto?> GetMatchDetailsAsync(int idZapas)
        {
            var zapas = await _zapasDao.GetByIdWithDetailsAsync(idZapas);
            
            if (zapas == null)
                return null;

            return new ZapasDto
            {
                IdZapas = zapas.IdZapas,
                Datum = zapas.Datum.ToString("yyyy-MM-dd"),
                IdTym1 = zapas.IdTym1,
                IdTym2 = zapas.IdTym2,
                IdSezona = zapas.IdSezona,
                SkoreTym1 = zapas.SkoreTym1,
                SkoreTym2 = zapas.SkoreTym2,
                Vitez = zapas.Vitez,
                NazevTym1 = zapas.IdTym1Navigation?.Nazev,
                NazevTym2 = zapas.IdTym2Navigation?.Nazev,
                NazevSezona = zapas.IdSezonaNavigation?.Nazev,
                NazevVitez = zapas.VitezNavigation?.Nazev
            };
        }

        public async Task SaveMatchAsync(UpdateZapasDto updateDto)
        {
            var zapas = await _zapasDao.GetByIdAsync(updateDto.IdZapas);
            
            if (zapas == null)
                throw new Exception("Zápas nebyl nalezen.");

            zapas.Datum = DateOnly.Parse(updateDto.Datum);
            zapas.IdSezona = updateDto.IdSezona;
            zapas.IdTym1 = updateDto.IdTym1;
            zapas.IdTym2 = updateDto.IdTym2;
            zapas.SkoreTym1 = updateDto.SkoreTym1;
            zapas.SkoreTym2 = updateDto.SkoreTym2;
            zapas.Vitez = updateDto.Vitez;

            await _zapasDao.UpdateAsync(zapas);
        }

        public async Task DeleteMatchAsync(int idZapas)
        {
            // First delete all lineups
            await _sestavaDao.DeleteByZapasIdAsync(idZapas);
            
            // Then delete the match
            await _zapasDao.DeleteAsync(idZapas);
        }

        public async Task<IEnumerable<Hrac>> GetPlayersByTeamAsync(int idTym)
        {
            return await _hracDao.GetPlayersByTeamAsync(idTym);
        }

        public async Task<IEnumerable<HracSestavaDto>> GetMatchLineupsAsync(int idZapas)
        {
            var sestavy = await _context.SestavaZapasus
                .Include(sz => sz.IdHracNavigation)
                .Where(sz => sz.IdZapas == idZapas)
                .ToListAsync();

            return sestavy.Select(sz => new HracSestavaDto
            {
                IdHrac = sz.IdHrac,
                Jmeno = sz.IdHracNavigation.Jmeno,
                Prijmeni = sz.IdHracNavigation.Prijmeni,
                IdTym = sz.IdTym,
                Hraje = true, // Pokud je v sestavě, hraje
                JeKapitan = sz.JeKapitan ?? false,
                JeLibero = sz.JeLibero ?? false
            });
        }
    }
}
