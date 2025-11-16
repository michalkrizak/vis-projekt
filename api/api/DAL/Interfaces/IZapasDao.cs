using api.Models;

namespace api.DAL.Interfaces
{
    public interface IZapasDao : IDao<Zapa>
    {
        Task<IEnumerable<Zapa>> FindMatchesAsync(DateOnly? datum, int? idSezona, int? idTym1, int? idTym2);
        Task<Zapa?> GetByIdWithDetailsAsync(int id);
    }
}
