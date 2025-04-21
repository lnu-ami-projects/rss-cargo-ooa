using OpenQA.Selenium;

namespace RSSCargo.Tests.UI.Pages
{
    public class RegisterPage : BasePage
    {
        // Locators
        private readonly By _emailField = By.Id("Email");
        private readonly By _passwordField = By.Id("Password");
        private readonly By _confirmPasswordField = By.Id("ConfirmPassword");
        private readonly By _registerButton = By.XPath("//button[contains(text(), 'Register')]");
        private readonly By _errorMessage = By.CssSelector(".validation-summary-errors");
        private readonly By _successMessage = By.CssSelector(".alert-success");
        private readonly By _loginLink = By.LinkText("Login");

        public RegisterPage(IWebDriver driver) : base(driver)
        {
        }

        public RegisterPage Navigate(string baseUrl)
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Account/SignUp");
            return this;
        }

        public RegisterPage EnterEmail(string email)
        {
            EnterText(_emailField, email);
            return this;
        }

        public RegisterPage EnterPassword(string password)
        {
            EnterText(_passwordField, password);
            return this;
        }

        public RegisterPage EnterConfirmPassword(string password)
        {
            EnterText(_confirmPasswordField, password);
            return this;
        }

        public RegisterPage ClickRegister()
        {
            ClickElement(_registerButton);
            return this;
        }

        public LoginPage ClickLogin()
        {
            ClickElement(_loginLink);
            return new LoginPage(Driver);
        }

        public bool IsErrorMessageDisplayed()
        {
            return IsElementPresent(_errorMessage);
        }

        public bool IsSuccessMessageDisplayed()
        {
            return IsElementPresent(_successMessage);
        }

        public string GetErrorMessage()
        {
            return IsElementPresent(_errorMessage) ? GetElementText(_errorMessage) : string.Empty;
        }

        public string GetSuccessMessage()
        {
            return IsElementPresent(_successMessage) ? GetElementText(_successMessage) : string.Empty;
        }

        // Helper method to register a new user
        public RegisterPage Register(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            EnterConfirmPassword(password);
            return ClickRegister();
        }
    }
}