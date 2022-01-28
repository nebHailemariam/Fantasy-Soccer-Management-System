using API.Dtos;
using API.Helpers;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(
            DataContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task AddRoleAsync(DataContext context, Guid userId, string role)
        {
            var userRole = new IdentityUserRole<Guid>()
            {
                UserId = userId,
                RoleId = (await GetApplicationRoleByName(role)).Id
            };
            await context.UserRoles.AddAsync(userRole);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser> CreateAsync(DataContext context, ApplicationUser user, string password)
        {
            user.UserName = user.Email;
            user.NormalizedUserName = user.UserName.ToUpper();
            user.NormalizedEmail = user.Email.ToUpper();

            // Hash password.
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);

            user.CreatedAt = DateTime.UtcNow;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public async Task<IdentityRole<Guid>> GetApplicationRoleByName(string roleName)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new AppException("Role not found", detail: $"Role with name {roleName} not found", statusCode: HttpStatusCode.NotFound);
            }
            return role;
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new AppException("Account not found", statusCode: HttpStatusCode.NotFound);
            }
            return user;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new AppException("Account not found", statusCode: HttpStatusCode.NotFound);
            }
            return user;
        }

        public async Task<UserDto> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new AppException("Account not found", statusCode: HttpStatusCode.NotFound);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task UpdatePasswordAsync(string userId, string password)
        {
            var user = await GetByIdAsync(userId);

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new AppException("Internal server Error", statusCode: HttpStatusCode.InternalServerError);
            }
        }
    }
}