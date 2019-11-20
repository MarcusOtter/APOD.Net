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
            ThrowExceptionIfDisposed();

            var uri = _uriBuilder.GetApodUri();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await _httpClient.SendAsync(requestMessage);

            requestMessage.Dispose();
            return response;
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime)
        {
            ThrowExceptionIfDisposed();

            var uri = _uriBuilder.GetApodUri(dateTime);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await _httpClient.SendAsync(requestMessage);

            requestMessage.Dispose();
            return response;
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(DateTime startDate, DateTime endDate = default)
        {
            ThrowExceptionIfDisposed();

            var uri = _uriBuilder.GetApodUri(startDate, endDate);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await _httpClient.SendAsync(requestMessage);

            requestMessage.Dispose();
            return response;
        }

        public async Task<HttpResponseMessage> SendHttpRequestAsync(int count)
        {
            ThrowExceptionIfDisposed();

            var uri = _uriBuilder.GetApodUri(count);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            var response = await _httpClient.SendAsync(requestMessage);

            requestMessage.Dispose();
            return response;
        }

        private void ThrowExceptionIfDisposed()
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }
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
