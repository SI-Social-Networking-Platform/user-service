using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService;
using UserService.Model;

public class UsersService
{
    private readonly DataContext _context;

    public UsersService(DataContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> SearchUsersAsync(string name, string email)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => EF.Functions.ILike(u.Username, $"%{name}%"));
        }

        if (!string.IsNullOrEmpty(email))
        {
            query = query.Where(u => EF.Functions.ILike(u.Email, email));
        }

        return await query.ToListAsync();
    }
}
