using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Logic.Errors
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

        private DateTime GetDefaultFirstValidDate() 
            => new DateTime(1995, 06, 16);

        private DateTime GetDefaultLastValidDate() 
            => DateTime.Today;

        public ApodError ValidateDate(DateTime dateTime)
        {
            if (!DateIsInRange(dateTime)) { return _errorBuilder.GetDateOutOfRangeError(_firstValidDate, _lastValidDate); }
            
            return new ApodError(ApodErrorCode.None);
        }

        public ApodError ValidateDateRange(DateTime startDate, DateTime endDate = default)
        {
            endDate = endDate == default ? _lastValidDate : endDate;

            if (!DateIsInRange(startDate)) { return _errorBuilder.GetDateOutOfRangeError(_firstValidDate, _lastValidDate); }
            if (!DateIsInRange(endDate)) { return _errorBuilder.GetDateOutOfRangeError(_firstValidDate, _lastValidDate); }
            if (DateTime.Compare(startDate, endDate) > 0) { return _errorBuilder.GetStartDateAfterEndDateError(); }

            return new ApodError(ApodErrorCode.None);
        }

        public async Task<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) { return new ApodError(ApodErrorCode.None); }

            if (IsTimeoutError(httpResponse)) { return _errorBuilder.GetTimeoutError(); }

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"There was an error with the HTTP request. Error: \n-----\n{responseContent}\n-----");
            Console.WriteLine($"The ContentType header ToString is: {httpResponse.Content.Headers.ContentType.ToString()}.");

            return new ApodError(ApodErrorCode.BadRequest);
        }

        // If the application times out, it returns html content instead of json.
        private bool IsTimeoutError(HttpResponseMessage httpResponse)
            => httpResponse.Content.Headers.ContentType.ToString().Contains("text/html");

        /// <summary>
        /// Checks if the <paramref name="dateTime"/> is between the first valid date and the last valid date (inclusive).
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to check.</param>
        /// <returns>Whether or not the <paramref name="dateTime"/> is within the allowed range.</returns>
        private bool DateIsInRange(DateTime dateTime)
            => (DateTime.Compare(dateTime, _lastValidDate.AddDays(1)) < 0) // The date is before or equal to the last valid date
            && (DateTime.Compare(dateTime, _firstValidDate.AddDays(-1)) > 0); // The date is after or equal to the first valid date
    }
}
