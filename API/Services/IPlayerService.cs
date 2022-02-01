using API.Data;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Helpers.Filter;

namespace API.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> CreatePlayersForNewTeamAsync(DataContext context, Guid teamId);
        Task<PagedList<PlayerResponseDto>> GetAsync(QueryStringParameters queryStringParameters);
        Task<PlayerResponseDto> GetByIdAsync(Guid id);
        Task<List<PlayerResponseDto>> GetPlayersByOwnerIdAsync(Guid ownerId);
        DateTime RandomAgeForPlayer();
        Task<PagedList<PlayerResponseDto>> SearchAsync(QueryStringParameters queryStringParameters, FilterParameters<Player> filterParameters, string nameSearch);
        Task UpdateNameAndCountryAsync(Guid playerId, PlayerDto playerDto, Guid ownerId);
    }
}