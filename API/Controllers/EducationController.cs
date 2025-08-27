using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly EducationService _service;

        public EducationController(EducationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Nếu user đăng nhập, chỉ lấy education của user đó
                if (User.Identity.IsAuthenticated)
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userIdClaim, out int userId))
                    {
                        var userEducations = await _service.GetByUserIdAsync(userId);
                        return Ok(userEducations);
                    }
                }

                // Admin có thể xem tất cả
                if (User.IsInRole("Admin"))
                {
                    var allEducations = await _service.GetAllAsync();
                    return Ok(allEducations);
                }

                return Unauthorized("Please login to view education records");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                // Chỉ cho phép user xem education của mình hoặc admin xem tất cả
                if (User.Identity.IsAuthenticated)
                {
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(userIdClaim, out int currentUserId))
                    {
                        if (currentUserId == userId || User.IsInRole("Admin"))
                        {
                            var educations = await _service.GetByUserIdAsync(userId);
                            return Ok(educations);
                        }
                    }
                }

                return Unauthorized("Access denied");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var education = await _service.GetByIdAsync(id);
                if (education == null)
                    return NotFound("Education record not found");

                return Ok(education);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEducation(EducationDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Lấy UserID từ claims
                if (!User.Identity.IsAuthenticated)
                    return Unauthorized("Please login to add education");

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                    return BadRequest("Invalid user session");

                await _service.AddEducation(dto, userId);
                return Ok(new { message = "Education added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEducation(int id, EducationDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Lấy UserID từ claims
                if (!User.Identity.IsAuthenticated)
                    return Unauthorized("Please login to update education");

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int userId))
                    return BadRequest("Invalid user session");

                dto.UserID = userId;
                await _service.UpdateEducation(id, dto);
                return Ok(new { message = "Education updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEducation(int id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Unauthorized("Please login to delete education");

                await _service.DeleteEducation(id);
                return Ok(new { message = "Education deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}