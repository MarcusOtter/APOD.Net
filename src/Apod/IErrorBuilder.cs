using System;

namespace Apod
{
    public interface IErrorBuilder
    {
        ApodError GetDateOutOfRangeError(DateTime firstValidDate, DateTime lastValidDate);
        ApodError GetStartDateAfterEndDateError();
    }
}
