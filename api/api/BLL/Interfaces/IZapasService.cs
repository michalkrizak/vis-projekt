using api.Controllers;

namespace api.BLL.Interfaces
{
    public interface IZapasService
    {
        Task VlozSestavuAsync(int idZapas, int idTymDomaci, int idTymHost, List<HracSestavaRequest> sestava);
        Task VlozSestavuTsqlAsync(int idZapas, int idTymDomaci, int idTymHost, List<HracSestavaRequest> sestava);
    }
}
