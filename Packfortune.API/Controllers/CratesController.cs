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
        public async Task<IActionResult> CreateCrate(CreateCrateRequest request)
        {
            await _cratesService.AddCrate(request.Name, request.Price, request.Picture);
            return Ok();
        }

        [HttpGet] 
        public async Task <IActionResult> GetAllCrates()
        {
            var crates = await _cratesService.GetAllCrates();
            return Ok(crates);
        }
    }
}
