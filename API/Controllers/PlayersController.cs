using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Helpers.Filter;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _playerService.GetByIdAsync(id));
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] QueryStringParameters queryStringParameters, [FromQuery] FilterParameters<Player> filterParameters, [FromQuery] string nameSearch)
        {
            var players = await _playerService.SearchAsync(queryStringParameters, filterParameters, nameSearch);
            Response.AddPagination(ref players);
            return Ok(players);
        }

        [HttpPatch("{id}/current-user")]
        public async Task<IActionResult> UpdateByCurrentUser([FromRoute] Guid id, [FromBody] PlayerDto playerDto)
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            await _playerService.UpdateNameAndCountryAsync(id, playerDto, currentUserId);
            return NoContent();
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetByCurrentUser()
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            return Ok(await _playerService.GetPlayersByOwnerIdAsync(currentUserId));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] QueryStringParameters queryStringParamenter)
        {
            var players = await _playerService.GetAsync(queryStringParamenter);
            Response.AddPagination(ref players);
            return Ok(players);
        }
    }
}