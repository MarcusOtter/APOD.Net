using System;
using Xunit;
using Apod.Logic.Errors;
using Apod;

namespace ApodTests
{
    public class ErrorBuilderTests
    {
        [Theory]
        [InlineData("MMMM dd, yyyy", "April 21, 1992", "January 02, 2049")]
        [InlineData("dd/MM-yyyy", "21/04-1992", "02/01-2049")]
        [InlineData("ddMMyyyy", "21041992", "02012049")]
        public void GetDateOutOfRangeError_CorrectCustomDateFormat(string dateFormat, string expectedFirstValidDate, string expectedLastValidDate)
        {
            var firstValidDate = new DateTime(1992, 04, 21);
            var lastValidDate = new DateTime(2049, 01, 02);
            var errorBuilder = new ErrorBuilder(dateFormat);

            var actual = errorBuilder.GetDateOutOfRangeError(firstValidDate, lastValidDate);
            
            Assert.Contains(expectedFirstValidDate, actual.ErrorMessage);
            Assert.Contains(expectedLastValidDate, actual.ErrorMessage);
        }

        [Fact]
        public void GetDateOutOfRangeError_CorrectDefaultDateFormat()
        {
            var firstValidDate = new DateTime(2002, 02, 22);
            var lastValidDate = new DateTime(2073, 10, 05);
            var errorBuilder = new ErrorBuilder();

            var expectedFirstValidDate = "February 22 2002";
            var expectedLastValidDate = "October 05 2073";

            var actual = errorBuilder.GetDateOutOfRangeError(firstValidDate, lastValidDate);

            Assert.Contains(expectedFirstValidDate, actual.ErrorMessage);
            Assert.Contains(expectedLastValidDate, actual.ErrorMessage);
        }

        [Fact]
        public void GetDateOutOfRangeError_CorrectErrorCode()
        {
            var firstValidDate = new DateTime(1997, 03, 12);
            var lastValidDate = new DateTime(2005, 07, 01);
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.DateOutOfRange;
            var actualErrorCode = errorBuilder.GetDateOutOfRangeError(firstValidDate, lastValidDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public void GetStartDateAfterEndDateError_CorrectErrorCode()
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.BadRequest;
            var actualErrorCode = errorBuilder.GetStartDateAfterEndDateError().ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public void GetTimeoutError_CorrectErrorCode()
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.Timeout;
            var actualErrorCode = errorBuilder.GetTimeoutError().ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public void GetCountOutOfrangeError_CorrectErrorCode()
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.CountOutOfRange;
            var actualErrorCode = errorBuilder.GetCountOutOfRangeError().ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public void GetBadRequestError_CorrectErrorCode()
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.BadRequest;
            var actualErrorCode = errorBuilder.GetBadRequestError().ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Theory]
        [InlineData("Bad Request: incorrect field passed. Allowed request fields for apod method are 'concept_tags', 'date', 'hd', 'count', 'start_date', 'end_date'")]
        [InlineData("")]
        public void GetBadRequestError_ContainsErrorMessage(string errorMessage)
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorMessage = errorMessage;
            var actualErrorMessage = errorBuilder.GetBadRequestError(errorMessage).ErrorMessage;

            Assert.Contains(expectedErrorMessage, actualErrorMessage);
        }

        [Fact]
        public void GetInternalServiceError_CorrectErrorCode()
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.InternalServiceError;
            var actualErrorCode = errorBuilder.GetInternalServiceError().ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Theory]
        [InlineData("Internal Service Error")]
        [InlineData("")]
        public void GetInternalServiceError_ContainsErrorMessage(string errorMessage)
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorMessage = errorMessage;
            var actualErrorMessage = errorBuilder.GetInternalServiceError(errorMessage).ErrorMessage;

            Assert.Contains(expectedErrorMessage, actualErrorMessage);
        }

        [Fact]
        public void GetUnknownError_CorrectErrorCode()
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorCode = ApodErrorCode.Unknown;
            var actualErrorCode = errorBuilder.GetUnknownError().ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Theory]
        [InlineData("Something unexpected happened somewhere...")]
        [InlineData("")]
        public void GetUnknownError_ContainsErrorMessage(string errorMessage)
        {
            var errorBuilder = new ErrorBuilder();

            var expectedErrorMessage = errorMessage;
            var actualErrorMessage = errorBuilder.GetUnknownError(errorMessage).ErrorMessage;

            Assert.Contains(expectedErrorMessage, actualErrorMessage);
        }
    }
}
