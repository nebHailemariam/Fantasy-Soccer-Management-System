using API.Data;
using API.Dtos;
using API.Helpers;
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
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerRepository playerRepository, IPlayerService playerService)
        {
            _playerRepository = playerRepository;
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _playerRepository.GetByIdAsync(id));
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
        public async Task<IActionResult> Get([FromQuery] QueryStringParameters queryStringParamenter)
        {
            var posts = await _playerRepository.GetAsync(queryStringParamenter);
            Response.AddPagination(ref posts);
            return Ok(posts);
        }
    }
}