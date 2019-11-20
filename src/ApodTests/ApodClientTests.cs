using Apod;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Apod.Logic.Net;
using Apod.Logic.Errors;
using System.Net.Http;
using System.Net;
using Apod.Logic.Net.Dtos;

namespace ApodTests
{
    public class ApodClientTests : IDisposable
    {
        private const string _apiKey = "DEMO_KEY";

        private readonly Mock<IHttpRequester> _httpRequester;
        private readonly Mock<IHttpResponseParser> _httpResponseParser;
        private readonly Mock<IErrorHandler> _errorHandler;

        private readonly IApodClient _client;

        // Data used in tests
        private readonly HttpResponseMessage _errorResponseMessageExample;
        private readonly HttpResponseMessage _successResponseMessageExample;

        private readonly ApodResponse _errorApodResponseExample;
        private readonly ApodResponse _successApodResponseExample;

        public ApodClientTests()
        {
            // Initialize mocks
            _httpRequester = new Mock<IHttpRequester>();
            _httpResponseParser = new Mock<IHttpResponseParser>();
            _errorHandler = new Mock<IErrorHandler>();

            // Setup testing data
            _errorResponseMessageExample = GetErrorResponseMessageExample();
            _errorApodResponseExample = GetErrorApodResponseExample();
            _successResponseMessageExample = GetSuccessResponseMessageExample();
            _successApodResponseExample = GetSuccessApodResponseExample();

            // Initialize client
            _client = new ApodClient(_apiKey, _httpRequester.Object, _httpResponseParser.Object, _errorHandler.Object);
        }


        [Fact]
        public async Task FetchApodAsync_Today_CorrectResult()
        {
            HttpResponseIsValid(true);
            ParseSingleApodAsyncReturns(_successApodResponseExample);

            var expected = _successApodResponseExample;
            var actual = await _client.FetchApodAsync();

            Assert.Equal(expected.StatusCode, actual.StatusCode);
            Assert.Equal(expected.Content, actual.Content);
        }

        [Fact]
        public async Task FetchApodAsync_Date_CorrectResult()
        {
            var date = new DateTime(2002, 06, 25);

            HttpResponseIsValid(true);
            ValidateDateReturns(ApodErrorCode.None); // date is valid (true)
            ParseSingleApodAsyncReturns(_successApodResponseExample);

            var expected = _successApodResponseExample;
            var actual = await _client.FetchApodAsync(date);

            Assert.Equal(expected.StatusCode, actual.StatusCode);
            Assert.Equal(expected.Content, actual.Content);
        }

        [Fact]
        public async Task FetchApodAsync_Date_SendHttpRequestAsyncWasCalled()
        {
            var date = new DateTime(1999, 12, 10);
            HttpResponseIsValid(true);
            ValidateDateReturns(ApodErrorCode.None);

            await _client.FetchApodAsync(date);

            _httpRequester.Verify(x => x.SendHttpRequestAsync(date), Times.Once);
        }

        [Fact]
        public async Task FetchApodAsync_DateRange_SendHttpRequestAsyncWasCalled()
        {
            var startDate = new DateTime(2001, 11, 08);
            var endDate = new DateTime(2002, 01, 02);

            HttpResponseIsValid(true);
            ValidateDateRangeReturns(ApodErrorCode.None);

            await _client.FetchApodAsync(startDate, endDate);

            _httpRequester.Verify(x => x.SendHttpRequestAsync(startDate, endDate), Times.Once);
        }

        [Fact]
        public async Task FetchApodAsync_Count_SendHttpRequestAsyncWasCalled()
        {
            var count = 5;

            HttpResponseIsValid(true);
            ValidateCountReturns(ApodErrorCode.None);

            await _client.FetchApodAsync(count);

            _httpRequester.Verify(x => x.SendHttpRequestAsync(count), Times.Once);
        }

        private void HttpResponseIsValid(bool responseIsValid)
        {
            var errorCode = responseIsValid ? ApodErrorCode.None : ApodErrorCode.BadRequest;
            var error = new ApodError(errorCode);

            _errorHandler
                .Setup(x => x.ValidateHttpResponseAsync(It.IsAny<HttpResponseMessage>()))
                .ReturnsAsync(() => error);
        }

        private void ValidateDateReturns(ApodErrorCode errorCode)
        {
            var error = new ApodError(errorCode);

            _errorHandler
                .Setup(x => x.ValidateDate(It.IsAny<DateTime>()))
                .Returns(error);
        }

