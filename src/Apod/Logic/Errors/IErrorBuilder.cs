using System;

namespace Apod.Logic.Errors
{
    public interface IErrorBuilder
    {
        ApodError GetDateOutOfRangeError(DateTime firstValidDate, DateTime lastValidDate);
        ApodError GetStartDateAfterEndDateError();
        ApodError GetTimeoutError();
        ApodError GetCountOutOfRangeError();
        ApodError GetBadRequestError(string errorMessage = "");
        ApodError GetInternalServiceError(string errorMessage = "");
        ApodError GetUnknownError(string errorMessage = "");
    }
}
