using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using task_management_backend.Data;
using task_management_backend.Helpers;
using task_management_backend.Models;
using task_management_backend.Models.DTOs;

namespace task_management_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        if (string.IsNullOrWhiteSpace(userRegisterDto.Username) || string.IsNullOrWhiteSpace(userRegisterDto.Password))
        {
            return BadRequest("Username and Password are required");
        }

        userRegisterDto.Username = userRegisterDto.Username.ToLower();

        var userExists = await dbContext.Users.AnyAsync(x => x.Username.Equals(userRegisterDto.Username));
        if (userExists)
        {
            return BadRequest("Username already exists");
        }

        var user = new User
        {
            Username = userRegisterDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegisterDto.Password),
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(x => x.Username.Equals(userLoginDto.Username));
        if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
        {
            return BadRequest("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }
    
    [HttpGet("check")]
    [Authorize]
    public async Task<IActionResult> Check()
    {
        return Ok();
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(JwtHelper.GetKey());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(JwtHelper.GetExpireDays());

        var token = new JwtSecurityToken(
            JwtHelper.GetIssuer(),
            JwtHelper.GetAudience(),
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}