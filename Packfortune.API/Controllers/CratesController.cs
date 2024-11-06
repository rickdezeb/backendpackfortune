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
        public async Task<IActionResult> PostUserCoinData([FromBody] Crate crateData)
        {
            await _cratesService.AddCrate(crateData);
            return Ok(crateData);
        }
    }
}
