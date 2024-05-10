using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UserService.Model;

[Route("user-service")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _userService;

    public UsersController(IUsersService userService)
    {
        _userService = userService;
    }
    
    [Authorize] 
    [HttpPost("follow")]
    public async Task<IActionResult> FollowUser([FromBody] int followedUserId)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("User ID not found in token");
        }

        var userId = int.Parse(userIdClaim.Value);

        if (await _userService.FollowUserAsync(userId, followedUserId))
            return Ok();

        return BadRequest("Unable to follow the user (possibly already following or user not found).");
    }
    
    [HttpGet("{id}/follows")]
    public async Task<IActionResult> GetFollowedUserIds(int id)
    {
        var followedUserIds = await _userService.GetFollowedUserIdsAsync(id);

        if (followedUserIds == null || followedUserIds.Count == 0)
            return NotFound($"User with ID {id} is not following anyone.");

        return Ok(followedUserIds);
    }
    

    [HttpGet("user/{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound($"User with ID {id} not found.");
        }

        return Ok(user);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers([FromQuery] string name)
    {
        var users = await _userService.SearchUsersAsync(name);

        if (users == null || !users.Any())
        {
            return NotFound("No users matching the criteria were found.");
        }
        
        var userDtos = users.Select(user => new UserDto
        {
            Id = user.UserId,
            Name = user.Username
        }).ToList();

        return Ok(userDtos);
    }
}

