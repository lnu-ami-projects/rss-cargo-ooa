using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace RSSCargo.Tests.UI.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement WaitUntilElementExists(By locator)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        }

        protected IWebElement WaitUntilElementClickable(By locator)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        protected bool IsElementPresent(By locator)
        {
            try
            {
                Driver.FindElement(locator);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        protected void ClickElement(By locator)
        {
            WaitUntilElementClickable(locator).Click();
        }

        protected void EnterText(By locator, string text)
        {
            var element = WaitUntilElementExists(locator);
            element.Clear();
            element.SendKeys(text);
        }

        protected string GetElementText(By locator)
        {
            return WaitUntilElementExists(locator).Text;
        }
    }
}