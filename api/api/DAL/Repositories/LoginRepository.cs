using api.DAL.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace api.DAL.Repositories;

public class LoginRepository : ILoginDao
{
    private readonly VolejbalContext _context;

    public LoginRepository(VolejbalContext context)
    {
        _context = context;
    }

    public async Task<Login?> GetUserByCredentialsAsync(string jmeno, string prijmeni, string heslo)
    {
        return await _context.Logins
            .FirstOrDefaultAsync(u => u.Jmeno == jmeno && u.Prijmeni == prijmeni && u.Heslo == heslo);
    }
}
