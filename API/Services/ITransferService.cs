using API.Dtos;
using API.Helpers;

namespace API.Services
{
    public interface ITransferService
    {
        Task BuyAsync(Guid transferId, Guid ownerId);
        Task<TransferResponseDto> CreateAsync(TransferCreateDto transferCreateDto, Guid ownerId);
        Task<PagedList<TransferResponseDto>> GetAsync(QueryStringParameters queryStringParameters);
        Task<TransferResponseDto> GetByIdAsync(Guid id);
        Task<List<PlayerResponseDto>> GetPlayersOnTheMarketAsync(Guid ownerId);
        Task RemovePlayerFromTheMarketAsync(Guid transferId, Guid ownerId);
    }
}