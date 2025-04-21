using RSSCargo.Tests.UI.Pages;
using System;
using Xunit;

namespace RSSCargo.Tests.UI.Tests
{
    public class CargoManagementTests : SeleniumTestBase
    {
        // Test data
        private const string ValidEmail = "test_user@example.com";
        private const string ValidPassword = "Test123!";
        private const string WorldNewsCargoName = "World News";
        private const string BusinessCargoName = "Business";

        [Fact]
        public void ViewCargos_DisplaysSubscribedAndUnsubscribedCargos()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            
            // Act
            var cargosPage = new CargosPage(Driver).Navigate(BaseUrl);
            
            // Assert
            Assert.True(cargosPage.IsOnCargosPage(), "Should navigate to cargos page");
            Assert.True(cargosPage.GetSubscribedCargoCount() > 0 || cargosPage.GetUnsubscribedCargoCount() > 0, 
                "Should display at least some cargos (either subscribed or unsubscribed)");
        }

        [Fact]
        public void SubscribeToCargo_MovesCargoToSubscribedList()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            var cargosPage = new CargosPage(Driver).Navigate(BaseUrl);
            
            // Make sure we have an unsubscribed cargo to work with
            if (cargosPage.GetUnsubscribedCargoCount() == 0)
            {
                // We need to unsubscribe from a cargo first to have something to subscribe to
                if (cargosPage.GetSubscribedCargoCount() > 0)
                {
                    cargosPage.UnsubscribeFromCargo(0);
                }
                else
                {
                    Assert.True(false, "No cargos available to test subscription functionality");
                }
            }
            
            // Get name of cargo we're going to subscribe to
            var unsubscribedNames = cargosPage.GetUnsubscribedCargoNames();
            var cargoToSubscribe = unsubscribedNames[0];
            var initialSubscribedCount = cargosPage.GetSubscribedCargoCount();
            
            // Act
            cargosPage.SubscribeToCargo(0);
            
            // Assert
            Assert.Equal(initialSubscribedCount + 1, cargosPage.GetSubscribedCargoCount());
            Assert.True(cargosPage.IsCargoSubscribed(cargoToSubscribe), 
                $"Cargo '{cargoToSubscribe}' should be in the subscribed list");
        }

        [Fact]
        public void UnsubscribeFromCargo_MovesCargoToUnsubscribedList()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            var cargosPage = new CargosPage(Driver).Navigate(BaseUrl);
            
            // Make sure we have a subscribed cargo to work with
            if (cargosPage.GetSubscribedCargoCount() == 0)
            {
                // We need to subscribe to a cargo first to have something to unsubscribe from
                if (cargosPage.GetUnsubscribedCargoCount() > 0)
                {
                    cargosPage.SubscribeToCargo(0);
                }
                else
                {
                    Assert.True(false, "No cargos available to test unsubscription functionality");
                }
            }
            
            // Get name of cargo we're going to unsubscribe from
            var subscribedNames = cargosPage.GetSubscribedCargoNames();
            var cargoToUnsubscribe = subscribedNames[0];
            var initialUnsubscribedCount = cargosPage.GetUnsubscribedCargoCount();
            
            // Act
            cargosPage.UnsubscribeFromCargo(0);
            
            // Assert
            Assert.Equal(initialUnsubscribedCount + 1, cargosPage.GetUnsubscribedCargoCount());
            Assert.False(cargosPage.IsCargoSubscribed(cargoToUnsubscribe), 
                $"Cargo '{cargoToUnsubscribe}' should not be in the subscribed list");
        }

        [Fact]
        public void ViewCargoFeeds_DisplaysAggregatedFeedContent()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            var cargosPage = new CargosPage(Driver).Navigate(BaseUrl);
            
            // Make sure we have a subscribed cargo to view
            if (cargosPage.GetSubscribedCargoCount() == 0)
            {
                // We need to subscribe to a cargo first
                if (cargosPage.GetUnsubscribedCargoCount() > 0)
                {
                    cargosPage.SubscribeToCargo(0);
                }
                else
                {
                    Assert.True(false, "No cargos available to test view feeds functionality");
                }
            }
            
            // Act
            var cargoFeedsPage = cargosPage.ViewCargoFeeds(0);
            
            // Assert
            Assert.True(cargoFeedsPage.IsOnCargoFeedsPage(), "Should navigate to cargo feeds page");
            Assert.True(cargoFeedsPage.HasFeedContent(), "Should display aggregated feed content");
        }
    }
}