using api.Models;

namespace api.DAL.Interfaces
{
    public interface ITymDao : IDao<Tym>
    {
        Task<IEnumerable<Tym>> GetTeamsBySeasonAsync(int idSezona);
    }
}
