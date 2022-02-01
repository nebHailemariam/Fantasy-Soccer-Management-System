using API.Helpers;
using API.Entities;
using API.Helpers.Filter;

namespace API.Data
{
    public interface IPlayerRepository
    {
        Task<Player> CreateAsync(Player player);
        Task<List<Player>> CreateAsync(DataContext context, List<Player> players);
        Task DeleteByIdAsync(string id);
        Task<PagedList<Player>> GetAsync(QueryStringParameters queryStringParameters);
        Task<List<Player>> GetByTeamIdAsync(Guid teamId);
        Task<Player> GetByIdAsync(Guid id);
        Task<PagedList<Player>> SearchAsync(QueryStringParameters queryStringParameters, FilterParameters<Player> filterParameters);
        Task UpdateNameAndCountryAsync(Guid id, string FirstName, string LastName, string Country);
        Task UpdateValueAndTeamIdAsync(Guid id, double value, Guid teamId, DataContext context);
    }
}