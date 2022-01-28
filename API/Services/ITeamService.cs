using API.Dtos;

namespace API.Services
{
    public interface ITeamService
    {
        Task UpdateNameAndCountryAsync(TeamDto teamDto, Guid ownerId);
    }
}