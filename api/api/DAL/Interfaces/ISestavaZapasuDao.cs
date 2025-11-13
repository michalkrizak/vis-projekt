using api.Models;

namespace api.DAL.Interfaces
{
    public interface ISestavaZapasuDao : IDao<SestavaZapasu>
    {
        Task<IEnumerable<SestavaZapasu>> GetByZapasIdAsync(int idZapas);
        Task<bool> ExistsSestavaForZapasAsync(int idZapas);
        Task DeleteByZapasIdAsync(int idZapas);
    }
}
