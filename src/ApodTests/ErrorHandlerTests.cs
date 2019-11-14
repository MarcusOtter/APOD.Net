using System;
using Xunit;
using Apod.Logic.Errors;
using Moq;
using Apod;
using System.Globalization;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Text;

namespace ApodTests
{
    public class ErrorHandlerTests
    {
        [Theory]
        [InlineData("1993-08-11")] // 1 Day before first valid date
        [InlineData("2005-01-05")] // 1 Day after last valid date
        [InlineData("2006-01-04")] // Random invalid date
        [InlineData("1803-06-21")] // Random invalid date
        public void ValidateDate_CorrectErrorOnInvalidDate(string invalidDate)
        {
            var inputDate = DateTime.Parse(invalidDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var firstValidDate = new DateTime(1993, 08, 12);
            var lastValidDate = new DateTime(2005, 01, 04);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetDateOutOfRangeError(firstValidDate, lastValidDate))
                .Returns(new ApodError(ApodErrorCode.DateOutOfRange));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object, firstValidDate, lastValidDate);

            var expectedErrorCode = ApodErrorCode.DateOutOfRange;
            var actualErrorCode = errorHandler.ValidateDate(inputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetDateOutOfRangeError(firstValidDate, lastValidDate), Times.Once);
        }

        [Theory]
        [InlineData("2003-02-26")] // First valid date
        [InlineData("2010-10-25")] // Last valid date
        [InlineData("2004-01-01")] // Random valid date
        [InlineData("2009-12-10")] // Random valid date
        public void ValidateDate_NoErrorOnValidDate(string validDate)
        {
            var inputDate = DateTime.Parse(validDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var firstValidDate = new DateTime(2003, 02, 26);
            var lastValidDate = new DateTime(2010, 10, 25);

            var errorHandler = new ErrorHandler(null, firstValidDate, lastValidDate);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = errorHandler.ValidateDate(inputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Theory]
        [InlineData("1969-05-20", "1969-01-02")] // Start date 1 day before first valid date
        [InlineData("1989-11-01", "1989-11-08")] // End date 1 day after last valid date
        [InlineData("1969-02-03", "1990-04-01")] // Both dates out of range
        public void ValidateDateRange_CorrectErrorOnDateOutOfRange(string startDate, string endDate)
        {
            var startInputDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture.DateTimeFormat);
            var endInputDate = DateTime.Parse(endDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var firstValidDate = new DateTime(1969, 05, 21);
            var lastValidDate = new DateTime(1989, 11, 07);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetDateOutOfRangeError(firstValidDate, lastValidDate))
                .Returns(new ApodError(ApodErrorCode.DateOutOfRange));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object, firstValidDate, lastValidDate);

            var expectedErrorCode = ApodErrorCode.DateOutOfRange;
            var actualErrorCode = errorHandler.ValidateDateRange(startInputDate, endInputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetDateOutOfRangeError(firstValidDate, lastValidDate), Times.Once);
        }

