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

    public async Task<IEnumerable<User>> SearchUsersAsync(string name)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(u => EF.Functions.ILike(u.Username, $"%{name}%"));
        }

        return await query.ToListAsync();
    }
    
    public async Task<bool> FollowUserAsync(int followerId, int followedId)
    {
        if (followerId == followedId)
            return false;

        var existingFollow = await _context.FollowedUsers
            .AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followedId);

        if (existingFollow)
        {
            return false;
        }

        var newFollow = new FollowedUser
        {
            FollowerId = followerId,
            FollowedId = followedId
        };

        _context.FollowedUsers.Add(newFollow);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<int>> GetFollowedUserIdsAsync(int followerId)
    {
        return await _context.FollowedUsers
            .Where(f => f.FollowerId == followerId)
            .Select(f => f.FollowedId)
            .ToListAsync();
    }
}

public interface IUsersService
{
    Task<User> CreateUserAsync(User user);
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> SearchUsersAsync(string name);
    Task<bool> FollowUserAsync(int followerId, int followedId);
    Task<List<int>> GetFollowedUserIdsAsync(int followerId);
}


