using api.DAL.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class TymRepository : ITymDao
    {
        private readonly VolejbalContext _context;

        public TymRepository(VolejbalContext context)
        {
            _context = context;
        }

        public async Task<Tym?> GetByIdAsync(int id)
        {
            return await _context.Tyms.FindAsync(id);
        }

        public async Task<IEnumerable<Tym>> GetAllAsync()
        {
            return await _context.Tyms.OrderBy(t => t.Nazev).ToListAsync();
        }

        public async Task<IEnumerable<Tym>> GetTeamsBySeasonAsync(int idSezona)
        {
            return await _context.Tyms
                .Where(t => t.IdSezonas.Any(s => s.IdSezona == idSezona))
                .OrderBy(t => t.Nazev)
                .ToListAsync();
        }

        public async Task<Tym> CreateAsync(Tym entity)
        {
            _context.Tyms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Tym entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Tyms.FindAsync(id);
            if (entity != null)
            {
                _context.Tyms.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
