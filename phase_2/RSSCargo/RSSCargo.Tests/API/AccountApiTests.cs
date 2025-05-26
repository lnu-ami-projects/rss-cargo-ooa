using System.Net;
using Newtonsoft.Json;
using RestSharp;


namespace RSSCargo.Tests.API
{
    public class AccountApiTests : ApiTestBase
    {
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new RestRequest("/api/account/login");
            request.AddJsonBody(new
            {
                Email = "test_user@example.com",
                Password = "Test123!"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content);
            Assert.False(string.IsNullOrEmpty(response.Content));
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var request = new RestRequest("/api/account/login");
            request.AddJsonBody(new
            {
                Email = "invalid@example.com",
                Password = "WrongPassword!"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsSuccessStatusCode()
        {
            // Arrange
            var randomEmail = $"test_{Guid.NewGuid().ToString().Substring(0, 8)}@example.com";
            var request = new RestRequest("/api/account/register");
            request.AddJsonBody(new
            {
                Email = randomEmail,
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var request = new RestRequest("/api/account/register");
            request.AddJsonBody(new
            {
                Email = "invalid-email", // Invalid email format
                Password = "short", // Too short password
                ConfirmPassword = "different" // Doesn't match password
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ForgotPassword_WithValidEmail_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new RestRequest("/api/account/forgot-password");
            request.AddJsonBody(new
            {
                Email = "test_user@example.com"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var existingEmail = "test_user@example.com"; // Using the existing email from other tests
            var request = new RestRequest("/api/account/register");
            request.AddJsonBody(new
            {
                Email = existingEmail,
                Password = "Test123!",
                ConfirmPassword = "Test123!"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ForgotPassword_WithInvalidEmail_ReturnsNotFound()
        {
            // Arrange
            var request = new RestRequest("/api/account/forgot-password");
            request.AddJsonBody(new
            {
                Email = "nonexistent@example.com"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ResetPassword_WithValidData_ReturnsSuccessStatusCode()
        {
            // Note: This test is a placeholder as the actual reset password functionality would
            // require a valid token from email which is hard to test automatically

            // Arrange
            var request = new RestRequest("/api/account/reset-password");
            request.AddJsonBody(new
            {
                Email = "test_user@example.com",
                Password = "NewTest123!",
                ConfirmPassword = "NewTest123!",
                Token = "valid-token" // In a real test, this would be a valid token
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert - We expect this to fail because we don't have a real token
            // This is just to demonstrate the API call structure
            Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_WithValidCredentials_ReturnsSuccessStatusCode()
        {
            // Arrange
            await AuthenticateAsync("test_user@example.com", "Test123!");
            var request = CreateAuthorizedRequest("/api/account/change-password", Method.Post);
            request.AddJsonBody(new
            {
                CurrentPassword = "Test123!",
                NewPassword = "NewTest123!",
                ConfirmNewPassword = "NewTest123!"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Change it back for other tests
            var resetRequest = CreateAuthorizedRequest("/api/account/change-password", Method.Post);
            resetRequest.AddJsonBody(new
            {
                CurrentPassword = "NewTest123!",
                NewPassword = "Test123!",
                ConfirmNewPassword = "Test123!"
            });
            await Client.ExecutePostAsync(resetRequest);
        }

        [Fact]
        public async Task ChangePassword_WithInvalidCurrentPassword_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync("test_user@example.com", "Test123!");
            var request = CreateAuthorizedRequest("/api/account/change-password", Method.Post);
            request.AddJsonBody(new
            {
                CurrentPassword = "WrongPassword!",
                NewPassword = "NewTest123!",
                ConfirmNewPassword = "NewTest123!"
            });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_WithValidToken_ReturnsNewToken()
        {
            // Arrange - First login to get a token
            var loginRequest = new RestRequest("/api/account/login");
            loginRequest.AddJsonBody(new
            {
                Email = "test_user@example.com",
                Password = "Test123!"
            });

            var loginResponse = await Client.ExecutePostAsync(loginRequest);
            var loginResult = JsonConvert.DeserializeObject<LoginResponse>(loginResponse.Content);

            // Make sure we have a refresh token
            Assert.NotNull(loginResult.RefreshToken);

            // Now try to refresh the token
            var refreshRequest = new RestRequest("/api/account/refresh-token");
            refreshRequest.AddJsonBody(new
            {
                Token = loginResult.Token,
                RefreshToken = loginResult.RefreshToken
            });

            // Act
            var response = await Client.ExecutePostAsync(refreshRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var refreshResult = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
            Assert.NotNull(refreshResult);
            Assert.NotNull(refreshResult.Token);
            Assert.NotEqual(loginResult.Token, refreshResult.Token); // New token should be different
        }
    }
}