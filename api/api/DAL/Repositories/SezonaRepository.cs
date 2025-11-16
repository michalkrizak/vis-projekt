using api.DAL.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class SezonaRepository : ISezonaDao
    {
        private readonly VolejbalContext _context;

        public SezonaRepository(VolejbalContext context)
        {
            _context = context;
        }

        public async Task<Sezona?> GetByIdAsync(int id)
        {
            return await _context.Sezonas.FindAsync(id);
        }

        public async Task<IEnumerable<Sezona>> GetAllAsync()
        {
            return await _context.Sezonas.OrderByDescending(s => s.Rok).ToListAsync();
        }

        public async Task<Sezona> CreateAsync(Sezona entity)
        {
            _context.Sezonas.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Sezona entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Sezonas.FindAsync(id);
            if (entity != null)
            {
                _context.Sezonas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
