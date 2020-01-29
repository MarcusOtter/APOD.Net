using Apod;
using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Apod.Logic.Net;
using Apod.Logic.Errors;
using System.Net.Http;

namespace ApodTests
{
    public class ApodClientTests : IDisposable
    {
        private const string _apiKey = "DEMO_KEY";

        private readonly Mock<IHttpRequester> _httpRequester;
        private readonly Mock<IHttpResponseParser> _httpResponseParser;
        private readonly Mock<IErrorHandler> _errorHandler;

        private readonly ApodClient _client;

        public ApodClientTests()
        {
            // Initialize mocks
            _httpRequester = new Mock<IHttpRequester>();
            _httpResponseParser = new Mock<IHttpResponseParser>();
            _errorHandler = new Mock<IErrorHandler>();

            // Initialize client
            _client = new ApodClient(_apiKey, _httpRequester.Object, _httpResponseParser.Object, _errorHandler.Object);
        }

        [Fact]
        public async Task FetchApodAsync_Today_DoesNotValidateInput() 
        {
            InputHasError(ApodErrorCode.None);
            HttpResponseHasError(ApodErrorCode.None);

            await _client.FetchApodAsync();

            // The method doesn't have any input, so nothing should be validated.
            AssertInputWasNotValidated();
        }

        [Fact]
        public async Task FetchApodAsync_Date_TodayDoesNotValidateInput()
        {
            var date = DateTime.Now;
            InputHasError(ApodErrorCode.None);
            HttpResponseHasError(ApodErrorCode.None);

            await _client.FetchApodAsync(date);

            // The method has input, but the date of the input is DateTime.Today 
            // which is always valid so it should not be validated.
            AssertInputWasNotValidated();
        }

        [Fact]
        public async Task FetchApodAsync_DateRange_SetsEndDateToDefaultIfToday()
        {
            var startDate = DateTime.Today.AddDays(-5);
            var endDate = DateTime.Today;

            InputHasError(ApodErrorCode.None);
            HttpResponseHasError(ApodErrorCode.None);

            await _client.FetchApodAsync(startDate, endDate);

            AssertEndDateIsDefault();
        }

        [Fact]
        public async Task FetchApodAsync_Date_DoesNotSendHttpRequestOnInputError()
        {
            var date = default(DateTime);
            InputHasError(ApodErrorCode.DateOutOfRange);
            HttpResponseHasError(ApodErrorCode.None);

            await _client.FetchApodAsync(date);

            AssertHttpRequestNotSent();
        }

        [Fact]
        public async Task FetchApodAsync_DateRange_DoesNotSendHttpRequestOnInputError()
        {
            var startDate = default(DateTime);
            var endDate = default(DateTime);
            InputHasError(ApodErrorCode.DateOutOfRange);
            HttpResponseHasError(ApodErrorCode.None);

            await _client.FetchApodAsync(startDate, endDate);

            AssertHttpRequestNotSent();
        }

        [Fact]
        public async Task FetchApodAsync_Count_DoesNotSendHttpRequestOnInputError()
        {
            var count = default(int);
            InputHasError(ApodErrorCode.CountOutOfRange);
            HttpResponseHasError(ApodErrorCode.None);

            await _client.FetchApodAsync(count);

            AssertHttpRequestNotSent();
        }

