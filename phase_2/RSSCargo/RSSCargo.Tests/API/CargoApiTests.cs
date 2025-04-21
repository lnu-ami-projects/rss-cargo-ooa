using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace RSSCargo.Tests.API
{
    public class CargoApiTests : ApiTestBase
    {
        private readonly string _validEmail = "test_user@example.com";
        private readonly string _validPassword = "Test123!";

        [Fact]
        public async Task GetCargos_WhenAuthorized_ReturnsUserCargos()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var request = CreateAuthorizedRequest("/api/cargos");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var result = JsonConvert.DeserializeObject<CargoListResponse>(response.Content);
            Assert.NotNull(result);
            Assert.NotNull(result.SubscribedCargos);
            Assert.NotNull(result.UnsubscribedCargos);
        }

        [Fact]
        public async Task GetCargos_WhenUnauthorized_ReturnsUnauthorized()
        {
            // Arrange - No authentication
            var request = new RestRequest("/api/cargos");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetCargoFeeds_WithValidId_ReturnsCargoFeeds()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // First get all cargos to find an existing one
            var cargosRequest = CreateAuthorizedRequest("/api/cargos");
            var cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            
            // Ensure there's at least one subscribed cargo
            if (cargos.SubscribedCargos.Count == 0)
            {
                // If no subscribed cargos, try to subscribe to one first
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
            var request = CreateAuthorizedRequest($"/api/cargos/{cargoId}/feeds");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var feeds = JsonConvert.DeserializeObject<List<RssFeedResponse>>(response.Content);
            Assert.NotNull(feeds);
            // Note: A cargo might have 0 feeds, so we don't assert on the count
        }

        [Fact]
        public async Task GetCargoFeeds_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidCargoId = 99999; // Assuming this ID doesn't exist
            var request = CreateAuthorizedRequest($"/api/cargos/{invalidCargoId}/feeds");

            // Act
            var response = await Client.ExecuteGetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SubscribeToCargo_WithValidId_SubscribesAndReturnsSuccess()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get all cargos to find one to subscribe to
            var cargosRequest = CreateAuthorizedRequest("/api/cargos");
            var cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            
            // If no unsubscribed cargos, try to unsubscribe from one first
            if (cargos.UnsubscribedCargos.Count == 0 && cargos.SubscribedCargos.Count > 0)
            {
                var cargoToUnsubscribe = cargos.SubscribedCargos[0];
                var unsubscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoToUnsubscribe.Id}/unsubscribe", Method.Post);
                await Client.ExecutePostAsync(unsubscribeRequest);
                
                // Get updated cargo list
                cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
                cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            }
            
            Assert.True(cargos.UnsubscribedCargos.Count > 0, "Need at least one unsubscribed cargo for this test");
            
            var cargoId = cargos.UnsubscribedCargos[0].Id;
            var subscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoId}/subscribe", Method.Post);

            // Act
            var response = await Client.ExecutePostAsync(subscribeRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // Verify the cargo was subscribed
            var verifyRequest = CreateAuthorizedRequest("/api/cargos");
            var verifyResponse = await Client.ExecuteGetAsync(verifyRequest);
            var updatedCargos = JsonConvert.DeserializeObject<CargoListResponse>(verifyResponse.Content);
            Assert.Contains(updatedCargos.SubscribedCargos, c => c.Id == cargoId);
        }

        [Fact]
        public async Task UnsubscribeFromCargo_WithValidId_UnsubscribesAndReturnsSuccess()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get all cargos to find one to unsubscribe from
            var cargosRequest = CreateAuthorizedRequest("/api/cargos");
            var cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            
            // If no subscribed cargos, try to subscribe to one first
            if (cargos.SubscribedCargos.Count == 0 && cargos.UnsubscribedCargos.Count > 0)
            {
                var cargoToSubscribe = cargos.UnsubscribedCargos[0];
                var subscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoToSubscribe.Id}/subscribe", Method.Post);
                await Client.ExecutePostAsync(subscribeRequest);
                
                // Get updated cargo list
                cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
                cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            }
            
            Assert.True(cargos.SubscribedCargos.Count > 0, "Need at least one subscribed cargo for this test");
            
            var cargoId = cargos.SubscribedCargos[0].Id;
            var unsubscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoId}/unsubscribe", Method.Post);

            // Act
            var response = await Client.ExecutePostAsync(unsubscribeRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            // Verify the cargo was unsubscribed
            var verifyRequest = CreateAuthorizedRequest("/api/cargos");
            var verifyResponse = await Client.ExecuteGetAsync(verifyRequest);
            var updatedCargos = JsonConvert.DeserializeObject<CargoListResponse>(verifyResponse.Content);
            Assert.Contains(updatedCargos.UnsubscribedCargos, c => c.Id == cargoId);
        }

        [Fact]
        public async Task SubscribeToCargo_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidCargoId = 99999; // Assuming this ID doesn't exist
            var request = CreateAuthorizedRequest($"/api/cargos/{invalidCargoId}/subscribe", Method.Post);

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UnsubscribeFromCargo_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            var invalidCargoId = 99999; // Assuming this ID doesn't exist
            var request = CreateAuthorizedRequest($"/api/cargos/{invalidCargoId}/unsubscribe", Method.Post);

            // Act
            var response = await Client.ExecutePostAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SubscribeToCargo_WhenAlreadySubscribed_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get all cargos to find one that's already subscribed
            var cargosRequest = CreateAuthorizedRequest("/api/cargos");
            var cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            
            // If no subscribed cargos, subscribe to one first
            if (cargos.SubscribedCargos.Count == 0 && cargos.UnsubscribedCargos.Count > 0)
            {
                var cargoToSubscribe = cargos.UnsubscribedCargos[0];
                var initialSubscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoToSubscribe.Id}/subscribe", Method.Post);
                await Client.ExecutePostAsync(initialSubscribeRequest);
                
                // Get updated cargo list
                cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
                cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            }
            
            Assert.True(cargos.SubscribedCargos.Count > 0, "Need at least one subscribed cargo for this test");
            
            var cargoId = cargos.SubscribedCargos[0].Id;
            var subscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoId}/subscribe", Method.Post);

            // Act
            var response = await Client.ExecutePostAsync(subscribeRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UnsubscribeFromCargo_WhenNotSubscribed_ReturnsBadRequest()
        {
            // Arrange
            await AuthenticateAsync(_validEmail, _validPassword);
            
            // Get all cargos to find one that's not subscribed
            var cargosRequest = CreateAuthorizedRequest("/api/cargos");
            var cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
            var cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            
            // If no unsubscribed cargos, unsubscribe from one first
            if (cargos.UnsubscribedCargos.Count == 0 && cargos.SubscribedCargos.Count > 0)
            {
                var cargoToUnsubscribe = cargos.SubscribedCargos[0];
                var initialUnsubscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoToUnsubscribe.Id}/unsubscribe", Method.Post);
                await Client.ExecutePostAsync(initialUnsubscribeRequest);
                
                // Get updated cargo list
                cargosResponse = await Client.ExecuteGetAsync(cargosRequest);
                cargos = JsonConvert.DeserializeObject<CargoListResponse>(cargosResponse.Content);
            }
            
            Assert.True(cargos.UnsubscribedCargos.Count > 0, "Need at least one unsubscribed cargo for this test");
            
            var cargoId = cargos.UnsubscribedCargos[0].Id;
            var unsubscribeRequest = CreateAuthorizedRequest($"/api/cargos/{cargoId}/unsubscribe", Method.Post);

            // Act
            var response = await Client.ExecutePostAsync(unsubscribeRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }

    public class CargoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int FeedCount { get; set; }
    }

    public class CargoListResponse
    {
        public List<CargoResponse> SubscribedCargos { get; set; } = new List<CargoResponse>();
        public List<CargoResponse> UnsubscribedCargos { get; set; } = new List<CargoResponse>();
    }
}