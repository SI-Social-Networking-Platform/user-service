using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserService.Model;

[Route("user-service/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IConfiguration _config;

    public AuthController(AuthService authService, IConfiguration config)
    {
        _authService = authService;
        _config = config;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        var userToCreate = new User
        {
            Username = request.Username,
            Email = request.Email
        };

        var createdUser = await _authService.Register(userToCreate, request.Password);

        if (createdUser == null)
        {
            return BadRequest("User cannot be created.");
        }

        var token = GenerateJwtToken(createdUser);

        return StatusCode(201, new
        {
            Token = token,
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.Login(request.Username, request.Password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);

        return Ok(new
        {
            Token = token,
        });
    }
}
