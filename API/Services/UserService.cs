using API.Data;
using API.Dtos;
using API.Helpers;
using API.Entities;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPlayerService _playerService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        public UserService(
            IConfiguration configuration,
            IMapper mapper,
            IPlayerService playerService,
            IServiceProvider serviceProvider,
            ITeamRepository teamRepository,
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _mapper = mapper;
            _playerService = playerService;
            _serviceProvider = serviceProvider;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
            if (!await _userRepository.CheckPasswordAsync(user, userLoginDto.Password))
            {
                throw new AppException("Password doesn't match", statusCode: HttpStatusCode.BadRequest);
            }

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWTSettings");
            var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings.GetSection("validIssuer").Value,
            audience: jwtSettings.GetSection("validAudience").Value,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expiryInMinutes").Value)),
            signingCredentials: signingCredentials);
            return tokenOptions;
        }

        public async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
             new Claim("id", user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email)
            };
            var roles = await _userRepository.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JWTSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task RegisterAsync(UserRegistrationDto userRegistrationDto)
        {
            if (userRegistrationDto.Password.Length < 6)
            {
                throw new AppException("Password length must be greater than 6 characters", statusCode: HttpStatusCode.BadRequest);
            }
            if (await _userRepository.IsEmailTakenAsync(userRegistrationDto.Email))
            {
                throw new AppException("Email is already registered", statusCode: HttpStatusCode.BadRequest);
            }
            var user = _mapper.Map<ApplicationUser>(userRegistrationDto);
            user.UserName = user.Email;
            await CreateAsync(user, userRegistrationDto.Password, RoleConstants.USER_ROLE);
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user, string password, string role)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var strategy = context.Database.CreateExecutionStrategy();
                await strategy.Execute(async () =>
                {
                    using var transaction = context.Database.BeginTransaction();
                    try
                    {
                        var createdUser = await _userRepository.CreateAsync(context, user, password);
                        await _userRepository.AddRoleAsync(context, createdUser.Id, role);
                        var createdUsersTeam = await _teamRepository.CreateAsync(context, new()
                        {
                            Name = createdUser.FirstName + " " + createdUser.LastName + "'s Team",
                            OwnerId = createdUser.Id,
                            TeamValue = 20000000,
                            Money = 5000000,
                            Country = "",
                            CreatedAt = DateTime.UtcNow
                        });
                        await _playerService.CreatePlayersForNewTeamAsync(context, createdUsersTeam.Id);
                        transaction.Commit();
                        return user;
                    }
                    catch (Exception err)
                    {
                        transaction.Rollback();
                        throw new AppException("Internal server error", detail: err.ToString(), statusCode: HttpStatusCode.InternalServerError);
                    }
                });
            }
            return user;
        }

        public async Task UpdatePasswordAsync(string userId, string password)
        {
            await _userRepository.UpdatePasswordAsync(userId, password);
        }
    }
}