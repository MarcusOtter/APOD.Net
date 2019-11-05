using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod
{
    public interface IErrorHandler
    {
        ApodResponse ValidateDate(DateTime dateTime);
        ApodResponse ValidateDateRange(DateTime startDate, DateTime endDate);
        Task<ApodResponse> ValidateHttpResponseAsync(HttpResponseMessage httpResponse);
    }
}
