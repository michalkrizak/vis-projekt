using api.DAL.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class ZapasRepository : IZapasDao
    {
        private readonly VolejbalContext _context;

        public ZapasRepository(VolejbalContext context)
        {
            _context = context;
        }

        public async Task<Zapa?> GetByIdAsync(int id)
        {
            return await _context.Zapas.FindAsync(id);
        }

        public async Task<Zapa?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Zapas
                .Include(z => z.IdSezonaNavigation)
                .Include(z => z.IdTym1Navigation)
                .Include(z => z.IdTym2Navigation)
                .Include(z => z.VitezNavigation)
                .FirstOrDefaultAsync(z => z.IdZapas == id);
        }

        public async Task<IEnumerable<Zapa>> GetAllAsync()
        {
            return await _context.Zapas
                .Include(z => z.IdSezonaNavigation)
                .Include(z => z.IdTym1Navigation)
                .Include(z => z.IdTym2Navigation)
                .ToListAsync();
        }

        public async Task<IEnumerable<Zapa>> FindMatchesAsync(DateOnly? datum, int? idSezona, int? idTym1, int? idTym2)
        {
            var query = _context.Zapas
                .Include(z => z.IdSezonaNavigation)
                .Include(z => z.IdTym1Navigation)
                .Include(z => z.IdTym2Navigation)
                .AsQueryable();

            if (datum.HasValue)
            {
                query = query.Where(z => z.Datum == datum.Value);
            }

            if (idSezona.HasValue)
            {
                query = query.Where(z => z.IdSezona == idSezona.Value);
            }

            if (idTym1.HasValue)
            {
                query = query.Where(z => z.IdTym1 == idTym1.Value);
            }

            if (idTym2.HasValue)
            {
                query = query.Where(z => z.IdTym2 == idTym2.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Zapa> CreateAsync(Zapa entity)
        {
            _context.Zapas.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Zapa entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Zapas.FindAsync(id);
            if (entity != null)
            {
                _context.Zapas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
