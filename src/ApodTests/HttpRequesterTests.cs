using System;
using Xunit;
using Moq;
using Apod.Logic.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq.Protected;
using System.Threading;
using System.Net;

namespace ApodTests
{
    public class HttpRequesterTests
    {
        private readonly Mock<IApodUriBuilder> _uriBuilder;
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private readonly IHttpRequester _httpRequester;

        public HttpRequesterTests()
        {
            _uriBuilder = new Mock<IApodUriBuilder>();
            _httpMessageHandler = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandler.Object);
            _httpRequester = new HttpRequester(_uriBuilder.Object, httpClient);
        }

        [Fact]
        public async Task SendHttpRequestAsync_Today_CorrectHttpRequest()
        {
            var uri = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY";

            SetupHttpMessageHandler(uri);
            SetupUriBuilder(uri);

            await _httpRequester.SendHttpRequestAsync();

            AssertSendAsyncWasCalledWithUri(uri);
        }

        [Fact]
        public async Task SendHttpRequestAsync_Date_CorrectHttpRequest()
        {
            var date = new DateTime(2019, 11, 06);
            var uri = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&date=2019-11-06";

            SetupHttpMessageHandler(uri);
            SetupUriBuilder(uri);

            await _httpRequester.SendHttpRequestAsync(date);

            AssertSendAsyncWasCalledWithUri(uri);
        }

        [Fact]
        public async Task SendHttpRequestAsync_DateRange_CorrectHttpRequest()
        {
            var startDate = new DateTime(2007, 06, 25);
            var endDate = new DateTime(2007, 08, 09);
            var uri = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&start_date=2007-06-25&end_date=2007-08-09";

            SetupHttpMessageHandler(uri);
            SetupUriBuilder(uri);

            await _httpRequester.SendHttpRequestAsync(startDate, endDate);

            AssertSendAsyncWasCalledWithUri(uri);
        }

        [Fact]
        public async Task SendHttpRequestAsync_Count_CorrectHttpRequest()
        {
            var count = 10;
            var uri = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY&count=10";

            SetupHttpMessageHandler(uri);
            SetupUriBuilder(uri);

            await _httpRequester.SendHttpRequestAsync(count);

            AssertSendAsyncWasCalledWithUri(uri);
        }

        private void AssertSendAsyncWasCalledWithUri(string uri)
        {
            _httpMessageHandler
                .Protected()
                .Verify<Task<HttpResponseMessage>>("SendAsync", Times.Once(), ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get
                    && x.RequestUri.ToString() == uri), ItExpr.IsAny<CancellationToken>());
        }

        private void SetupHttpMessageHandler(string uri)
        {
            _httpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get && x.RequestUri.ToString() == uri), 
                    ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));
        }

        private void SetupUriBuilder(string uri)
        {
            _uriBuilder.Setup(x => x.GetApodUri()).Returns(uri);
            _uriBuilder.Setup(x => x.GetApodUri(It.IsAny<DateTime>())).Returns(uri);
            _uriBuilder.Setup(x => x.GetApodUri(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(uri);
            _uriBuilder.Setup(x => x.GetApodUri(It.IsAny<int>())).Returns(uri);
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

        [Fact]
        public void Dispose_CanCallMultipleTimes()
        {
            _httpRequester.Dispose();
            _httpRequester.Dispose();
            _httpRequester.Dispose();
        }
    }
}
