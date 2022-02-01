using API.Entities;
using API.Helpers;
using API.Helpers.Filter;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Data
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _context;

        public PlayerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Player> CreateAsync(Player player)
        {
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<List<Player>> CreateAsync(DataContext context, List<Player> players)
        {
            await context.Players.AddRangeAsync(players);
            await context.SaveChangesAsync();
            return players;
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<Player>> GetAsync(QueryStringParameters queryStringParameters)
        {
            return await PagedList<Player>.ToPagedList(_context.Players.Include(p => p.Team).OrderByDescending(p => p.CreatedAt),
                queryStringParameters.PageNumber,
                queryStringParameters.PageSize);
        }

        public async Task<List<Player>> GetByTeamIdAsync(Guid teamId)
        {
            return await _context.Players.Where(p => p.TeamId == teamId).OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Player> GetByIdAsync(Guid id)
        {
            var player = await _context.Players.Include(p => p.Team).SingleOrDefaultAsync(a => a.Id == id);
            if (player == null)
            {
                throw new AppException("Player not found", statusCode: HttpStatusCode.NotFound);
            }
            return player;
        }

        public async Task<PagedList<Player>> SearchAsync(QueryStringParameters queryStringParameters, FilterParameters<Player> filterParameters, string nameSearch)
        {
            return await PagedList<Player>.ToPagedList(filterParameters.Apply(_context.Players.Where(p => string.IsNullOrWhiteSpace(nameSearch) || (p.FirstName + p.LastName).Contains(nameSearch)).Include(p => p.Team).OrderByDescending(p => p.CreatedAt)),
                   queryStringParameters.PageNumber,
                   queryStringParameters.PageSize);
        }

        public async Task UpdateNameAndCountryAsync(Guid id, string FirstName, string LastName, string Country)
        {
            var existingPlayer = await GetByIdAsync(id);
            existingPlayer.FirstName = FirstName;
            existingPlayer.LastName = LastName;
            existingPlayer.Country = Country;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateValueAndTeamIdAsync(Guid id, double value, Guid teamId, DataContext context)
        {
            var existingPlayer = await GetByIdAsync(id);
            existingPlayer.Value = value;
            existingPlayer.TeamId = teamId;

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