using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private readonly DegreeService _service;
        public DegreeController(DegreeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var degree = await _service.GetAllAsync();
            return degree == null ? NotFound() : Ok(degree);
        }
    }
}
