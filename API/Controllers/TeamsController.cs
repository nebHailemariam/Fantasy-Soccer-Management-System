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
    public class TeamsController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamService _teamService;

        public TeamsController(ITeamRepository teamRepository, ITeamService teamService)
        {
            _teamRepository = teamRepository;
            _teamService = teamService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _teamRepository.GetByIdAsync(id));
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetByCurrentUser()
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            return Ok(await _teamRepository.GetByOwnerIdAsync(currentUserId));
        }

        [HttpPatch("current-user")]
        public async Task<IActionResult> UpdateByCurrentUser([FromBody] TeamDto teamDto)
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            await _teamService.UpdateNameAndCountryAsync(teamDto, currentUserId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryStringParameters queryStringParamenter)
        {
            var posts = await _teamRepository.GetAsync(queryStringParamenter);
            Response.AddPagination(ref posts);
            return Ok(posts);
        }
    }
}