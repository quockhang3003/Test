using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationOfficeController : ControllerBase
    {
        private readonly LocationOfficeService _service;
        public LocationOfficeController(LocationOfficeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locationOffice = await _service.GetAll();
            return locationOffice == null ? NotFound() : Ok(locationOffice);
        }
    }
}
