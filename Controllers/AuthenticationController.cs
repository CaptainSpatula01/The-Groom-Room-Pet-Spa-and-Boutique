using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using groomroom.Common;
using groomroom.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace groomroom.Controllers;

[ApiController]
[Route("/api")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthenticationController(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("authenticate")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate([FromBody] LoginDto dto)
    {
        var response = new Response();
        var user = await _userManager.FindByNameAsync(dto.UserName ?? "");

        if (user == null)
        {
            response.AddError(string.Empty, "Username or password is incorrect");
            return BadRequest(response);
        }

        var result = await _signInManager.PasswordSignInAsync(
            dto.UserName,
            dto.Password,
            false,
            false);

        if (!result.Succeeded)
        {
            response.AddError(string.Empty, "Username or password is incorrect");
            return BadRequest(response);
        }

        var token = GenerateJwtToken(user);

        var userGetDto = new UserGetDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
            Pets = user.Pets.Select(p => new Pets
            {
                Id = p.Id,
                Name = p.Name,
                Breed = p.Breed,
                Size = p.Size
            }).ToList()
        };

        response.Data = new
        {
            username = userGetDto.UserName,
            token,
            data = true,
        };

        return Ok(response);
    }

    private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, "YourIssuer"),
                new Claim(JwtRegisteredClaimNames.Aud, "YourAudience"),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsSuperSecretLongKeyPleaseFuckingWorkForTheLoveOfGod"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("get-current-user")]
    [Authorize]
    public async Task<IActionResult> GetLoggedInUser()
    {
        var userId = User.FindFirstValue("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest(new
            {
                data = (object)null,
                errors = new[] { new { property = "", message = "User Id not found in claims." } },
                hasErrors = true
            });
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound(new
            {
                data = (object)null,
                errors = new[] { new { property = "", message = "User not found." } },
                hasErrors = true
            });
        }

        var userGetDto = new UserGetDto
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
            Pets = user.Pets.Select(p => new Pets
            {
                Id = p.Id,
                Name = p.Name,
                Breed = p.Breed,
                Size = p.Size
            }).ToList()
        };

        return Ok(new { data = userGetDto });
    }
}

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
