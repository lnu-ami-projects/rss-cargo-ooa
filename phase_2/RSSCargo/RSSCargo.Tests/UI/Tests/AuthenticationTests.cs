using RSSCargo.Tests.UI.Pages;
using System;
using Xunit;

namespace RSSCargo.Tests.UI.Tests
{
    public class AuthenticationTests : SeleniumTestBase
    {
        // Test data
        private const string ValidEmail = "test_user@example.com";
        private const string ValidPassword = "Test123!";
        private const string InvalidEmail = "nonexistent@example.com";
        private const string InvalidPassword = "WrongPassword!";
        private readonly string RandomEmail = $"test_{Guid.NewGuid().ToString().Substring(0, 6)}@example.com";

        [Fact]
        public void Login_WithValidCredentials_RedirectsToFeedsPage()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);

            // Act
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);

            // Assert
            Assert.True(feedsPage.IsOnFeedsPage(), "User should be redirected to feeds page after successful login");
        }

        [Fact]
        public void Login_WithInvalidCredentials_ShowsErrorMessage()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);

            // Act
            loginPage.Login(InvalidEmail, InvalidPassword);

            // Assert
            Assert.True(loginPage.IsErrorMessageDisplayed(), "Error message should be displayed for invalid credentials");
        }

        [Fact]
        public void Register_WithValidData_ShowsSuccessMessage()
        {
            // Arrange
            var registerPage = new RegisterPage(Driver).Navigate(BaseUrl);

            // Act
            var resultPage = registerPage.Register(RandomEmail, ValidPassword);

            // Assert
            Assert.True(resultPage.IsSuccessMessageDisplayed() || 
                       Driver.Url.Contains("/Account/SignIn"), 
                       "Registration should be successful and show confirmation message or redirect to login");
        }

        [Fact]
        public void Logout_FromFeedsPage_RedirectsToHomePage()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            
            // Act
            var resultPage = feedsPage.Logout();
            
            // Assert
            Assert.True(Driver.Url.Contains("/Account/SignIn") || Driver.Url.Contains("/Home"), 
                        "User should be redirected to login or home page after logout");
        }

        [Fact]
        public void ForgotPassword_WithValidEmail_ShowsSuccessMessage()
        {
            // Arrange
            var forgotPasswordPage = new ForgotPasswordPage(Driver).Navigate(BaseUrl);
            
            // Act
            var resultPage = forgotPasswordPage.RequestPasswordReset(ValidEmail);
            
            // Assert
            Assert.True(resultPage.IsSuccessMessageDisplayed(), 
                       "Success message should be displayed after password reset request");
        }
    }
}