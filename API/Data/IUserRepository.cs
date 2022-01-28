using API.Dtos;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public interface IUserRepository
    {
        Task AddRoleAsync(DataContext context, Guid userId, string role);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser> CreateAsync(DataContext context, ApplicationUser user, string password);
        Task<bool> IsEmailTakenAsync(string email);
        Task<IdentityRole<Guid>> GetApplicationRoleByName(string roleName);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<UserDto> GetProfileAsync(string userId);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task UpdatePasswordAsync(string userId, string password);
    }
}
