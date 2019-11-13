using System;

namespace Apod.Logic.Errors
{
    public class ErrorBuilder : IErrorBuilder
    {
        private readonly string _dateFormat;
        private readonly string _unknownErrorIssueUrl = "https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&labels=bug&template=unknown-error.md&title=Unknown+error";

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

        public ApodError GetBadRequestError(string errorMessage = "")
        {
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            return apodError;
        }

        public ApodError GetInternalServiceError(string errorMessage = "")
        {
            var apodError = new ApodError(ApodErrorCode.InternalServiceError, errorMessage);
            return apodError;
        }

        public ApodError GetUnknownError(string errorMessage = "")
        {
            var apodError = new ApodError(ApodErrorCode.Unknown, $"{errorMessage} Please open an issue at {_unknownErrorIssueUrl}.");
            return apodError;
        }
    }
}
