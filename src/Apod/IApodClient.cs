using System;
using System.Threading.Tasks;

namespace Apod
{
    public interface IApodClient
    {
        Task<ApodResponse> FetchApodAsync();
        Task<ApodResponse> FetchApodAsync(DateTime dateTime);
        Task<ApodResponse> FetchApodAsync(DateTime startDate, DateTime endDate);
        Task<ApodResponse> FetchApodAsync(int count);
    }
}
