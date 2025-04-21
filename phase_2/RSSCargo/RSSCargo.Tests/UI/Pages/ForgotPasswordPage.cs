using OpenQA.Selenium;

namespace RSSCargo.Tests.UI.Pages
{
    public class ForgotPasswordPage : BasePage
    {
        // Locators
        private readonly By _emailField = By.Id("Email");
        private readonly By _submitButton = By.XPath("//button[contains(text(), 'Submit')]");
        private readonly By _errorMessage = By.CssSelector(".validation-summary-errors");
        private readonly By _successMessage = By.CssSelector(".alert-success");

        public ForgotPasswordPage(IWebDriver driver) : base(driver)
        {
        }

        public ForgotPasswordPage Navigate(string baseUrl)
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Account/ForgotPassword");
            return this;
        }

        public ForgotPasswordPage EnterEmail(string email)
        {
            EnterText(_emailField, email);
            return this;
        }

        public ForgotPasswordPage ClickSubmit()
        {
            ClickElement(_submitButton);
            return this;
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

        // Helper method to request password reset
        public ForgotPasswordPage RequestPasswordReset(string email)
        {
            EnterEmail(email);
            return ClickSubmit();
        }
    }
}