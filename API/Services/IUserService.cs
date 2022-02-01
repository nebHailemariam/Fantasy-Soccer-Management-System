using API.Dtos;
using API.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services
{
    public interface IUserService
    {
        Task<string> LoginAsync(UserLoginDto userLoginDto);
        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims);
        Task<List<Claim>> GetClaimsAsync(ApplicationUser user);
        Task<UserDto> GetProfileAsync(string userId);
        SigningCredentials GetSigningCredentials();
        Task RegisterAsync(UserRegistrationDto userRegistrationDto);
        Task UpdatePasswordAsync(string userId, string password);
    }
}