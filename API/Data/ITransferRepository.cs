using API.Helpers;
using API.Entities;

namespace API.Data
{
    public interface ITransferRepository
    {
        Task<Transfer> CreateAsync(Transfer transfer);
        Task DeleteByIdAsync(string id);
        Task<PagedList<Transfer>> GetAsync(QueryStringParameters queryStringParameters);
        Task<List<Player>> GetByTeamIdWithListedStatusAsync(Guid teamId);
        Task<Transfer> GetByIdAsync(Guid id);
        Task<bool> GetActiveTransferByPlayerIdAsync(Guid playerId);
        Task UpdateAsync(Transfer transferToUpdate, DataContext context);
    }
}