        //          Input error                   HttpResponse error           Expected status code
        [Theory]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.None,          ApodStatusCode.OK)]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.OverRateLimit, ApodStatusCode.Error)]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.ApiKeyInvalid, ApodStatusCode.Error)]
        public async Task FetchApodAsync_Today_CorrectApodStatusCode(ApodErrorCode inputError, ApodErrorCode httpResponseError, ApodStatusCode expectedStatusCode)
        {
            InputHasError(inputError);
            HttpResponseHasError(httpResponseError);

            var actualStatusCode = (await _client.FetchApodAsync()).StatusCode;

            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        //          Input error                   HttpResponse error           Expected status code
        [Theory]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.None,          ApodStatusCode.OK)]
        [InlineData(ApodErrorCode.DateOutOfRange, ApodErrorCode.None,          ApodStatusCode.Error)]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.ApiKeyInvalid, ApodStatusCode.Error)]
        public async Task FetchApodAsync_Date_CorrectApodStatusCode(ApodErrorCode inputError, ApodErrorCode httpResponseError, ApodStatusCode expectedStatusCode)
        {
            var date = default(DateTime);
            InputHasError(inputError);
            HttpResponseHasError(httpResponseError);

            var actualStatusCode = (await _client.FetchApodAsync(date)).StatusCode;

            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        //          Input error                   HttpResponse error           Expected status code
        [Theory]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.None,          ApodStatusCode.OK)]
        [InlineData(ApodErrorCode.DateOutOfRange, ApodErrorCode.None,          ApodStatusCode.Error)]
        [InlineData(ApodErrorCode.None,           ApodErrorCode.ApiKeyInvalid, ApodStatusCode.Error)]
        public async Task FetchApodAsync_DateRange_CorrectApodStatusCode(ApodErrorCode inputError, ApodErrorCode httpResponseError, ApodStatusCode expectedStatusCode)
        {
            var startDate = default(DateTime);
            var endDate = default(DateTime);
            InputHasError(inputError);
            HttpResponseHasError(httpResponseError);

            var actualStatusCode = (await _client.FetchApodAsync(startDate, endDate)).StatusCode;

            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        //          Input error                    HttpResponse error           Expected status code
        [Theory]
        [InlineData(ApodErrorCode.None,            ApodErrorCode.None,          ApodStatusCode.OK)]
        [InlineData(ApodErrorCode.CountOutOfRange, ApodErrorCode.None,          ApodStatusCode.Error)]
        [InlineData(ApodErrorCode.None,            ApodErrorCode.ApiKeyInvalid, ApodStatusCode.Error)]
        public async Task FetchApodAsync_Count_CorrectApodStatusCode(ApodErrorCode inputError, ApodErrorCode httpResponseError, ApodStatusCode expectedStatusCode)
        {
            var count = default(int);
            InputHasError(inputError);
            HttpResponseHasError(httpResponseError);

            var actualStatusCode = (await _client.FetchApodAsync(count)).StatusCode;

            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        private void AssertInputWasNotValidated()
        {
            _errorHandler.Verify(x => x.ValidateDate(It.IsAny<DateTime>()), Times.Never);
            _errorHandler.Verify(x => x.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never);
            _errorHandler.Verify(x => x.ValidateCount(It.IsAny<int>()), Times.Never);
        }

        private void AssertEndDateIsDefault()
        {
            _errorHandler.Verify(x => x.ValidateDateRange(It.IsAny<DateTime>(), It.Is<DateTime>(x => x == default)));
        }

        private void AssertHttpRequestNotSent()
        {
            _httpRequester.VerifyNoOtherCalls();
        }

        private void InputHasError(ApodErrorCode errorCode)
        {
            var error = new ApodError(errorCode);
            _errorHandler.Setup(x => x.ValidateCount(It.IsAny<int>())).Returns(error);
            _errorHandler.Setup(x => x.ValidateDate(It.IsAny<DateTime>())).Returns(error);
            _errorHandler.Setup(x => x.ValidateDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(error);
        }

        private void HttpResponseHasError(ApodErrorCode errorCode)
        {
            var error = new ApodError(errorCode);
            _errorHandler
                .Setup(x => x.ValidateHttpResponseAsync(It.IsAny<HttpResponseMessage>()))
                .ReturnsAsync(() => error);

            if (errorCode is ApodErrorCode.None)
            {
                var apodResponse = new ApodResponse(ApodStatusCode.OK);

                _httpResponseParser
                    .Setup(x => x.ParseSingleApodAsync(It.IsAny<HttpResponseMessage>()))
                    .ReturnsAsync(() => apodResponse);

                _httpResponseParser
                    .Setup(x => x.ParseMultipleApodsAsync(It.IsAny<HttpResponseMessage>()))
                    .ReturnsAsync(() => apodResponse);
            }
        }

        [Theory]
        [InlineData("2001-04-10", "https://apod.nasa.gov/apod/ap010410.html")]
        [InlineData("1965-11-28", "https://apod.nasa.gov/apod/ap651128.html")]
        [InlineData("2038-08-02", "https://apod.nasa.gov/apod/ap380802.html")]
        public void GetPermalink_CorrectLink(string dateString, string expected)
        {
            var date = DateTime.Parse(dateString);
            var apod = GetApodContentWithDate(date);

            var actual = _client.GetPermalink(apod);

            Assert.Equal(expected, actual);
        }

        private ApodContent GetApodContentWithDate(DateTime date)
        {
            return new ApodContent { Date = date };
        }

        [Fact]
        public void Constructor_NoArguments_NotNull()
        {
            // Pretty unecessary test, but there's not really
            // a way for me to validate that the empty ctor "works"
            var result = new ApodClient();
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

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
