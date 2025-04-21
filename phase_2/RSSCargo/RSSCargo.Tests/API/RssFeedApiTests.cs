using System.Net;
using Newtonsoft.Json;
using RestSharp;
using RSSCargo.DAL.Models;
using Xunit;

namespace RSSCargo.Tests.API
{
    public class RssFeedApiTests : ApiTestBase
    {
        private readonly string _validEmail = "test_user@example.com";
        private readonly string _validPassword = "Test123!";

        [Fact]
        public async Task GetFeeds_WhenAuthorized_ReturnsUserFeeds()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var request = CreateAuthorizedRequest("/api/rss/feeds");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(response.Content);
            Assert.NotNull(feeds);
            Assert.True(feeds.Count > 0, "Should return at least one feed");
        }

        [Fact]
        public async Task GetFeeds_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange - No authentication
            var request = new RestRequest("/api/rss/feeds");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AddFeed_WithValidUrl_AddsFeedAndReturnsSuccess()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var validFeedUrl = "https://feeds.bbci.co.uk/news/rss.xml";
            var request = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
            request.AddJsonBody(new { Url = validFeedUrl });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // Verify feed was added by getting all feeds and checking if it exists
            var getFeedsRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var getFeedsResponse = await Client.ExecuteGetAsync(getFeedsRequest);
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(getFeedsResponse.Content);
            Assert.Contains(feeds, f => f.Link == validFeedUrl);
        }

        [Fact]
        public async Task AddFeed_WithInvalidUrl_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidFeedUrl = "not-a-valid-url";
            var request = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
            request.AddJsonBody(new { Url = invalidFeedUrl });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveFeed_WithValidId_RemovesFeedAndReturnsSuccess()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);

            // First, add a feed we can then remove
            var tempFeedUrl = "http://rss.cnn.com/rss/edition_world.rss";
            var addRequest = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
            addRequest.AddJsonBody(new { Url = tempFeedUrl });
            await Client.ExecutePostAsync(addRequest);

            // Find the ID of the feed we just added
            var getFeedsRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var getFeedsResponse = await Client.ExecuteGetAsync(getFeedsRequest);
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(getFeedsResponse.Content);
            var feedToRemove = feeds.FirstOrDefault(f => f.Link == tempFeedUrl);

            Assert.NotNull(feedToRemove);

            var removeRequest = CreateAuthorizedRequest($"/api/rss/feeds/{feedToRemove.Id}", Method.Delete);

            // Act
            var response = await Client.ExecuteDeleteAsync(removeRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // Verify feed was removed
            var verifyRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var verifyResponse = await Client.ExecuteGetAsync(verifyRequest);
            var updatedFeeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(verifyResponse.Content);
            Assert.DoesNotContain(updatedFeeds, f => f.Link == tempFeedUrl);
        }

        [Fact]
        public async Task RemoveFeed_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidFeedId = 99999; // Assuming this ID doesn't exist
            var request = CreateAuthorizedRequest($"/api/rss/feeds/{invalidFeedId}", Method.Delete);

            // Act
            var response = await Client.ExecuteDeleteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task AddFeed_WithDuplicateUrl_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get existing feeds to find one that's already added
            var getFeedsRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var getFeedsResponse = await Client.ExecuteGetAsync(getFeedsRequest);
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(getFeedsResponse.Content);
            
            // If no feeds exist, add one first
            if (feeds.Count == 0)
            {
                var tempFeedUrl = "https://feeds.bbci.co.uk/news/rss.xml";
                var addRequest = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
                addRequest.AddJsonBody(new { Url = tempFeedUrl });
                await Client.ExecutePostAsync(addRequest);
                
                // Get updated list of feeds
                getFeedsResponse = await Client.ExecuteGetAsync(getFeedsRequest);
                feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(getFeedsResponse.Content);
            }
            
            Assert.True(feeds.Count > 0, "Need at least one feed for this test");
            
            var existingFeedUrl = feeds[0].Link;
            var request = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
            request.AddJsonBody(new { Url = existingFeedUrl });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddFeed_WithMalformedRssFeed_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var malformedFeedUrl = "https://example.com"; // Not an RSS feed
            var request = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
            request.AddJsonBody(new { Url = malformedFeedUrl });

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetFeedById_WithValidId_ReturnsFeed()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get existing feeds to find one that exists
            var getFeedsRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var getFeedsResponse = await Client.ExecuteGetAsync(getFeedsRequest);
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(getFeedsResponse.Content);
            
            // If no feeds exist, add one first
            if (feeds.Count == 0)
            {
                var tempFeedUrl = "https://feeds.bbci.co.uk/news/rss.xml";
                var addRequest = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
                addRequest.AddJsonBody(new { Url = tempFeedUrl });
                await Client.ExecutePostAsync(addRequest);
                
                // Get updated list of feeds
                getFeedsResponse = await Client.ExecuteGetAsync(getFeedsRequest);
                feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(getFeedsResponse.Content);
            }
            
            Assert.True(feeds.Count > 0, "Need at least one feed for this test");
            
            var feedId = feeds[0].Id;
            var request = CreateAuthorizedRequest($"/api/rss/feeds/{feedId}");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var feed = JsonConvert.DeserializeObject<RssFeedResponse>(response.Content);
            Assert.NotNull(feed);
            Assert.Equal(feedId, feed.Id);
        }

        [Fact]
        public async Task GetFeedById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidFeedId = 99999; // Assuming this ID doesn't exist
            var request = CreateAuthorizedRequest($"/api/rss/feeds/{invalidFeedId}");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    public class RssFeedResponse
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public string Authors { get; set; }
    }
}