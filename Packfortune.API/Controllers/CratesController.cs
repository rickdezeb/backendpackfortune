using Microsoft.AspNetCore.Mvc;
using Packfortune.Logic;
using Packfortune.Logic.Models;

namespace Packfortune.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CratesController : ControllerBase
    {
        private readonly CratesService _cratesService;

        public CratesController(CratesService cratesService)
        {
            _cratesService = cratesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCrate([FromForm] string name, [FromForm] int price, [FromForm] IFormFile Picture)
        {
            await _cratesService.AddCrate(name, price, Picture);
            return Ok();
        }
    }
}
