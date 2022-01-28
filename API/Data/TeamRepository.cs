using API.Entities;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DataContext _context;

        public TeamRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Team> CreateAsync(DataContext context, Team team)
        {
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();
            return team;
        }

        public Task DeleteByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<Team>> GetAsync(QueryStringParameters queryStringParameters)
        {
            return await PagedList<Team>.ToPagedList(_context.Teams.OrderByDescending(t => t.CreatedAt).Include(t => t.Owner),
                queryStringParameters.PageNumber,
                queryStringParameters.PageSize);
        }

        public async Task<Team> GetByIdAsync(Guid id)
        {
            var team = await _context.Teams.Include(p => p.Owner).SingleOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                throw new AppException("Team not found", statusCode: HttpStatusCode.NotFound);
            }
            return team;
        }

        public async Task<Team> GetByOwnerIdAsync(Guid ownerId)
        {
            var team = await _context.Teams.SingleOrDefaultAsync(t => t.OwnerId == ownerId);
            if (team == null)
            {
                throw new AppException("Team not found", statusCode: HttpStatusCode.NotFound);
            }
            return team;
        }

        public async Task UpdateNameAndCountryAsync(Guid id, string Name, string Country)
        {
            Team existingTeam = await GetByIdAsync(id);
            existingTeam.Name = Name;
            existingTeam.Country = Country;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateValueAndMoneyAsync(Guid id, double teamValue, double money, DataContext context = null)
        {
            Team existingTeam = await GetByIdAsync(id);
            existingTeam.TeamValue = teamValue;
            existingTeam.Money = money;

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