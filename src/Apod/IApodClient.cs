using System;
using System.Threading.Tasks;

namespace Apod
{
    /// <summary>
    /// An interface for a client that interfaces with NASA's Astronomy Picture of the Day API.
    /// </summary>
    /// <remarks>
    /// <para>See <see cref="ApodClient"/> for the default implementation.</para>
    /// <para>
    /// NASA's Astronomy Picture of the Day API can be found at <a href="https://api.nasa.gov#apod">https://api.nasa.gov#apod</a>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ApodClient"/>
    public interface IApodClient
    {
        /// <summary>
        /// Fetch the Astronomy Picture of the Day for today's date.
        /// </summary>
        Task<ApodResponse> FetchApodAsync();

        /// <summary>
        /// Fetch the Astronomy Picture of the Day for a specific date.
        /// </summary>
        /// <param name="dateTime">The date to request the APOD for. Must be between June 16th 1995 and today's date (inclusive).</param>
        Task<ApodResponse> FetchApodAsync(DateTime dateTime);

        /// <summary>
        /// Fetch all the Astronomy Pictures of the Day between two dates (inclusive).
        /// </summary>
        /// <param name="startDate">The start date. Must be between June 16th 1995 and today's date (inclusive).</param>
        /// <param name="endDate">The end date. Must be between the <paramref name="startDate"/> and today's date (inclusive). Defaults to today's date.</param>
        Task<ApodResponse> FetchApodAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Fetch an amount of random Astronomy Pictures of the Day.
        /// </summary>
        /// <param name="count">The amount of APODs to fetch. Must be positive and cannot exceed 100.</param>
        Task<ApodResponse> FetchApodAsync(int count);
    }
}
