using System;

namespace Apod
{
    public class ErrorBuilder : IErrorBuilder
    {
        private const string _dateFormat = "MMMM dd yyyy";
        private readonly DateTime _firstValidDate;
        private readonly DateTime _lastValidDate;

        public ErrorBuilder(DateTime firstValidDate, DateTime lastValidDate)
        {
            _firstValidDate = firstValidDate;
            _lastValidDate = lastValidDate;
        }

        public ApodResponse GetDateOutOfRangeError()
        {
            var errorMessage = $"Dates must be between {_firstValidDate.ToString(_dateFormat)} and {_lastValidDate.ToString(_dateFormat)}.";
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            var apodResponse = new ApodResponse(ApodStatusCode.Error, error: apodError);
            return apodResponse;
        }

        public ApodResponse GetStartDateAfterEndDateError()
        {
            var errorMessage = $"The start date cannot be after the end date.";
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            var apodResponse = new ApodResponse(ApodStatusCode.Error, error: apodError);
            return apodResponse;
        }
    }
}
