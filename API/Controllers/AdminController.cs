using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var admin = await _adminService.LoginAsync(dto.Email, dto.Password);
            if (admin == null)
                return Unauthorized("Invalid username or password.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Email),
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("AdminId", admin.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, "AdminAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AdminAuth", principal);

            return Ok(new
            {
                Message = "Admin login successful",
                AdminId = admin.Id,
                Username = admin.Username,
                FullName = admin.FullName
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return Ok(new { Message = "Admin logout successful" });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var username = User.Identity.Name;
            var admin = await _adminService.GetByUsernameAsync(username);

            if (admin == null)
                return NotFound();

            return Ok(new
            {
                admin.Id,
                admin.Username,
                admin.Email,
                admin.FullName
            });
        }
    }
}