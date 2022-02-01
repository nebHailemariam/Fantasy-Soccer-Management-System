using API.Data;
using API.Dtos;
using API.Helpers;
using AutoMapper;

namespace API.Services
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;
        private readonly ITeamRepository _teamRepository;

        public TeamService(IMapper mapper, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
        }

        public async Task<PagedList<TeamResponseDto>> GetAsync(QueryStringParameters queryStringParameters)
        {
            return _mapper.Map<PagedList<TeamResponseDto>>(await _teamRepository.GetAsync(queryStringParameters));
        }

        public async Task<TeamResponseDto> GetByIdAsync(Guid id)
        {
            return _mapper.Map<TeamResponseDto>(await _teamRepository.GetByIdAsync(id));
        }

        public async Task<TeamResponseDto> GetByOwnerIdAsync(Guid ownerId)
        {
            return _mapper.Map<TeamResponseDto>(await _teamRepository.GetByOwnerIdAsync(ownerId));
        }

        public async Task UpdateNameAndCountryAsync(TeamDto teamDto, Guid ownerId)
        {
            var teamWithOwnerId = await _teamRepository.GetByOwnerIdAsync(ownerId);
            await _teamRepository.UpdateNameAndCountryAsync(teamWithOwnerId.Id, teamDto.Name, teamDto.Country);
        }
    }
}