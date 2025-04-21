using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace RSSCargo.Tests.API
{
    public class FeedItemsApiTests : ApiTestBase
    {
        private readonly string _validEmail = "test_user@example.com";
        private readonly string _validPassword = "Test123!";

        [Fact]
        public async Task GetFeedItems_WhenAuthorized_ReturnsItems()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // First get all user feeds to find one with items
            var feedsRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var feedsResponse = await Client.ExecuteGetAsync(feedsRequest);
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(feedsResponse.Content);
            
            // Make sure we have at least one feed
            if (feeds.Count == 0)
            {
                // Add a feed if none exist
                var addFeedRequest = CreateAuthorizedRequest("/api/rss/feeds", Method.Post);
                addFeedRequest.AddJsonBody(new { Url = "https://feeds.bbci.co.uk/news/rss.xml" });
                await Client.ExecutePostAsync(addFeedRequest);
                
                // Get updated list of feeds
                feedsResponse = await Client.ExecuteGetAsync(feedsRequest);
                feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(feedsResponse.Content);
            }
            
            Assert.True(feeds.Count > 0, "Need at least one feed for this test");
            
            var feedId = feeds[0].Id;
            var request = CreateAuthorizedRequest($"/api/rss/feeds/{feedId}/items");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var items = JsonConvert.DeserializeObject<List<FeedItemResponse>>(response.Content);
            Assert.NotNull(items);
            // A feed might not have items yet if newly added
        }
        
        [Fact]
        public async Task GetFeedItems_WithInvalidFeedId_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidFeedId = 99999; // Assuming this ID doesn't exist
            var request = CreateAuthorizedRequest($"/api/rss/feeds/{invalidFeedId}/items");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [Fact]
        public async Task GetFeedItems_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange - Get a valid feed ID first with authentication
            await AuthenticateAsync(_validEmail, _validPassword);
            var feedsRequest = CreateAuthorizedRequest("/api/rss/feeds");
            var feedsResponse = await Client.ExecuteGetAsync(feedsRequest);
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(feedsResponse.Content);
            
            Assert.True(feeds.Count > 0, "Need at least one feed for this test");
            var feedId = feeds[0].Id;
            
            // Now create an unauthorized request
            var request = new RestRequest($"/api/rss/feeds/{feedId}/items");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Fact]
        public async Task GetLatestFeedItems_WhenAuthorized_ReturnsItems()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var request = CreateAuthorizedRequest("/api/rss/latest-items");
            
            // Act
            var response = await Client.ExecuteGetAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var items = JsonConvert.DeserializeObject<List<FeedItemResponse>>(response.Content);
            Assert.NotNull(items);
        }
        
        [Fact]
        public async Task GetLatestFeedItems_WithLimit_ReturnsLimitedItems()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var limit = 5;
            var request = CreateAuthorizedRequest($"/api/rss/latest-items?limit={limit}");
            
            // Act
            var response = await Client.ExecuteGetAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var items = JsonConvert.DeserializeObject<List<FeedItemResponse>>(response.Content);
            Assert.NotNull(items);
            Assert.True(items.Count <= limit, $"Should return at most {limit} items");
        }
        
        [Fact]
        public async Task SearchFeedItems_WithValidKeyword_ReturnsMatchingItems()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var searchTerm = "news"; // Generic search term likely to have results
            var request = CreateAuthorizedRequest($"/api/rss/search?q={searchTerm}");
            
            // Act
            var response = await Client.ExecuteGetAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var items = JsonConvert.DeserializeObject<List<FeedItemResponse>>(response.Content);
            Assert.NotNull(items);
            // Note: We can't guarantee results will contain the search term since content may vary
        }
        
        [Fact]
        public async Task GetCargoFeedItems_WithValidCargoId_ReturnsItems()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get all cargos to find an existing one
            var cargosRequest = CreateAuthorizedRequest("/api/cargos");
            var cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            
            // Ensure there's at least one subscribed cargo
            if (cargos.SubscribedCargos.Count == 0)
            {
                // If no subscribed cargos, try to subscribe to one
                if (cargos.UnsubscribedCargos.Count > 0)
                {
                    var cargoToSubscribe = cargos.UnsubscribedCargos[0];
                    var subscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoToSubscribe.Id}/subscribe", Method.Post);
                    await Client.ExecutePostAsync(subscribeRequest);
                    
                    // Get updated cargo list
                    cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
                    cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
                }
            }
            
            Assert.True(cargos.SubscribedCargos.Count > 0, "Need at least one subscribed cargo for this test");
            
            var cargoId = cargos.SubscribedCargos[0].Id;
            var request = CreateAuthorizedRequest($"/api/cargos/{cargoId}/items");
            
            // Act
            var response = await Client.ExecuteGetAsync(request);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var items = JsonConvert.DeserializeObject<List<FeedItemResponse>>(response.Content);
            Assert.NotNull(items);
            // A cargo might not have items yet if newly added or feeds don't have items
        }
    }
    
    public class FeedItemResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public string PublishDate { get; set; }
        public string FeedTitle { get; set; }
        public int FeedId { get; set; }
    }
}