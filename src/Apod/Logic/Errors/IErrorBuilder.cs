using System;

namespace Apod.Logic.Errors
{
    public interface IErrorBuilder
    {
        ApodError GetDateOutOfRangeError(DateTime firstValidDate, DateTime lastValidDate);
        ApodError GetStartDateAfterEndDateError();
        ApodError GetTimeoutError();
    }
}
