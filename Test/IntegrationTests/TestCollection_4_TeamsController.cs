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
    public class TestCollection_4_TeamsController :
           IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TestCollection_4_TeamsController(
            CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact, TestPriority(0)]
        public async Task Get_User_1s_Team_HTTP_Status_Code_OK()
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
            var currentUserTeamEndpoint = "/api/Teams/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserTeamEndpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact, TestPriority(1)]
        public async Task Get_User_1s_Team_Money_Assert_Money_Equal_To_6_Milion()
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
            var currentUserTeamEndpoint = "/api/Teams/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserTeamEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var team = JsonConvert.DeserializeObject<Team>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(6000000 == team.Money);
        }

        [Fact, TestPriority(2)]
        public async Task Get_User_1s_Team_Money_Assert_Team_Value_Equal_To_19_Milion()
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
            var currentUserTeamEndpoint = "/api/Teams/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserTeamEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var team = JsonConvert.DeserializeObject<Team>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(19000000 == team.TeamValue);
        }

        [Fact, TestPriority(3)]
        public async Task Get_User_2s_Team_Money_Assert_Money_Equal_To_4_Milion()
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
            var currentUserTeamEndpoint = "/api/Teams/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserTeamEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var team = JsonConvert.DeserializeObject<Team>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(4000000 == team.Money);
        }

        [Fact, TestPriority(4)]
        public async Task Get_User_2s_Team_Money_Assert_Team_Value_Greater_Than_21_Milion()
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
            var currentUserTeamEndpoint = "/api/Teams/current-user";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(currentUserTeamEndpoint);
            content = await response.Content.ReadAsStringAsync();
            var team = JsonConvert.DeserializeObject<Team>(content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(21000000 > team.TeamValue);
        }
    }
}