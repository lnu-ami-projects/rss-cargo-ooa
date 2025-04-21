using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace RSSCargo.Tests.UI.Pages
{
    public class FeedsPage : BasePage
    {
        // Locators
        private readonly By _addFeedUrlField = By.Id("feedUrl");
        private readonly By _addFeedButton = By.XPath("//button[contains(text(), 'Add')]");
        private readonly By _feedItems = By.CssSelector(".feed-item");
        private readonly By _feedTitles = By.CssSelector(".feed-title");
        private readonly By _removeFeedButtons = By.CssSelector(".remove-feed-button");
        private readonly By _feedsNavLink = By.LinkText("Your Feeds");
        private readonly By _userDropdown = By.Id("userDropdown");
        private readonly By _logoutLink = By.Id("logout-button");
        private readonly By _feedUrlList = By.CssSelector(".feed-url");

        public FeedsPage(IWebDriver driver) : base(driver)
        {
        }

        public FeedsPage Navigate(string baseUrl)
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Rss/Feeds");
            return this;
        }

        public bool IsOnFeedsPage()
        {
            return Driver.Url.Contains("/Rss/Feeds") && IsElementPresent(_addFeedUrlField);
        }

        public FeedsPage AddFeed(string url)
        {
            ClickElement(_feedsNavLink);
            EnterText(_addFeedUrlField, url);
            ClickElement(_addFeedButton);
            // Wait for page to reload
            WaitUntilElementExists(_addFeedUrlField);
            return this;
        }

        public FeedsPage RemoveFeed(int index = 0)
        {
            var removeButtons = Driver.FindElements(_removeFeedButtons);
            if (removeButtons.Count > index)
            {
                removeButtons[index].Click();
                // Wait for page to reload
                WaitUntilElementExists(_addFeedUrlField);
            }
            return this;
        }

        public List<string> GetFeedUrls()
        {
            var urlElements = Driver.FindElements(_feedUrlList);
            return urlElements.Select(e => e.Text).ToList();
        }

        public bool HasFeedWithUrl(string url)
        {
            return GetFeedUrls().Any(feedUrl => feedUrl.Contains(url));
        }

        public int GetFeedCount()
        {
            return Driver.FindElements(_feedItems).Count;
        }

        public LoginPage Logout()
        {
            if (IsElementPresent(_userDropdown))
            {
                ClickElement(_userDropdown);
                ClickElement(_logoutLink);
                // Wait for redirect to login page
                WaitUntilElementExists(By.Id("Email"));
                return new LoginPage(Driver);
            }
            return new LoginPage(Driver);
        }

        public bool IsFeedContentDisplayed()
        {
            return Driver.FindElements(_feedTitles).Count > 0;
        }
    }
}