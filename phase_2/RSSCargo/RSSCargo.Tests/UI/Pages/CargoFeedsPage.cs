using OpenQA.Selenium;
using System.Collections.Generic;

namespace RSSCargo.Tests.UI.Pages
{
    public class CargoFeedsPage : BasePage
    {
        // Locators
        private readonly By _cargoTitle = By.CssSelector(".cargo-title");
        private readonly By _feedItems = By.CssSelector(".feed-item");
        private readonly By _feedTitles = By.CssSelector(".feed-title");
        private readonly By _feedContents = By.CssSelector(".feed-content");
        private readonly By _backToCargosLink = By.LinkText("Back to Cargos");

        public CargoFeedsPage(IWebDriver driver) : base(driver)
        {
        }

        public bool IsOnCargoFeedsPage()
        {
            return Driver.Url.Contains("/Rss/Feeds/Cargo/") && IsElementPresent(_cargoTitle);
        }

        public string GetCargoTitle()
        {
            return IsElementPresent(_cargoTitle) ? GetElementText(_cargoTitle) : string.Empty;
        }

        public int GetFeedItemCount()
        {
            return Driver.FindElements(_feedItems).Count;
        }

        public List<string> GetFeedTitles()
        {
            var titles = new List<string>();
            var titleElements = Driver.FindElements(_feedTitles);
            
            foreach (var titleElement in titleElements)
            {
                titles.Add(titleElement.Text);
            }
            
            return titles;
        }

        public bool HasFeedContent()
        {
            return Driver.FindElements(_feedContents).Count > 0;
        }

        public CargosPage BackToCargos()
        {
            ClickElement(_backToCargosLink);
            // Wait for navigation back to cargos page
            WaitUntilElementExists(By.LinkText("Your Cargos"));
            return new CargosPage(Driver);
        }
    }
}