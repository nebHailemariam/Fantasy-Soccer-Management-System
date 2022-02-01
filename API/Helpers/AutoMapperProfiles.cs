using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegistrationDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDto>();
            CreateMap(typeof(PagedList<>), typeof(PagedList<>)).ConvertUsing(typeof(PagedListTypeConverter<,>));
            CreateMap<TeamDto, Team>();
            CreateMap<TransferCreateDto, Transfer>();
            CreateMap<Player, PlayerResponseDto>();
            CreateMap<Team, TeamResponseDto>();
            CreateMap<Transfer, TransferResponseDto>();
        }
    }
}