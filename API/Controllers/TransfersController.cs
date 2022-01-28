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
    public class TransfersController : ControllerBase
    {
        private readonly ITransferRepository _transferRepository;
        private readonly ITransferService _transferService;

        public TransfersController(ITransferRepository transferRepository, ITransferService transferService)
        {
            _transferRepository = transferRepository;
            _transferService = transferService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            return Ok(await _transferRepository.GetByIdAsync(id));
        }

        [HttpGet("players/current-user")]
        public async Task<IActionResult> GetByCurrentUser()
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            return Ok(await _transferService.GetPlayersOnTheMarketAsync(currentUserId));
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryStringParameters queryStringParamenter)
        {
            var transfers = await _transferRepository.GetAsync(queryStringParamenter);
            Response.AddPagination(ref transfers);
            return Ok(transfers);
        }

        [HttpPost("current-user/list-player")]
        public async Task<IActionResult> Create(TransferCreateDto transferCreateDto)
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            return Ok(await _transferService.CreateAsync(transferCreateDto, currentUserId));
        }

        [HttpPatch("{id}/buy")]
        public async Task<IActionResult> Buy([FromRoute] Guid id)
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            await _transferService.BuyAsync(id, currentUserId);
            return NoContent(); ;
        }

        [HttpPatch("{id}/current-user/remove-listed-player")]
        public async Task<IActionResult> RemovePlayerFromTheMarket([FromRoute] Guid id)
        {
            var currentUserId = new Guid(User.FindFirst("id").Value);
            await _transferService.RemovePlayerFromTheMarketAsync(id, currentUserId);
            return NoContent();
        }
    }
}