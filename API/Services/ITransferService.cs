using API.Data;
using API.Dtos;
using API.Entities;

namespace API.Services
{
    public interface ITransferService
    {
        Task BuyAsync(Guid transferId, Guid ownerId);
        Task<Transfer> CreateAsync(TransferCreateDto transferCreateDto, Guid ownerId);
        Task<List<Player>> GetPlayersOnTheMarketAsync(Guid ownerId);
        Task RemovePlayerFromTheMarketAsync(Guid transferId, Guid ownerId);
    }
}