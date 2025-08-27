using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System.Data.Entity.Infrastructure;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly EducationService _service;
        private readonly IDbConnectionFactory _dbFactory;

        public EducationController(EducationService service, IDbConnectionFactory dbFactory)
        {
            _service = service;
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var education = await _service.GetAllAsync();
            return education == null ? NotFound() : Ok(education);
        }
        [HttpPost]
        public async Task<IActionResult> AddEducation(EducationDTO _dto)
        {
            await _service.AddEducation(_dto);
            return Ok("Created");
        }

    }
}
