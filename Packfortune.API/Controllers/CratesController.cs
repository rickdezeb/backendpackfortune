using Microsoft.AspNetCore.Mvc;
using Packfortune.Logic;
using Packfortune.Logic.DTO_s;
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

        [HttpPut]
        public async Task<IActionResult> UpdateCrate(UpdateCrateRequest request)
        {
            await _cratesService.UpdateCrate(request.Id, request.Name, request.Price, request.Picture);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCrate(int id)
        {
            await _cratesService.RemoveCrate(id);
            return Ok();
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyCrate(BuyCrateRequest request)
        {
            await _cratesService.BuyCrate(request.SteamId, request.CrateId);
            return Ok();
        }
    }
}
