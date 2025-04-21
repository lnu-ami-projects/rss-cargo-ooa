using RSSCargo.Tests.UI.Pages;
using System;
using Xunit;

namespace RSSCargo.Tests.UI.Tests
{
    public class FeedManagementTests : SeleniumTestBase
    {
        // Test data
        private const string ValidEmail = "test_user@example.com";
        private const string ValidPassword = "Test123!";
        private const string ValidFeedUrl = "http://rss.cnn.com/rss/edition_world.rss";
        private const string AnotherValidFeedUrl = "https://feeds.bbci.co.uk/news/rss.xml";
        private const string InvalidFeedUrl = "http://invalid.feed.url/rss.xml";

        [Fact]
        public void AddFeed_WithValidUrl_AddsFeedToUserList()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            var initialFeedCount = feedsPage.GetFeedCount();

            // Act
            feedsPage.AddFeed(ValidFeedUrl);

            // Assert
            Assert.True(feedsPage.HasFeedWithUrl(ValidFeedUrl), "Feed should be added to user's feed list");
            Assert.Equal(initialFeedCount + 1, feedsPage.GetFeedCount());
        }

        [Fact]
        public void RemoveFeed_FromUserList_RemovesFeedFromList()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            
            // Add a feed first to ensure we have one to remove
            feedsPage.AddFeed(AnotherValidFeedUrl);
            var initialFeedCount = feedsPage.GetFeedCount();

            // Act
            feedsPage.RemoveFeed(0);

            // Assert
            Assert.Equal(initialFeedCount - 1, feedsPage.GetFeedCount());
        }

        [Fact]
        public void ViewFeeds_OnFeedsPage_DisplaysFeedContent()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);

            // Ensure we have a feed
            if (feedsPage.GetFeedCount() == 0)
            {
                feedsPage.AddFeed(ValidFeedUrl);
            }

            // Act - the feeds should be displayed automatically on the feeds page
            
            // Assert
            Assert.True(feedsPage.IsFeedContentDisplayed(), "Feed content should be displayed on feeds page");
        }

        [Fact]
        public void AddFeed_WithInvalidUrl_StaysOnAddFeedPage()
        {
            // Arrange
            var loginPage = new LoginPage(Driver).Navigate(BaseUrl);
            var feedsPage = loginPage.Login(ValidEmail, ValidPassword);
            var initialFeedCount = feedsPage.GetFeedCount();

            // Act
            feedsPage.AddFeed(InvalidFeedUrl);

            // Assert
            // Either the count remains the same, or we stay on the add feed page
            // This behavior might vary depending on how the application handles invalid URLs
            Assert.Equal(initialFeedCount, feedsPage.GetFeedCount());
        }
    }
}