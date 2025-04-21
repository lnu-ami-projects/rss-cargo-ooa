using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace RSSCargo.Tests.UI.Pages
{
    public class CargosPage : BasePage
    {
        // Locators
        private readonly By _cargosNavLink = By.LinkText("Your Cargos");
        private readonly By _subscribedCargos = By.CssSelector(".subscribed-cargos .cargo-item");
        private readonly By _unsubscribedCargos = By.CssSelector(".unsubscribed-cargos .cargo-item");
        private readonly By _cargoNames = By.CssSelector(".cargo-name");
        private readonly By _subscribeButtons = By.CssSelector(".subscribe-button");
        private readonly By _unsubscribeButtons = By.CssSelector(".unsubscribe-button");
        private readonly By _viewFeedsButtons = By.CssSelector(".view-cargo-feeds");
        private readonly By _feedCountLabels = By.CssSelector(".feed-count");
        
        public CargosPage(IWebDriver driver) : base(driver)
        {
        }

        public CargosPage Navigate(string baseUrl)
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Rss/Cargos");
            return this;
        }

        public CargosPage GoToCargosPage()
        {
            ClickElement(_cargosNavLink);
            return this;
        }

        public bool IsOnCargosPage()
        {
            return Driver.Url.Contains("/Rss/Cargos");
        }

        public int GetSubscribedCargoCount()
        {
            return Driver.FindElements(_subscribedCargos).Count;
        }

        public int GetUnsubscribedCargoCount()
        {
            return Driver.FindElements(_unsubscribedCargos).Count;
        }

        public List<string> GetSubscribedCargoNames()
        {
            var subscribedCargos = Driver.FindElements(_subscribedCargos);
            var cargoNames = new List<string>();
            
            foreach (var cargo in subscribedCargos)
            {
                var nameElement = cargo.FindElement(_cargoNames);
                cargoNames.Add(nameElement.Text);
            }
            
            return cargoNames;
        }

        public List<string> GetUnsubscribedCargoNames()
        {
            var unsubscribedCargos = Driver.FindElements(_unsubscribedCargos);
            var cargoNames = new List<string>();
            
            foreach (var cargo in unsubscribedCargos)
            {
                var nameElement = cargo.FindElement(_cargoNames);
                cargoNames.Add(nameElement.Text);
            }
            
            return cargoNames;
        }

        public CargosPage SubscribeToCargo(int index = 0)
        {
            var subscribeButtons = Driver.FindElements(_subscribeButtons);
            if (subscribeButtons.Count > index)
            {
                subscribeButtons[index].Click();
                // Wait for page to reload
                WaitUntilElementExists(_cargosNavLink);
            }
            return this;
        }

        public CargosPage UnsubscribeFromCargo(int index = 0)
        {
            var unsubscribeButtons = Driver.FindElements(_unsubscribeButtons);
            if (unsubscribeButtons.Count > index)
            {
                unsubscribeButtons[index].Click();
                // Wait for page to reload
                WaitUntilElementExists(_cargosNavLink);
            }
            return this;
        }

        public CargoFeedsPage ViewCargoFeeds(int index = 0)
        {
            var viewFeedsButtons = Driver.FindElements(_viewFeedsButtons);
            if (viewFeedsButtons.Count > index)
            {
                viewFeedsButtons[index].Click();
                // Wait for navigation
                return new CargoFeedsPage(Driver);
            }
            return new CargoFeedsPage(Driver);
        }

        public bool HasCargo(string cargoName)
        {
            var subscribedNames = GetSubscribedCargoNames();
            var unsubscribedNames = GetUnsubscribedCargoNames();
            
            return subscribedNames.Contains(cargoName) || unsubscribedNames.Contains(cargoName);
        }

        public bool IsCargoSubscribed(string cargoName)
        {
            return GetSubscribedCargoNames().Contains(cargoName);
        }

        public int GetFeedCountForCargo(string cargoName, bool subscribed)
        {
            var cargoList = subscribed ? _subscribedCargos : _unsubscribedCargos;
            var cargos = Driver.FindElements(cargoList);
            
            foreach (var cargo in cargos)
            {
                var nameElement = cargo.FindElement(_cargoNames);
                if (nameElement.Text == cargoName)
                {
                    var countElement = cargo.FindElement(_feedCountLabels);
                    if (int.TryParse(countElement.Text, out int count))
                    {
                        return count;
                    }
                    break;
                }
            }
            
            return 0;
        }
    }
}