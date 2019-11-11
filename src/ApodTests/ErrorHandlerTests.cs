using System;
using Xunit;
using Apod.Logic.Errors;
using Moq;
using Apod;
using System.Globalization;

namespace ApodTests
{
    public class ErrorHandlerTests
    {
        [Theory]
        [InlineData("1993-08-11")]
        [InlineData("2005-01-05")]
        [InlineData("2006-01-04")]
        [InlineData("1803-06-21")]
        public void ValidateDate_CorrectErrorOnInvalidDate(string invalidDate)
        {
            var inputDate = DateTime.Parse(invalidDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var firstValidDate = new DateTime(1993, 08, 12);
            var lastValidDate = new DateTime(2005, 01, 04);

            var expectedError = new ApodError(ApodErrorCode.BadRequest, $"Dates must be between {firstValidDate} and {lastValidDate}.");

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetDateOutOfRangeError(firstValidDate, lastValidDate))
                .Returns(expectedError);

            var errorHandler = new ErrorHandler(errorBuilderMock.Object, firstValidDate, lastValidDate);

            var actual = errorHandler.ValidateDate(inputDate);

            Assert.Equal(expectedError.ErrorCode, actual.ErrorCode);
        }

        [Theory]
        [InlineData("2003-02-26")]
        [InlineData("2010-10-25")]
        [InlineData("2004-01-01")]
        [InlineData("2009-12-10")]
        public void ValidateDate_NoErrorOnValidDate(string validDate)
        {
            var inputDate = DateTime.Parse(validDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var firstValidDate = new DateTime(2003, 02, 26);
            var lastValidDate = new DateTime(2010, 10, 25);

            var expectedError = new ApodError(ApodErrorCode.None);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            errorBuilderMock
                .Setup(x => x.GetDateOutOfRangeError(firstValidDate, lastValidDate))
                .Returns(expectedError);

            var errorHandler = new ErrorHandler(errorBuilderMock.Object, firstValidDate, lastValidDate);

            var actual = errorHandler.ValidateDate(inputDate);

            Assert.Equal(expectedError.ErrorCode, actual.ErrorCode);
        }
    }
}
