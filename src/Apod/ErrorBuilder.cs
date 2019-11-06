using System;

namespace Apod
{
    public class ErrorBuilder : IErrorBuilder
    {
        private readonly string _dateFormat;

        public ErrorBuilder(string dateFormat = "MMMM dd yyyy")
        {
            _dateFormat = dateFormat;
        }

        public ApodError GetDateOutOfRangeError(DateTime firstValidDate, DateTime lastValidDate)
        {
            var errorMessage = $"Dates must be between {firstValidDate.ToString(_dateFormat)} and {lastValidDate.ToString(_dateFormat)}.";
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            return apodError;
        }

        public ApodError GetStartDateAfterEndDateError()
        {
            var errorMessage = $"The start date cannot be after the end date.";
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            return apodError;
        }
    }
}