        [Theory]
        [InlineData("2000-04-04", "2000-04-03")] // Start date 1 day after end date
        [InlineData("2000-08-01", "2000-01-12")] // Start date many days after end date
        public void ValidateDateRange_CorrectErrorOnStartDateAfterEndDate(string startDate, string endDate)
        {
            var startInputDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture.DateTimeFormat);
            var endInputDate = DateTime.Parse(endDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var firstValidDate = new DateTime(2000, 01, 01);
            var lastValidDate = new DateTime(2000, 12, 31);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetStartDateAfterEndDateError())
                .Returns(new ApodError(ApodErrorCode.StartDateAfterEndDate));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object, firstValidDate, lastValidDate);

            var expectedErrorCode = ApodErrorCode.StartDateAfterEndDate;
            var actualErrorCode = errorHandler.ValidateDateRange(startInputDate, endInputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetStartDateAfterEndDateError(), Times.Once);
        }

        [Theory]
        [InlineData("2019-08-20", "2019-09-01")] // Random valid dates within default range
        [InlineData("2014-05-21", "2014-05-21")] // Same valid dates within default range
        public void ValidateDateRange_NoErrorOnValidDates(string startDate, string endDate = default)
        {
            var startInputDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture.DateTimeFormat);
            var endInputDate = DateTime.Parse(endDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var errorHandler = new ErrorHandler(null);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = errorHandler.ValidateDateRange(startInputDate, endInputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public void ValidateDateRange_NoErrorOnDefaultEndDate()
        {
            var startInputDate = new DateTime(2007, 07, 10);

            var errorHandler = new ErrorHandler(null);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = errorHandler.ValidateDateRange(startInputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Theory]
        [InlineData(1)] // First valid number
        [InlineData(100)] // Last valid number
        [InlineData(37)] // Random valid number
        public void ValidateCount_NoErrorOnValidCount(int count)
        {
            var errorHandler = new ErrorHandler(null);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = errorHandler.ValidateCount(count).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Theory]
        [InlineData(0)] // First valid number minus 1
        [InlineData(101)] // Last valid number plus 1
        [InlineData(-34)] // Random invalid negative number
        [InlineData(145)] // Random invalid positive number
        public void ValidateCount_CorrectErrorOnInvalidCount(int count)
        {
            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetCountOutOfRangeError())
                .Returns(new ApodError(ApodErrorCode.CountOutOfRange));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.CountOutOfRange;
            var actualErrorCode = errorHandler.ValidateCount(count).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public async Task ValidateHttpResponseAsync_NoErrorOnValidHttpResponse()
        {
            var errorHandler = new ErrorHandler(null);
            using var response = new HttpResponseMessage(HttpStatusCode.OK);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = (await errorHandler.ValidateHttpResponseAsync(response)).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public async Task ValidateHttpResponseAsync_CorrectErrorOnTimeoutHttpResponse()
        {
            using var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(GetTimeoutContent(), Encoding.UTF8, "text/html")
            };

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetTimeoutError())
                .Returns(new ApodError(ApodErrorCode.Timeout));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.Timeout;
            var actualErrorCode = (await errorHandler.ValidateHttpResponseAsync(response)).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetTimeoutError(), Times.Once);
        }

        [Theory]
        [InlineData(@"{""code"":400,""msg"":""unconverted data remains: https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""invalid literal for int() with base 10: '3https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY'"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""Bad Request: incorrect field passed. Allowed request fields for apod method are 'concept_tags', 'date', 'hd', 'count', 'start_date', 'end_date'"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""Bad Request: invalid field combination passed. Allowed request fields for apod method are 'concept_tags', 'date', 'hd', 'count', 'start_date', 'end_date'"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""time data 'test' does not match format '%Y-%m-%d'"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""Date must be between Jun 16, 1995 and Oct 30, 2019."",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""Count must be positive and cannot exceed 100"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":400,""msg"":""start_date cannot be after end_date"",""service_version"":""v1""}")]
        public async Task ValidateHttpResponseAsync_CorrectErrorOnBadRequestHttpResponse(string errorResponseContent)
        {
            using var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(errorResponseContent, Encoding.UTF8, "application/json")
            };

            // The value of the "msg" parameter.
            var errorMessage = errorResponseContent.Remove(errorResponseContent.Length - 25).Remove(0, 19);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetBadRequestError(errorMessage))
                .Returns(new ApodError(ApodErrorCode.BadRequest));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.BadRequest;
            var actualErrorCode = (await errorHandler.ValidateHttpResponseAsync(response)).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetBadRequestError(errorMessage), Times.Once);
        }

        [Theory]
        [InlineData(@"{""code"":500,""msg"":""Internal Service Error"",""service_version"":""v1""}")]
        [InlineData(@"{""code"":500,""msg"":""Sorry, unexpected error: must be str, not NotFound"",""service_version"":""v1""}")]
        public async Task ValidateHttpResponseAsync_CorrectErrorOnInternalServiceErrorHttpResponse(string errorResponseContent)
        {
            using var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(errorResponseContent, Encoding.UTF8, "application/json")
            };

            // The value of the "msg" parameter.
            var errorMessage = errorResponseContent.Remove(errorResponseContent.Length - 25).Remove(0, 19);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetInternalServiceError(errorMessage))
                .Returns(new ApodError(ApodErrorCode.InternalServiceError));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.InternalServiceError;
            var actualErrorCode = (await errorHandler.ValidateHttpResponseAsync(response)).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetInternalServiceError(errorMessage), Times.Once);
        }

        [Theory]
        [InlineData(@"{""code"":401,""msg"":""Undiscovered with unauthorized status code"",""service_version"":""v1""}")]
        public async Task ValidateHttpResponseAsync_CorrectErrorOnUnknownErrorHttpResponse(string errorResponseContent)
        {
            using var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent(errorResponseContent, Encoding.UTF8, "application/json")
            };

            // The value of the "msg" parameter.
            var errorMessage = errorResponseContent.Remove(errorResponseContent.Length - 25).Remove(0, 19);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetUnknownError(errorMessage))
                .Returns(new ApodError(ApodErrorCode.Unknown));

            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.Unknown;
            var actualErrorCode = (await errorHandler.ValidateHttpResponseAsync(response)).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
            errorBuilderMock.Verify(x => x.GetUnknownError(errorMessage), Times.Once);
        }

        // An exact copy of what would be returned when the api times out, incorrect indentations included.
        private string GetTimeoutContent() => @"
<!DOCTYPE html>
        <html>
          <head>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"" >
                <meta charset=""utf-8"" >
                <title>Application Error</title>
                <style media=""screen"" >
                  html,body,iframe {
                        margin: 0;
                        padding: 0;
                  }
                  html,body {
                        height: 100%;
                        overflow: hidden;
                  }
                  iframe {
                        width: 100%;
                        height: 100%;
                        border: 0;
                  }
                </style>
          </head>
          <body>
                <iframe src=""//www.herokucdn.com/error-pages/application-error.html"" ></iframe>
          </body>
        </html>";
    }
}
