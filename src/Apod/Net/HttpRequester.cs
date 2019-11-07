using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Net
{
    public class HttpRequester : IHttpRequester
    {
        private readonly IApodUriBuilder _uriBuilder;
        private readonly HttpClient _httpClient;

        public HttpRequester(IApodUriBuilder uriBuilder, HttpClient httpClient = null)
        {
            _uriBuilder = uriBuilder;
            _httpClient = httpClient ?? GetDefaultHttpClient();
        }

        private HttpClient GetDefaultHttpClient()
            => new HttpClient();

        public async Task<HttpResponseMessage> SendHttpRequestAsync()
        {
            var uri = _uriBuilder.GetApodUri();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            return await _httpClient.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime)
        {
            var uri = _uriBuilder.GetApodUri(dateTime);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime startDate, DateTime endDate = default)
        {
            var uri = _uriBuilder.GetApodUri(startDate, endDate);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(int count)
        {
            var uri = _uriBuilder.GetApodUri(count);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }
    }
}
