using API.Entities;
using API.Enums;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Data
{
    public class TransferRepository : ITransferRepository
    {
        private readonly DataContext _context;

        public TransferRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Transfer> CreateAsync(Transfer transfer)
        {
            await _context.Transfers.AddAsync(transfer);
            await _context.SaveChangesAsync();
            return transfer;
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GetActiveTransferByPlayerIdAsync(Guid playerId)
        {
            return await _context.Transfers.Include(m => m.Player)
            .Where(a => a.PlayerId == playerId && a.PlayerTransferStatus == Enums.PlayerTransferStatus.Listed).AnyAsync();
        }

        public async Task<PagedList<Transfer>> GetAsync(QueryStringParameters queryStringParameters)
        {
            return await PagedList<Transfer>.ToPagedList(_context.Transfers.Include(m => m.Player).ThenInclude(p => p.Team).Include(m => m.Seller)
            .Include(m => m.Buyer).OrderByDescending(p => p.CreatedAt),
                queryStringParameters.PageNumber,
                queryStringParameters.PageSize);
        }

        public async Task<Transfer> GetByIdAsync(Guid id)
        {
            var transfer = await _context.Transfers.Include(m => m.Player).ThenInclude(p => p.Team).Include(m => m.Seller)
            .Include(m => m.Buyer).SingleOrDefaultAsync(a => a.Id == id);
            if (transfer == null)
            {
                throw new AppException("Transfer not found", statusCode: HttpStatusCode.NotFound);
            }
            return transfer;
        }

        public Task<List<Player>> GetByTeamIdWithListedStatusAsync(Guid teamId)
        {
            return _context.Transfers.Include(t => t.Player).ThenInclude(p => p.Team).Where(t => t.Player.TeamId == teamId
            && t.PlayerTransferStatus == PlayerTransferStatus.Listed).Include(t => t.Seller)
            .Include(t => t.Buyer).OrderByDescending(t => t.CreatedAt).Select(t => t.Player).ToListAsync();
        }

        public async Task UpdateAsync(Transfer transferToUpdate, DataContext context = null)
        {
            var existingPlayer = await GetByIdAsync(transferToUpdate.Id);
            existingPlayer.SellerId = transferToUpdate.SellerId;
            existingPlayer.BuyerId = transferToUpdate.BuyerId;
            existingPlayer.PlayerId = transferToUpdate.PlayerId;
            existingPlayer.AskingPrice = transferToUpdate.AskingPrice;
            existingPlayer.PlayerTransferStatus = transferToUpdate.PlayerTransferStatus;

            if (context != null)
            {
                await context.SaveChangesAsync();
            }
            else
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}