

using Microsoft.AspNetCore.Mvc;
using Packfortune.Logic.Models;
using Packfortune.Logic;
using Microsoft.AspNetCore.RateLimiting;
using System.ComponentModel.DataAnnotations;
using Packfortune.Logic.Exceptions;

namespace Packfortune.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [EnableRateLimiting("Test")]
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
            await _userCoinService.ProcessUserCoinDataAsync(userCoinData);
            return Ok(userCoinData);
        }

        [HttpGet("{steamId}")]
        public async Task<IActionResult> GetUserBySteamId(string steamId)
        {
            var user = await _userCoinService.GetUserInfo(steamId);
            if (user == null)
            {
                throw new UserNotFoundException("User not found.");
            }
            return Ok(user);
        }
    }
}
