using System;
using Xunit;
using Moq;
using Apod.Logic.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApodTests
{
    public class HttpRequesterTests
    {
        private readonly Mock<IApodUriBuilder> _uriBuilder;
        private readonly Mock<HttpClient> _httpClient;
        private readonly IHttpRequester _httpRequester;

        public HttpRequesterTests()
        {
            _uriBuilder = new Mock<IApodUriBuilder>();
            _httpClient = new Mock<HttpClient>();
            _httpRequester = new HttpRequester(_uriBuilder.Object, _httpClient.Object);
        }

        [Fact]
        public async Task SendHttpRequestAsync_Today_ThrowsIfDisposed()
        {
            _httpRequester.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _httpRequester.SendHttpRequestAsync());
        }

        [Fact]
        public async Task SendHttpRequestAsync_Date_ThrowsIfDisposed()
        {
            var exampleInput = new DateTime(2008, 01, 20);
            _httpRequester.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _httpRequester.SendHttpRequestAsync(exampleInput));
        }

        [Fact]
        public async Task SendHttpRequestAsync_DateRange_ThrowsIfDisposed()
        {
            var exampleInput1 = new DateTime(2003, 08, 03);
            var exampleInput2 = new DateTime(2003, 09, 01);
            _httpRequester.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _httpRequester.SendHttpRequestAsync(exampleInput1, exampleInput2));
        }

        [Fact]
        public async Task SendHttpRequestAsync_Count_ThrowsIfDisposed()
        {
            var exampleInput = 3;
            _httpRequester.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _httpRequester.SendHttpRequestAsync(exampleInput));
        }
    }
}
