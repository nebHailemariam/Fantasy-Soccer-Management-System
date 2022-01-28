using API.Data;
using API.Dtos;
using API.Entities;

namespace API.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> CreatePlayersForNewTeamAsync(DataContext context, Guid teamId);
        Task<List<Player>> GetPlayersByOwnerIdAsync(Guid ownerId);
        DateTime RandomAgeForPlayer();
        Task UpdateNameAndCountryAsync(Guid playerId, PlayerDto playerDto, Guid ownerId);
    }
}