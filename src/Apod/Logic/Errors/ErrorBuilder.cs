using System;

namespace Apod.Logic.Errors
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
            var apodError = new ApodError(ApodErrorCode.DateOutOfRange, errorMessage);
            return apodError;
        }

        public ApodError GetStartDateAfterEndDateError()
        {
            var errorMessage = "The start date cannot be after the end date.";
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            return apodError;
        }

        public ApodError GetTimeoutError()
        {
            var errorMessage = "The API timed out.";
            var apodError = new ApodError(ApodErrorCode.Timeout, errorMessage);
            return apodError;
        }

        public ApodError GetCountOutOfRangeError()
        {
            var errorMessage = "The count must be positive and cannot exceed 100.";
            var apodError = new ApodError(ApodErrorCode.CountOutOfRange, errorMessage);
            return apodError;
        }
    }
}
