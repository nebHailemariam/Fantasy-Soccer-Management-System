using API.Dtos;
using API.Helpers;

namespace API.Services
{
    public interface ITeamService
    {
        Task<PagedList<TeamResponseDto>> GetAsync(QueryStringParameters queryStringParameters);
        Task<TeamResponseDto> GetByIdAsync(Guid id);
        Task<TeamResponseDto> GetByOwnerIdAsync(Guid ownerId);
        Task UpdateNameAndCountryAsync(TeamDto teamDto, Guid ownerId);
    }
}