using System.Net;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Enums;
using API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class TransferService : ITransferService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITeamRepository _teamRepository;
        private readonly ITransferRepository _transferRepository;

        public TransferService(IPlayerRepository playerRepository,
                               IServiceProvider serviceProvider,
                               ITeamRepository teamRepository,
                               ITransferRepository transferRepository)
        {
            _playerRepository = playerRepository;
            _serviceProvider = serviceProvider;
            _teamRepository = teamRepository;
            _transferRepository = transferRepository;
        }

        public async Task BuyAsync(Guid transferId, Guid currentUserId)
        {
            var transfer = await _transferRepository.GetByIdAsync(transferId);
            var buyerTeam = await _teamRepository.GetByOwnerIdAsync(currentUserId);
            if (buyerTeam.Money < transfer.AskingPrice)
            {
                throw new AppException("You don't have sufficient fund to buy the player", statusCode: HttpStatusCode.BadRequest);
            }

            var sellerTeam = await _teamRepository.GetByOwnerIdAsync(transfer.SellerId);
            if (sellerTeam.OwnerId == currentUserId)
            {
                throw new AppException("You can not buy your own players", statusCode: HttpStatusCode.BadRequest);
            }

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.Execute(async () =>
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    if (transfer.PlayerTransferStatus != PlayerTransferStatus.Listed)
                    {
                        throw new AppException("You can not buy a player that is off the market list", statusCode: HttpStatusCode.BadRequest);
                    }

                    // Set buyer id
                    transfer.BuyerId = currentUserId;
                    transfer.PlayerTransferStatus = PlayerTransferStatus.Sold;
                    await _transferRepository.UpdateAsync(transfer, context);

                    // Update seller team
                    var player = await _playerRepository.GetByIdAsync(transfer.PlayerId);
                    var sellerTeamValue = sellerTeam.TeamValue - player.Value;
                    var sellerTeamMoney = sellerTeam.Money + transfer.AskingPrice;
                    await _teamRepository.UpdateValueAndMoneyAsync(sellerTeam.Id, sellerTeamValue, sellerTeamMoney);

                    // Randomly increase the players value between 10 and 100
                    var newPlayerValue = player.Value + (player.Value * new Random().Next(10, 101) / 100);
                    await _playerRepository.UpdateValueAndTeamIdAsync(player.Id, newPlayerValue, buyerTeam.Id, context);

                    // Update buyer team
                    var buyerTeamValue = buyerTeam.TeamValue + newPlayerValue;
                    var buyerMoney = buyerTeam.Money - transfer.AskingPrice;
                    await _teamRepository.UpdateValueAndMoneyAsync(buyerTeam.Id, buyerTeamValue, buyerMoney);

                    transaction.Commit();
                    return transfer;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });
        }

        public async Task<Transfer> CreateAsync(TransferCreateDto transferCreateDto, Guid ownerId)
        {
            var player = await _playerRepository.GetByIdAsync(transferCreateDto.PlayerId);
            var ownersTeam = await _teamRepository.GetByOwnerIdAsync(ownerId);
            if (player.TeamId != ownersTeam.Id)
            {
                throw new AppException("You don't have the previlage to transfer this player", statusCode: HttpStatusCode.Forbidden);
            }
            var isPlayerOnMarketList = await _transferRepository.GetActiveTransferByPlayerIdAsync(transferCreateDto.PlayerId);
            if (isPlayerOnMarketList)
            {
                throw new AppException("Player is already on the market", statusCode: HttpStatusCode.BadRequest);
            }

            Transfer transferToCreate = new()
            {
                SellerId = ownerId,
                BuyerId = null,
                PlayerId = transferCreateDto.PlayerId,
                AskingPrice = transferCreateDto.AskingPrice,
                PlayerTransferStatus = PlayerTransferStatus.Listed,
                CreatedAt = DateTime.UtcNow
            };
            return await _transferRepository.CreateAsync(transferToCreate);
        }

        public async Task<List<Player>> GetPlayersOnTheMarketAsync(Guid ownerId)
        {
            var ownersTeam = await _teamRepository.GetByOwnerIdAsync(ownerId);
            return await _transferRepository.GetByTeamIdWithListedStatusAsync(ownersTeam.Id);
        }

        public async Task RemovePlayerFromTheMarketAsync(Guid transferId, Guid ownerId)
        {
            var ownersTeam = await _teamRepository.GetByOwnerIdAsync(ownerId);
            var existingTransfer = await _transferRepository.GetByIdAsync(transferId);
            var player = await _playerRepository.GetByIdAsync(existingTransfer.PlayerId);
            if (player.TeamId != ownersTeam.Id)
            {
                throw new AppException("You don't have the previlage to update players transfer status", statusCode: HttpStatusCode.Forbidden);
            }

            if (existingTransfer.PlayerTransferStatus == PlayerTransferStatus.Sold)
            {
                throw new AppException("Player has already been sold", statusCode: HttpStatusCode.BadRequest);
            }

            if (existingTransfer.PlayerTransferStatus == PlayerTransferStatus.Cancelled)
            {
                throw new AppException("Player has already been removed from the market place", statusCode: HttpStatusCode.BadRequest);
            }

            existingTransfer.PlayerTransferStatus = PlayerTransferStatus.Cancelled;
            await _transferRepository.UpdateAsync(existingTransfer, null);
        }
    }
}