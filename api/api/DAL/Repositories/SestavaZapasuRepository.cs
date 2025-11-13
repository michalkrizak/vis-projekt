using api.DAL.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL.Repositories
{
    public class SestavaZapasuRepository : ISestavaZapasuDao
    {
        private readonly VolejbalContext _context;

        public SestavaZapasuRepository(VolejbalContext context)
        {
            _context = context;
        }

        public async Task<SestavaZapasu?> GetByIdAsync(int id)
        {
            return await _context.SestavaZapasus.FindAsync(id);
        }

        public async Task<IEnumerable<SestavaZapasu>> GetAllAsync()
        {
            return await _context.SestavaZapasus.ToListAsync();
        }

        public async Task<SestavaZapasu> CreateAsync(SestavaZapasu entity)
        {
            _context.SestavaZapasus.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(SestavaZapasu entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.SestavaZapasus.FindAsync(id);
            if (entity != null)
            {
                _context.SestavaZapasus.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SestavaZapasu>> GetByZapasIdAsync(int idZapas)
        {
            return await _context.SestavaZapasus
                .Where(s => s.IdZapas == idZapas)
                .ToListAsync();
        }

        public async Task<bool> ExistsSestavaForZapasAsync(int idZapas)
        {
            return await _context.SestavaZapasus.AnyAsync(s => s.IdZapas == idZapas);
        }

        public async Task DeleteByZapasIdAsync(int idZapas)
        {
            var sestavy = await _context.SestavaZapasus
                .Where(s => s.IdZapas == idZapas)
                .ToListAsync();
            
            _context.SestavaZapasus.RemoveRange(sestavy);
            await _context.SaveChangesAsync();
        }
    }
}
