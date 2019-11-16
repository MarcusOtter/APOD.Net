using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Logic.Net
{
    public class HttpRequester : IHttpRequester, IDisposable
    {
        private bool _disposed;

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
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }

            var uri = _uriBuilder.GetApodUri();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime)
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }

            var uri = _uriBuilder.GetApodUri(dateTime);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime startDate, DateTime endDate = default)
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }

            var uri = _uriBuilder.GetApodUri(startDate, endDate);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(int count)
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }

            var uri = _uriBuilder.GetApodUri(count);
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                return await _httpClient.SendAsync(requestMessage);
            }
        }

        public void Dispose()
        {
            if (_disposed) { return; }
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
