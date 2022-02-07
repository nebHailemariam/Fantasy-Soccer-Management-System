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

namespace Test.IntegrationTests
{

    [Collection("Integration Test Collection 3")]
    [TestCaseOrderer("Test.Helpers.Orderers.PriorityOrderer", "Test")]
    public class TestCollection_3_PlayersController :
   IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TestCollection_3_PlayersController(
            CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact, TestPriority(0)]
        public async Task Get_User_1s_Players_Count_After_Transfer_HTTP_Status_Code_OK()
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
            Assert.Equal(19, players.Count);
        }

        [Fact, TestPriority(1)]
        public async Task Get_User_2s_Players_Count_After_Transfer_HTTP_Status_Code_OK()
        {
            // Arrange
            string loginEndPoint = "/api/Users/login";

            UserLoginDto userLoginDto = new()
            {
                Email = "doe.smith@example.com",
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
            Assert.Equal(21, players.Count);
        }

        [Fact, TestPriority(2)]
        public async Task Get_User_2s_Bought_Player_Value_After_Transfer_Assert_Value_Greater_Than_1000000()
        {
            // Arrange
            string loginEndPoint = "/api/Users/login";

            UserLoginDto userLoginDto = new()
            {
                Email = "doe.smith@example.com",
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
            List<Player>? players = JsonConvert.DeserializeObject<List<Player>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            double? recentlyAddedPlayerValue = players.Find(p => p.Value > 1000000)?.Value;
            Assert.True(1000000 < recentlyAddedPlayerValue);
        }
    }
}