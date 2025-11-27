using api.Models;
using System.Threading.Tasks;

namespace api.DAL.Interfaces;

public interface ILoginDao
{
    Task<Login?> GetUserByCredentialsAsync(string jmeno, string prijmeni, string heslo);
}
