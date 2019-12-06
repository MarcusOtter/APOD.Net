using System;
using System.Threading.Tasks;

namespace Apod
{
    public interface IApodClient
    {
        /// <summary>Fetch the current Astronomy Picture of the Day.</summary>
        Task<ApodResponse> FetchApodAsync();
        ValueTask<ApodResponse> FetchApodAsync();

        /// <summary>Fetch the Astronomy Picture of the Day for a specific date.</summary>
        /// <param name="dateTime">The date to request the APOD for. Must be between June 16th 1995 and today's date.</param>
        ValueTask<ApodResponse> FetchApodAsync(DateTime dateTime);

        /// <summary>Fetch all the Astronomy Pictures of the Day between two dates.</summary>
        /// <param name="startDate">The start date. Must be between June 16th 1995 and today's date.</param>
        /// <param name="endDate">The end date. Must be between the <paramref name="startDate"/> and today's date. Defaults to today's date.</param>
        Task<ApodResponse> FetchApodAsync(DateTime startDate, DateTime endDate);

        /// <summary>Fetch an amount of random Astronomy Pictures of the Day.</summary>
        /// <param name="count">The amount of APODs to fetch. Must be positive and cannot exceed 100.</param>
        Task<ApodResponse> FetchApodAsync(int count);
    }
}
