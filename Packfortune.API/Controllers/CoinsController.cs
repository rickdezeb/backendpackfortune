

using Microsoft.AspNetCore.Mvc;
using Packfortune.Logic.Models;
using Packfortune.Logic;

namespace Packfortune.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinsController : ControllerBase
    {
        private readonly UserCoinService _userCoinService;

        public CoinsController(UserCoinService userCoinService)
        {
            _userCoinService = userCoinService;
        }

        [HttpPost]
        public async Task<IActionResult> PostUserCoinData([FromBody] User userCoinData)
        {
            if (userCoinData == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                await _userCoinService.ProcessUserCoinDataAsync(userCoinData);
                return Ok("Data processed and saved successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred while processing the data. {ex.Message}");
            }
        }
    }
}
