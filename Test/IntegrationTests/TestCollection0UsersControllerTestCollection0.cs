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

namespace Test.IntegrationTests
{
    [Collection("Integration Test Collection 0")]
    [TestCaseOrderer("Test.Helpers.Orderers.PriorityOrderer", "Test")]
    public class UsersControllerTestCollection0 :
      IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsersControllerTestCollection0(
            CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact, TestPriority(0)]
        public async Task Register_User_1_Returns_HTTP_Status_Code_OK()
        {
            // Arrange
            var registerEndpoint = "/api/Users/register";
            var userRegistrationDto = new UserRegistrationDto
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };

            var json = JsonConvert.SerializeObject(userRegistrationDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(registerEndpoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, TestPriority(1)]
        public async Task Register_With_A_Taken_Email_Returns_HTTP_Status_Code_Bad_Request()
        {
            // Arrange
            var registerEndpoint = "/api/Users/register";
            var userRegistrationDto = new UserRegistrationDto
            {
                FirstName = "Doe",
                LastName = "Smith",
                Email = "john.smith@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };

            var json = JsonConvert.SerializeObject(userRegistrationDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(registerEndpoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(2)]
        public async Task Register_User_2_Returns_HTTP_Status_Code_OK()
        {
            // Arrange
            var registerEndpoint = "/api/Users/register";
            var userRegistrationDto = new UserRegistrationDto
            {
                FirstName = "Doe",
                LastName = "Smith",
                Email = "doe.smith@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };

            var json = JsonConvert.SerializeObject(userRegistrationDto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(registerEndpoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact, TestPriority(3)]
        public async Task Login_Returns_HTTP_Status_Code_OK()
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
        }

        [Fact, TestPriority(4)]
        public async Task Get_Profile_HTTP_Status_Code_OK()
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
            var profileEndPoint = "/api/Users/current-user/profile";
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["token"];
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            response = await _client.GetAsync(profileEndPoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}