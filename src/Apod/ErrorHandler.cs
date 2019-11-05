using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IErrorBuilder _errorBuilder;
        private readonly DateTime _firstValidDate;
        private readonly DateTime _lastValidDate;

        public ErrorHandler(IErrorBuilder errorBuilder, DateTime firstValidDate = default, DateTime lastValidDate = default)
        {
            _errorBuilder = errorBuilder;
            _firstValidDate = firstValidDate == default ? GetDefaultFirstValidDate() : firstValidDate;
            _lastValidDate = lastValidDate == default ? GetDefaultLastValidDate() : lastValidDate;
        }

        private DateTime GetDefaultFirstValidDate() => new DateTime(1995, 06, 16);
        private DateTime GetDefaultLastValidDate() => DateTime.Today;

        public ApodResponse ValidateDate(DateTime dateTime)
        {
            if (!DateIsInRange(dateTime)) { return _errorBuilder.GetDateOutOfRangeError(_firstValidDate, _lastValidDate); }
            
            return new ApodResponse(ApodStatusCode.OK);
        }

        public ApodResponse ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            if (!DateIsInRange(startDate)) { return _errorBuilder.GetDateOutOfRangeError(_firstValidDate, _lastValidDate); }
            if (!DateIsInRange(endDate)) { return _errorBuilder.GetDateOutOfRangeError(_firstValidDate, _lastValidDate); }
            if (DateTime.Compare(startDate, endDate) > 0) { return _errorBuilder.GetStartDateAfterEndDateError(); }

            return new ApodResponse(ApodStatusCode.OK);
        }

        public async Task<ApodResponse> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"There was an error with the HTTP request. Error: \n-----\n{responseContent}\n-----");
                Console.WriteLine($"The ContentType header ToString is: {httpResponse.Content.Headers.ContentType.ToString()}.");
                return new ApodResponse(ApodStatusCode.Error);
            }

            return new ApodResponse(ApodStatusCode.OK);
        }

        /// <summary>
        /// Checks if the <paramref name="dateTime"/> is after or equal to the first valid date and before or equal to the last valid date.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to check.</param>
        /// <returns>Whether or not the <paramref name="dateTime"/> is within the allowed range.</returns>
        private bool DateIsInRange(DateTime dateTime)
            => (DateTime.Compare(dateTime, _lastValidDate.AddDays(1)) < 0) // The date is before or equal to the last valid date
            && (DateTime.Compare(dateTime, _firstValidDate.AddDays(-1)) > 0); // The date is after or equal to the first valid date
    }
}
