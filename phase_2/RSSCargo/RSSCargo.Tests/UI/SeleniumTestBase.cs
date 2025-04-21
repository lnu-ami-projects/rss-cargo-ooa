using System;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace RSSCargo.Tests.UI
{
    public class SeleniumTestBase : IDisposable
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;
        protected readonly string BaseUrl = "http://localhost:5000";  // Adjust to your application's URL

        public SeleniumTestBase()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");  // Run in headless mode (no UI) - remove for debugging
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            // Explicit wait for certain operations
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
        }

        public void Dispose()
        {
            Driver?.Quit();
            Driver?.Dispose();
        }

        /// <summary>
        /// Helper method to wait for an element to be clickable
        /// </summary>
        protected IWebElement WaitForElementToBeClickable(By locator)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// Helper method to wait for an element to be visible
        /// </summary>
        protected IWebElement WaitForElementVisible(By locator)
        {
            return Wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>
        /// Helper method to take a screenshot for debugging
        /// </summary>
        protected void TakeScreenshot(string fileName)
        {
            try
            {
                var screenshotDriver = (ITakesScreenshot)Driver;
                var screenshot = screenshotDriver.GetScreenshot();
                var screenshotDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                
                if (!Directory.Exists(screenshotDirectory))
                {
                    Directory.CreateDirectory(screenshotDirectory);
                }

                var screenshotPath = Path.Combine(screenshotDirectory, $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                Console.WriteLine($"Screenshot saved to {screenshotPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
    }
}