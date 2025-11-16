using api.DAL.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class HracRepository : IHracDao
    {
        private readonly VolejbalContext _context;

        public HracRepository(VolejbalContext context)
        {
            _context = context;
        }

        public async Task<Hrac?> GetByIdAsync(int id)
        {
            return await _context.Hracs.FindAsync(id);
        }

        public async Task<IEnumerable<Hrac>> GetAllAsync()
        {
            return await _context.Hracs.ToListAsync();
        }

        public async Task<IEnumerable<Hrac>> GetPlayersByTeamAsync(int idTym)
        {
            return await _context.Hracs
                .Where(h => h.IdTym == idTym)
                .OrderBy(h => h.Jmeno)
                .ThenBy(h => h.Prijmeni)
                .ToListAsync();
        }

        public async Task<Hrac> CreateAsync(Hrac entity)
        {
            _context.Hracs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Hrac entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Hracs.FindAsync(id);
            if (entity != null)
            {
                _context.Hracs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
