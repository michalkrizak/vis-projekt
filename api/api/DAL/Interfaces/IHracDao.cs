using api.Models;

namespace api.DAL.Interfaces
{
    public interface IHracDao : IDao<Hrac>
    {
        Task<IEnumerable<Hrac>> GetPlayersByTeamAsync(int idTym);
    }
}
