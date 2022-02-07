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
    [Collection("Integration Test Collection 2")]
    [TestCaseOrderer("Test.Helpers.Orderers.PriorityOrderer", "Test")]
    public class TestCollection_2_TransfersController :
           IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TestCollection_2_TransfersController(
            CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact, TestPriority(2)]
        public async Task List_User_1s_Players_In_The_Market_HTTP_Status_Code_OK()
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

            // Arrange
            string transferEndPoint = "/api/Transfers/current-user/list-player";
            var player = players[0];
            TransferCreateDto transferCreateDto = new()
            {
                PlayerId = player.Id,
                AskingPrice = player.Value
            };

            json = JsonConvert.SerializeObject(transferCreateDto);
            data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync(transferEndPoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(3)]
        public async Task User_2_Buys_User_1s_Player_HTTP_Status_Code_No_content()
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
            var PlayersOnTheMarketEndpoint = "/api/Transfers";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(PlayersOnTheMarketEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var transfers = JsonConvert.DeserializeObject<List<Transfer>>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Arrange
            string buyPlayerEndPoint = $"/api/Transfers/{transfers[0].Id}/buy";

            // Act
            response = await _client.PatchAsync(buyPlayerEndPoint, new StringContent("", Encoding.UTF8, "application/json"));

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}