        private void ValidateDateRangeReturns(ApodErrorCode errorCode)
        {
            var error = new ApodError(errorCode);

            _errorHandler
                .Setup(x => x.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(error);
        }

        private void ValidateCountReturns(ApodErrorCode errorCode)
        {
            var error = new ApodError(errorCode);

            _errorHandler
                .Setup(x => x.ValidateCount(It.IsAny<int>()))
                .Returns(error);
        }

        private void ParseSingleApodAsyncReturns(ApodResponse response)
        {
            _httpResponseParser
                .Setup(x => x.ParseSingleApodAsync(It.IsAny<HttpResponseMessage>()))
                .ReturnsAsync(() => response);
        }

        [Fact]
        public void Constructor_NoArguments_NotNull()
        {
            var result = new ApodClient();
            Assert.NotNull(result);
        }

        [Fact]
        public void Contructor_WithApiKey_NotNull()
        {
            var result = new ApodClient(_apiKey);
            Assert.NotNull(result);
        }

        [Fact]
        public void Constructor_WithAllArguments_NotNull()
        {
            var result = new ApodClient(_apiKey, _httpRequester.Object, _httpResponseParser.Object, _errorHandler.Object);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task FetchApodAsync_Today_ThrowsIfDisposed()
        {
            _client.Dispose();
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _client.FetchApodAsync());
        }

        [Fact]
        public async Task FetchApodAsync_Date_ThrowsIfDisposed()
        {
            var date = new DateTime(2015, 02, 16);

            _client.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _client.FetchApodAsync(date));
        }

        [Fact]
        public async Task FetchApodAsync_DateRange_ThrowsIfDisposed()
        {
            var startDate = new DateTime(2013, 10, 10);
            var endDate = new DateTime(2013, 10, 20);

            _client.Dispose();
            
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _client.FetchApodAsync(startDate, endDate));
        }

        [Fact]
        public async Task FetchApodAsync_Count_ThrowsIfDisposed()
        {
            var count = 14;

            _client.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await _client.FetchApodAsync(count));
        }

        [Fact]
        public void Dispose_CanCallMultipleTimes()
        {
            _client.Dispose();
            _client.Dispose();
            _client.Dispose();
        }

        private HttpResponseMessage GetErrorResponseMessageExample()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(@"{""code"":400,""msg"":""Bad Request: incorrect field passed. Allowed request fields for apod method are 'concept_tags', 'date', 'hd', 'count', 'start_date', 'end_date'"",""service_version"":""v1""}")
            };
        }

        private ApodResponse GetErrorApodResponseExample()
        {
            var error = new ApodError(ApodErrorCode.BadRequest, "Bad Request: incorrect field passed. Allowed request fields for apod method are 'concept_tags', 'date', 'hd', 'count', 'start_date', 'end_date'");
            return new ApodResponse(ApodStatusCode.Error, error: error);
        }

        private HttpResponseMessage GetSuccessResponseMessageExample()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""copyright"":""R Jay Gabany"",""date"":""2019-11-16"",""explanation"":""Grand tidal streams of stars seem to surround galaxy NGC 5907. The arcing structures form tenuous loops extending more than 150,000 light-years from the narrow, edge-on spiral, also known as the Splinter or Knife Edge Galaxy.Recorded only in very deep exposures, the streams likely represent the ghostly trail of a dwarf galaxy - debris left along the orbit of a smaller satellite galaxy that was gradually torn apart and merged with NGC 5907 over four billion years ago.Ultimately this remarkable discovery image, from a small robotic observatory in New Mexico, supports the cosmological scenario in which large spiral galaxies, including our own Milky Way, were formed by the accretion of smaller ones. NGC 5907 lies about 40 million light-years distant in the northern constellation Draco."",""hdurl"":""https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl.jpg"",""media_type"":""image"",""service_version"":""v1"",""title"":""The Star Streams of NGC 5907"",""url"":""https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl1024.jpg""}")
            };
        }

        private ApodResponse GetSuccessApodResponseExample()
        {
            var content = new ApodContent()
            {
                Copyright = "R Jay Gabany",
                Date = new DateTime(2019, 11, 16),
                Explanation = "Grand tidal streams of stars seem to surround galaxy NGC 5907. The arcing structures form tenuous loops extending more than 150,000 light-years from the narrow, edge-on spiral, also known as the Splinter or Knife Edge Galaxy.Recorded only in very deep exposures, the streams likely represent the ghostly trail of a dwarf galaxy - debris left along the orbit of a smaller satellite galaxy that was gradually torn apart and merged with NGC 5907 over four billion years ago.Ultimately this remarkable discovery image, from a small robotic observatory in New Mexico, supports the cosmological scenario in which large spiral galaxies, including our own Milky Way, were formed by the accretion of smaller ones. NGC 5907 lies about 40 million light-years distant in the northern constellation Draco.",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "The Star Streams of NGC 5907",
                ContentUrl = "https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl1024.jpg"
            };

            var allContent = new ApodContent[] { content };

            return new ApodResponse(ApodStatusCode.OK, allContent);
        }

        public void Dispose()
        {
            _client.Dispose();
            _errorResponseMessageExample.Dispose();
            _successResponseMessageExample.Dispose();
        }
    }
}
