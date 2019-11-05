using System;
using System.Collections.Generic;
using System.Text;

namespace Apod
{
    public interface IErrorHandler
    {
        ApodResponse ValidateDate(DateTime dateTime);
        ApodResponse ValidateDateRange(DateTime startDate, DateTime endDate);
    }
}
