using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Xunit;

namespace RSSCargo.Tests.API
{
    public abstract class ApiTestBase
    {
        protected RestClient Client { get; }
        protected string BaseUrl { get; }
        protected string AuthToken { get; private set; }

        protected ApiTestBase()
        {
            // Set the base URL for the API - this should be configurable for different environments
            BaseUrl = "http://localhost:5000";
            Client = new RestClient(BaseUrl);
        }

        protected async Task AuthenticateAsync(string email, string password)
        {
            var request = new RestRequest("/api/account/login", Method.Post);
            request.AddJsonBody(new { Email = email, Password = password });

            var response = await Client.ExecutePostAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Authentication failed with status code: {response.StatusCode}, response: {response.Content}");
            }

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content);
            AuthToken = loginResponse.Token;
        }

        protected RestRequest CreateAuthorizedRequest(string resource, Method method = Method.Get)
        {
            var request = new RestRequest(resource, method);
            if (!string.IsNullOrEmpty(AuthToken))
            {
                request.AddHeader("Authorization", $"Bearer {AuthToken}");
            }
            return request;
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}