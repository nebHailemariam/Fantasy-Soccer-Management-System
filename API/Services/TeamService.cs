using API.Data;
using API.Dtos;
using API.Entities;
using API.Enums;
using AutoMapper;

namespace API.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {

            _teamRepository = teamRepository;
        }
        public async Task UpdateNameAndCountryAsync(TeamDto teamDto, Guid ownerId)
        {
            var teamWithOwnerId = await _teamRepository.GetByOwnerIdAsync(ownerId);
            await _teamRepository.UpdateNameAndCountryAsync(teamWithOwnerId.Id, teamDto.Name, teamDto.Country);
        }
    }
}