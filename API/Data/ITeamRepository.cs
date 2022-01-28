using API.Helpers;
using API.Entities;

namespace API.Data
{
    public interface ITeamRepository
    {
        Task<Team> CreateAsync(DataContext context, Team team);
        Task DeleteByIdAsync(string id);
        Task<PagedList<Team>> GetAsync(QueryStringParameters queryStringParameters);
        Task<Team> GetByIdAsync(Guid id);
        Task<Team> GetByOwnerIdAsync(Guid ownerId);
        Task UpdateNameAndCountryAsync(Guid id, string Name, string Country);
        Task UpdateValueAndMoneyAsync(Guid id, double teamValue, double money, DataContext context = null);
    }
}