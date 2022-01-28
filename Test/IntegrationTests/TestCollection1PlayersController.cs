using Xunit;
using Test.Helpers.Attributes;
using API;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using API.Dtos;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using API.Entities;
using API.Enums;

namespace Test.IntegrationTests
{
    [Collection("Integration Test Collection 1")]
    [TestCaseOrderer("Test.Helpers.Orderers.PriorityOrderer", "Test")]
    public class PlayersControllerTestsCollection1 :
           IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PlayersControllerTestsCollection1(
            CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact, TestPriority(0)]
        public async Task Get_User_1s_Players_HTTP_Status_Code_OK()
        {
            // Arrange
            string loginEndPoint = "/api/Users/login";

            UserLoginDto userLoginDto = new()
            {
                Email = "john.smith@example.com",
                Password = "password"
            };

            var json = JsonConvert.SerializeObject(userLoginDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(loginEndPoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Arrange
            var currentUserPlayersEndpoint = "/api/Players/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserPlayersEndpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(1)]
        public async Task Get_User_1s_Players_Count_HTTP_Status_Code_OK()
        {
            // Arrange
            string loginEndPoint = "/api/Users/login";

            UserLoginDto userLoginDto = new()
            {
                Email = "john.smith@example.com",
                Password = "password"
            };

            var json = JsonConvert.SerializeObject(userLoginDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(loginEndPoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Arrange
            var currentUserPlayersEndpoint = "/api/Players/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserPlayersEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<List<Player>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(20, players.Count);
        }

        [Fact, TestPriority(2)]
        public async Task Get_User_1s_Players_Count_By_Position_Type_Assert_Count()
        {
            // Arrange
            string loginEndPoint = "/api/Users/login";

            UserLoginDto userLoginDto = new()
            {
                Email = "john.smith@example.com",
                Password = "password"
            };

            var json = JsonConvert.SerializeObject(userLoginDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(loginEndPoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Arrange
            var currentUserPlayersEndpoint = "/api/Players/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserPlayersEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<List<Player>>(content);

            int goalKeepersCount = 0, defendersCount = 0, midfieldersCount = 0, attackersCount = 0;

            foreach (var player in players)
            {
                switch (player.Position)
                {
                    case PlayerPositions.GoalKeeper:
                        goalKeepersCount++;
                        break;
                    case PlayerPositions.Defender:
                        defendersCount++;
                        break;
                    case PlayerPositions.Midfielder:
                        midfieldersCount++;
                        break;
                    case PlayerPositions.Attacker:
                        attackersCount++;
                        break;
                    default:
                        break;
                }
            }

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, goalKeepersCount);
            Assert.Equal(6, defendersCount);
            Assert.Equal(6, midfieldersCount);
            Assert.Equal(5, attackersCount);
        }
    }
}