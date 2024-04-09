using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UserService.Model;

[Route("user-service/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UsersService _userService;

    public UsersController(UsersService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        return Ok(user);
    }

    // GET: api/Users/search?name={name}&email={email}
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<User>>> SearchUsers([FromQuery] string name, [FromQuery] string email)
    {
        var users = await _userService.SearchUsersAsync(name, email);

        if (users == null || !users.Any())
        {
            return NotFound("No users matching the criteria were found.");
        }

        return Ok(users);
    }
}
