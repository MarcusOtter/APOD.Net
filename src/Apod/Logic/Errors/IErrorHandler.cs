using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Logic.Errors
{
    public interface IErrorHandler
    {
        ApodError ValidateDate(DateTime dateTime);
        ApodError ValidateDateRange(DateTime startDate, DateTime endDate = default);
        Task<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse);
    }
}
