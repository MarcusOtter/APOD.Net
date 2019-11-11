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

            var errorBuilderMock = new Mock<IErrorBuilder>();
            var errorHandler = new ErrorHandler(errorBuilderMock.Object, firstValidDate, lastValidDate);

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
        }

        [Theory]
        [InlineData("2019-08-20", "2019-09-01")] // Random valid dates within default range
        [InlineData("2014-05-21", "2014-05-21")] // Same valid dates within default range
        public void ValidateDateRange_NoErrorOnValidDates(string startDate, string endDate = default)
        {
            var startInputDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture.DateTimeFormat);
            var endInputDate = DateTime.Parse(endDate, CultureInfo.InvariantCulture.DateTimeFormat);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = errorHandler.ValidateDateRange(startInputDate, endInputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }

        [Fact]
        public void ValidateDateRange_NoErrorOnDefaultEndDate()
        {
            var startInputDate = new DateTime(2007, 07, 10);

            var errorBuilderMock = new Mock<IErrorBuilder>();
            var errorHandler = new ErrorHandler(errorBuilderMock.Object);

            var expectedErrorCode = ApodErrorCode.None;
            var actualErrorCode = errorHandler.ValidateDateRange(startInputDate).ErrorCode;

            Assert.Equal(expectedErrorCode, actualErrorCode);
        }
    }
}
