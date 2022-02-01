using System.Net;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Enums;
using API.Helpers;
using API.Helpers.Filter;
using AutoMapper;

namespace API.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly Random gen = new();
        private readonly IMapper _mapper;
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayerService(IMapper mapper, IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        public async Task<List<Player>> CreatePlayersForNewTeamAsync(DataContext context, Guid teamId)
        {
            List<Player> players = new();
            int i = 0;

            // Add 3 Goal Keepers
            for (; i < 3; i++)
            {
                players.Add(new()
                {
                    FirstName = "Player",
                    LastName = (i + 1).ToString(),
                    Position = PlayerPositions.GoalKeeper,
                    DateOfBirth = RandomAgeForPlayer(),
                    Value = 1000000,
                    Country = "",
                    TeamId = teamId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Add 6 Defenders
            for (; i < 9; i++)
            {
                players.Add(new()
                {
                    FirstName = "Player",
                    LastName = (i + 1).ToString(),
                    Position = PlayerPositions.Defender,
                    DateOfBirth = RandomAgeForPlayer(),
                    Value = 1000000,
                    Country = "",
                    TeamId = teamId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Add 6 Midfielders
            for (; i < 15; i++)
            {
                players.Add(new()
                {
                    FirstName = "Player",
                    LastName = (i + 1).ToString(),
                    Position = PlayerPositions.Midfielder,
                    DateOfBirth = RandomAgeForPlayer(),
                    Value = 1000000,
                    Country = "",
                    TeamId = teamId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Add 5 Attackers
            for (; i < 20; i++)
            {
                players.Add(new()
                {
                    FirstName = "Player",
                    LastName = (i + 1).ToString(),
                    Position = PlayerPositions.Attacker,
                    DateOfBirth = RandomAgeForPlayer(),
                    Value = 1000000,
                    Country = "",
                    TeamId = teamId,
                    CreatedAt = DateTime.UtcNow
                });
            }
            return await _playerRepository.CreateAsync(context, players);
        }

        public async Task<PagedList<PlayerResponseDto>> GetAsync(QueryStringParameters queryStringParameters)
        {
            return _mapper.Map<PagedList<PlayerResponseDto>>(await _playerRepository.GetAsync(queryStringParameters));
        }

        public Task<PlayerResponseDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlayerResponseDto>> GetPlayersByOwnerIdAsync(Guid ownerId)
        {
            var ownersTeam = await _teamRepository.GetByOwnerIdAsync(ownerId);
            return _mapper.Map<List<PlayerResponseDto>>(await _playerRepository.GetByTeamIdAsync(ownersTeam.Id));
        }

        public DateTime RandomAgeForPlayer()
        {
            // Forty years from today
            DateTime start = DateTime.UtcNow.AddYears(-40);
            // Twenty years from today
            DateTime end = DateTime.UtcNow.AddYears(-20);
            // Returns random age between 40 and 20
            int range = (end - start).Days;
            return start.AddDays(gen.Next(range));
        }

        public async Task<PagedList<PlayerResponseDto>> SearchAsync(QueryStringParameters queryStringParameters, FilterParameters<Player> filterParameters)
        {
            return _mapper.Map<PagedList<PlayerResponseDto>>(await _playerRepository.SearchAsync(queryStringParameters, filterParameters));
        }

        public async Task UpdateNameAndCountryAsync(Guid playerId, PlayerDto playerDto, Guid ownerId)
        {
            var player = await _playerRepository.GetByIdAsync(playerId);
            var ownersTeam = await _teamRepository.GetByOwnerIdAsync(ownerId);
            if (player.TeamId != ownersTeam.Id)
            {
                throw new AppException("You don't have the previlage to update this palyer", statusCode: HttpStatusCode.Forbidden);
            }
            await _playerRepository.UpdateNameAndCountryAsync(playerId, playerDto.FirstName, playerDto.LastName, playerDto.Country);
        }
    }
}