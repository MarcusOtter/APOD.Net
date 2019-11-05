using System;

namespace Apod
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IErrorBuilder _errorBuilder;

        public ErrorHandler(IErrorBuilder errorBuilder)
        {
            _errorBuilder = errorBuilder;
        }

        public ApodResponse ValidateDate(DateTime dateTime)
        {
            if (dateTime.Date == DateTime.Today) { } // Should not happen, make sure to check for this in the client
            if (!DateIsInRange(dateTime)) { return _errorBuilder.GetDateOutOfRangeError(); }
            
            return new ApodResponse(ApodStatusCode.OK);
        }

        public ApodResponse ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            if (!DateIsInRange(startDate)) { return _errorBuilder.GetDateOutOfRangeError(); }
            if (!DateIsInRange(endDate)) { return _errorBuilder.GetDateOutOfRangeError(); }
            if (DateTime.Compare(startDate, endDate) > 0) { return _errorBuilder.GetStartDateAfterEndDateError(); }

            return new ApodResponse(ApodStatusCode.OK);
        }

        /// <summary>
        /// Checks if the <paramref name="dateTime"/> is after or equal to the first APOD's publication date and before today's date.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to check.</param>
        /// <returns>Whether or not the <paramref name="dateTime"/> is within the allowed range.</returns>
        private bool DateIsInRange(DateTime dateTime)
            => (DateTime.Compare(dateTime, DateTime.Today) < 0) // The date is before today's date
            && (DateTime.Compare(dateTime, Constants.FirstApodDate.AddDays(-1)) > 0); // The date is after or equal to the first APOD's publication date
    }
}
