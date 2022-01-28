using API.Helpers;
using API.Entities;

namespace API.Data
{
    public interface IPlayerRepository
    {
        Task<Player> CreateAsync(Player player);
        Task<List<Player>> CreateAsync(DataContext context, List<Player> players);
        Task DeleteByIdAsync(string id);
        Task<PagedList<Player>> GetAsync(QueryStringParameters queryStringParameters);
        Task<List<Player>> GetAsync(Guid teamId);
        Task<Player> GetByIdAsync(Guid id);
        Task UpdateNameAndCountryAsync(Guid id, string FirstName, string LastName, string Country);
        Task UpdateValueAndTeamIdAsync(Guid id, double value, Guid teamId, DataContext context);
    }
}