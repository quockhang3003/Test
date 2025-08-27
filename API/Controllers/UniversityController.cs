using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
    public class UniversityController : ControllerBase
    {
        private readonly UniversityService _service;
        
        public UniversityController(UniversityService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var universities = await _service.GetAllAsync();
                return Ok(universities);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}