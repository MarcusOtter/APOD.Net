using System;

namespace Apod
{
    public interface IErrorBuilder
    {
        ApodResponse GetDateOutOfRangeError(DateTime firstValidDate, DateTime lastValidDate);
        ApodResponse GetStartDateAfterEndDateError();
    }
}
