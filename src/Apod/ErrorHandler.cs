using System;

namespace Apod
{
    public class ErrorHandler : IErrorHandler
    {
        /// <summary>
        /// The UTC offset (time zone) of NASA's API server. 
        /// A new APOD is published when the time in this time zone passes midnight (approximately).
        /// </summary>
        /// <seealso cref="https://support.microsoft.com/en-gb/help/973627/microsoft-time-zone-index-values"/>
        private readonly TimeSpan _apiUtcOffset = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset;

        /// <summary>The publication date of the very first Astronomy Picture of the Day.</summary>
        private readonly DateTime _firstApodDate;

        /// <summary>The publication date of the latest Astronomy Picture of the Day.</summary>
        private readonly DateTime _latestApodDate;

        public ErrorHandler()
        {
            _firstApodDate = new DateTime(1995, 06, 16);
            _latestApodDate = SetUtcOffset(DateTime.Now, _apiUtcOffset);
        }

        public ApodResponse ValidateDate(DateTime dateTime)
        {   
            if (dateTime.TimeOfDay != TimeSpan.Zero  && // If this DateTime contains information about the time of day
                dateTime.Kind is DateTimeKind.Local) // and the DateTime is based on the local time zone
            { 
                dateTime = SetUtcOffset(dateTime, _apiUtcOffset); 
            }

            if (!DateIsInRange(dateTime)) { return GetDateOutOfRangeError(); }

            return new ApodResponse(ApodStatusCode.OK);
        }

        /// <summary>
        /// Checks if the <paramref name="dateTime"/> is after or equal to the first APOD's publication date and before or equal to the latest APOD's publication date.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to check.</param>
        /// <returns>Whether or not the <paramref name="dateTime"/> is within the allowed range.</returns>
        private bool DateIsInRange(DateTime dateTime)
            => (DateTime.Compare(dateTime, _latestApodDate.AddDays(1)) < 0) // The date is before or equal to the latest APOD's publication date
            && (DateTime.Compare(dateTime, _firstApodDate.AddDays(-1)) > 0); // The date is after or equal to the first APOD's publication date

        // Adding an example would be good for this method, even though it's private.
        /// <summary>
        /// Changes the time of a <see cref="DateTime"/> to a new UTC offset.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to set the UTC offset for.</param>
        /// <param name="utcOffset">The new UTC offset.</param>
        /// <returns>The new <see cref="DateTime"/> with a time modified to the given time zone.</returns>
        private DateTime SetUtcOffset(DateTime dateTime, TimeSpan utcOffset)
        {
            if (dateTime.Kind is DateTimeKind.Unspecified)
            {
                // TODO: HANDLE ERRORS IN SOME WAY NEATLY TO THE USER.
                return DateTime.MinValue;
                throw new ArgumentException("Cannot set UTC offset for DateTimes with an unspecified kind.", nameof(dateTime));
            }

            var dateTimeOffset = new DateTimeOffset(dateTime);
            dateTimeOffset.ToOffset(utcOffset);
            return dateTimeOffset.DateTime;
        }

        private ApodResponse GetDateOutOfRangeError()
        {
            var format = "MMMM dd, yyyy";
            var errorMessage = $"Dates must be between {_firstApodDate.ToString(format)} and {SetUtcOffset(DateTime.Now, _apiUtcOffset).ToString(format)}.";
            var apodError = new ApodError(ApodErrorCode.BadRequest, errorMessage);
            var apodResponse = new ApodResponse(ApodStatusCode.Error, error: apodError);
            return apodResponse;
        }
    }
}
