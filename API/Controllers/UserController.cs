using Azure.Core;
using Dapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Service;
using System.Security.Claims;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IDbConnectionFactory _dbFactory;
        public UserController(UserService service, IDbConnectionFactory dbFactory)
        {
            _service = service;
            _dbFactory = dbFactory;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _service.GetAllAsync();
            return user == null ? NotFound() : Ok(user);
        }
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _service.GetUserByEmail(email);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterDTO dto)
        {
            await _service.RegisterUser(dto);
            return Ok("Created");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _service.LoginAsync(dto.Email, dto.Password);
            if (user == null) return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            var cookieHeader = HttpContext.Response.Headers["Set-Cookie"];

            return Ok(new
            {
                Message = "Login successful",
                User = user.Email,
                Cookies = cookieHeader.ToString()
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            Console.WriteLine("API received:");
            Console.WriteLine(JsonSerializer.Serialize(dto));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var conn = _dbFactory.CreateConnection();
            if (dto.ID > 0)
            {
                var existingUser = await conn.QueryFirstOrDefaultAsync<User>(
                    "SELECT * FROM Users WHERE Id = @Id", new { dto.ID });

                if (existingUser == null)
                    return NotFound("User not found");
                if (!string.Equals(existingUser.Email?.Trim(), dto.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    var emailExists = await conn.ExecuteScalarAsync<bool>(
                        "SELECT COUNT(1) FROM Users WHERE Email = @Email AND Id != @Id",
                        new { dto.Email, dto.ID });

                    if (emailExists)
                        return BadRequest("Email already exists.");
                }
                if (!string.Equals(existingUser.PasswordHash?.Trim(), dto.IdCardNumber?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    var idCardExists = await conn.ExecuteScalarAsync<bool>(
                        "SELECT COUNT(1) FROM Users WHERE IdCardNumber = @IdCardNumber AND Id != @Id",
                        new { dto.IdCardNumber, dto.ID });

                    if (idCardExists)
                        return BadRequest("ID Card already exists.");
                }

                string updateSql = @"UPDATE Users SET
                LastName = @LastName,
                FirstName = @FirstName,
                VietnameseName = @VietnameseName,
                Gender = @Gender,
                Nationality = @Nationality,
                DateOfBirth = @DateOfBirth,
                PlaceOfBirth = @PlaceOfBirth,
                Email = @Email,
                PasswordHash = @IdCardNumber,
                DateOfIssue = @DateOfIssue,
                PlaceOfIssue = @PlaceOfIssue,
                Mobile = @Mobile,
                Street = @Street,
                Ward = @Ward,
                CityID = @City,
                CurrentAddress = @CurrentAddress,
                PreferableOfficeLocationID = @PreferableOfficeLocation,
                FirstPreferenceID = @FirstPreference,
                SecondPreferenceID = @SecondPreference,
                UpdatedAt = GETDATE()
                WHERE Id = @ID";

                await conn.ExecuteAsync(updateSql, dto);
                return Ok(new { message = "Update successful." });
            }

            var emailExistsNew = await conn.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Users WHERE Email = @Email", new { dto.Email });

            if (emailExistsNew)
                return BadRequest("Email already exists.");

            var idCardExistsNew = await conn.ExecuteScalarAsync<bool>(
                "SELECT COUNT(1) FROM Users WHERE PasswordHash = @IdCardNumber", new { dto.IdCardNumber });

            if (idCardExistsNew)
                return BadRequest("IDCard already exists.");

            string insertSql = @"INSERT INTO Users (
                                                    LastName, FirstName, VietnameseName, Gender, Nationality, DateOfBirth,
                                                    PlaceOfBirth, Email, PasswordHash, DateOfIssue, PlaceOfIssue, Mobile,
                                                    Street, Ward, CityID, CurrentAddress, PreferableOfficeLocationID,
                                                    FirstPreferenceID, SecondPreferenceID, CreatedAt)
                                VALUES (
                                        @LastName, @FirstName, @VietnameseName, @Gender, @Nationality, @DateOfBirth,
                                        @PlaceOfBirth, @Email, @IdCardNumber, @DateOfIssue, @PlaceOfIssue, @Mobile,
                                        @Street, @Ward, @City, @CurrentAddress, @PreferableOfficeLocation,
                                        @FirstPreference, @SecondPreference, GETDATE())";
            string hashedIdCard = BCrypt.Net.BCrypt.HashPassword(dto.IdCardNumber);
            dto.IdCardNumber = hashedIdCard;
            await conn.ExecuteAsync(insertSql, dto);
            return Ok(new { message = "Register successful." });
        }
        [HttpPost("logout")]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "User logout successful" });
        }
    }
}
