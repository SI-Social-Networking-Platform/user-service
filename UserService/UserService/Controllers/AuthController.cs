using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Model;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(string username, string password)
    {
    
        User userToCreate = new User
        {
            Username = username,
            Email = username
        };

        var createdUser = await _authService.Register(userToCreate, password);
        return StatusCode(201, createdUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(string username, string password)
    {
        var user = await _authService.Login(username, password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok("Login successful!");
    }
}