using api.Controllers;
using api.Models;
using api.Models.DTOs;

namespace api.BLL.Interfaces
{
    public interface IZapasService
    {
        // Existing methods
        Task VlozSestavuAsync(int idZapas, int idTymDomaci, int idTymHost, List<HracSestavaRequest> sestava);
        Task VlozSestavuTsqlAsync(int idZapas, int idTymDomaci, int idTymHost, List<HracSestavaRequest> sestava);

        // New methods
        Task<IEnumerable<Sezona>> GetSeasonsAsync();
        Task<IEnumerable<Tym>> GetTeamsAsync();
        Task<IEnumerable<Tym>> GetTeamsBySeasonAsync(int idSezona);
        Task<IEnumerable<ZapasDto>> FindMatchesAsync(ZapasFilterDto filter);
        Task<ZapasDto> CreateMatchAsync(CreateZapasDto createDto);
        Task<ZapasDto?> GetMatchDetailsAsync(int idZapas);
        Task SaveMatchAsync(UpdateZapasDto updateDto);
        Task DeleteMatchAsync(int idZapas);
        Task<IEnumerable<Hrac>> GetPlayersByTeamAsync(int idTym);
    }
}
