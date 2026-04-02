using dotnet_backend.Modules.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_backend.Modules.Auth.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class CustomAuthController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomRegisterDto dto)
        {
            var existingUser = await userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return BadRequest(new { success = false, message = "User already exists" });
            }

            var newUser = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.Name
            };

            var result = await userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { success = false, errors = result.Errors.Select(e => e.Description) });
            }

            return Ok(new { success = true, message = "User registered successfully" });
        }
    }

    public record CustomRegisterDto(string Name, string Email, string Password);
}
