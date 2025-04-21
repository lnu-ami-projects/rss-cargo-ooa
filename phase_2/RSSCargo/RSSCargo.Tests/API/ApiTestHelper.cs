using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace RSSCargo.Tests.API
{
    public static class ApiTestHelper
    {
        /// <summary>
        /// Ensures that a user has at least one feed subscribed for testing
        /// </summary>
        public static async Task<int> EnsureUserHasFeed(RestClient client, string authToken)
        {
            var request = new RestRequest("/api/rss/feeds");
            request.AddHeader("Authorization", $"Bearer {authToken}");
            var response = await client.ExecuteGetAsync(request);
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to get feeds: {response.StatusCode}, {response.Content}");
            }
            
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(response.Content);
            
            if (feeds != null && feeds.Count > 0)
            {
                return feeds[0].Id;
            }
            
            // Add a feed if none exists
            var addRequest = new RestRequest("/api/rss/feeds", Method.Post);
            addRequest.AddHeader("Authorization", $"Bearer {authToken}");
            addRequest.AddJsonBody(new { Url = "https://feeds.bbci.co.uk/news/rss.xml" });
            
            var addResponse = await client.ExecutePostAsync(addRequest);
            if (addResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to add feed: {addResponse.StatusCode}, {addResponse.Content}");
            }
            
            // Get the feeds again to find the ID of the newly added feed
            var verifyRequest = new RestRequest("/api/rss/feeds");
            verifyRequest.AddHeader("Authorization", $"Bearer {authToken}");
            var verifyResponse = await client.ExecuteGetAsync(verifyRequest);
            
            var updatedFeeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(verifyResponse.Content);
            return updatedFeeds.FirstOrDefault()?.Id ?? throw new Exception("Failed to find feed after adding");
        }
        
        /// <summary>
        /// Ensures that a user has at least one subscribed cargo for testing
        /// </summary>
        public static async Task<int> EnsureUserHasSubscribedCargo(RestClient client, string authToken)
        {
            var request = new RestRequest("/api/cargos");
            request.AddHeader("Authorization", $"Bearer {authToken}");
            var response = await client.ExecuteGetAsync(request);
            
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to get cargos: {response.StatusCode}, {response.Content}");
            }
            
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(response.Content);
            
            if (cargos.SubscribedCargos.Count > 0)
            {
                return cargos.SubscribedCargos[0].Id;
            }
            
            if (cargos.UnsubscribedCargos.Count == 0)
            {
                throw new Exception("No cargos available for testing");
            }
            
            // Subscribe to a cargo if none is subscribed
            var cargoToSubscribe = cargos.UnsubscribedCargos[0];
            var subscribeRequest = new RestRequest($"/api/cargos/{cargoToSubscribe.Id}/subscribe", Method.Post);
            subscribeRequest.AddHeader("Authorization", $"Bearer {authToken}");
            
            var subscribeResponse = await client.ExecutePostAsync(subscribeRequest);
            if (subscribeResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to subscribe to cargo: {subscribeResponse.StatusCode}, {subscribeResponse.Content}");
            }
            
            return cargoToSubscribe.Id;
        }
        
        /// <summary>
        /// Generates a random test email that won't conflict with existing accounts
        /// </summary>
        public static string GenerateRandomEmail()
        {
            return $"test_{Guid.NewGuid().ToString().Substring(0, 8)}@example.com";
        }
        
        /// <summary>
        /// Creates a test user account and returns the credentials
        /// </summary>
        public static async Task<(string Email, string Password)> CreateTestUserAccount(RestClient client)
        {
            var email = GenerateRandomEmail();
            var password = "Test123!";
            
            var request = new RestRequest("/api/account/register");
            request.AddJsonBody(new
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            });
            
            var response = await client.ExecutePostAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed to create test user: {response.StatusCode}, {response.Content}");
            }
            
            return (email, password);
        }
        
        /// <summary>
        /// Compares the status code of a response with the expected status code
        /// and throws an exception with detailed information if they don't match
        /// </summary>
        public static void AssertStatusCode(HttpStatusCode expected, RestResponse response)
        {
            if (response.StatusCode != expected)
            {
                throw new Exception($"Expected status code {expected} but got {response.StatusCode}. Response content: {response.Content}");
            }
        }
    }
}