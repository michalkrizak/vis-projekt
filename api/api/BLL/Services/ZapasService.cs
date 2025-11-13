using api.BLL.Interfaces;
using api.Controllers;
using api.DAL.Interfaces;
using api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace api.BLL.Services
{
    public class ZapasService : IZapasService
    {
        private readonly VolejbalContext _context;
        private readonly ISestavaZapasuDao _sestavaDao;

        public ZapasService(VolejbalContext context, ISestavaZapasuDao sestavaDao)
        {
            _context = context;
            _sestavaDao = sestavaDao;
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
    }
}
