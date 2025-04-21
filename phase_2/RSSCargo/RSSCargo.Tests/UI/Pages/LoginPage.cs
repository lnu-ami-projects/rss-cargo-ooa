using OpenQA.Selenium;

namespace RSSCargo.Tests.UI.Pages
{
    public class LoginPage : BasePage
    {
        // Locators
        private readonly By _emailField = By.Id("Email");
        private readonly By _passwordField = By.Id("Password");
        private readonly By _signInButton = By.XPath("//button[contains(text(), 'Sign In')]");
        private readonly By _errorMessage = By.CssSelector(".validation-summary-errors");
        private readonly By _forgotPasswordLink = By.LinkText("Forgot your password?");
        private readonly By _registerLink = By.LinkText("Register as a new user");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public LoginPage Navigate(string baseUrl)
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Account/SignIn");
            return this;
        }

        public LoginPage EnterEmail(string email)
        {
            EnterText(_emailField, email);
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            EnterText(_passwordField, password);
            return this;
        }

        public FeedsPage ClickSignIn()
        {
            ClickElement(_signInButton);
            return new FeedsPage(Driver);
        }

        public bool IsErrorMessageDisplayed()
        {
            return IsElementPresent(_errorMessage);
        }

        public string GetErrorMessage()
        {
            return IsElementPresent(_errorMessage) ? GetElementText(_errorMessage) : string.Empty;
        }

        public ForgotPasswordPage ClickForgotPassword()
        {
            ClickElement(_forgotPasswordLink);
            return new ForgotPasswordPage(Driver);
        }

        public RegisterPage ClickRegister()
        {
            ClickElement(_registerLink);
            return new RegisterPage(Driver);
        }
        
        // Helper method to perform login
        public FeedsPage Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            return ClickSignIn();
        }
    }
}