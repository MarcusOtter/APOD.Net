using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apod.Logic.Errors
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly IErrorBuilder _errorBuilder;
        private readonly DateTime _firstValidDate;
        private readonly DateTime _lastValidDate;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ErrorHandler(IErrorBuilder errorBuilder, DateTime firstValidDate = default, DateTime lastValidDate = default)
        {
            _errorBuilder = errorBuilder;
            _firstValidDate = firstValidDate == default ? GetDefaultFirstValidDate() : firstValidDate;
            _lastValidDate = lastValidDate == default ? GetDefaultLastValidDate() : lastValidDate;
            _jsonSerializerOptions = GetDefaultJsonSerializerOptions();
        }

        private DateTime GetDefaultFirstValidDate() 
            => new DateTime(1995, 06, 16);

        private DateTime GetDefaultLastValidDate() 
            => DateTime.Today;

        private JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }

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

        public ApodError ValidateCount(int count)
        {
            if (!CountIsInRange(count)) { return _errorBuilder.GetCountOutOfRangeError(); }
            return new ApodError(ApodErrorCode.None);
        }

        public async Task<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) { return new ApodError(ApodErrorCode.None); }

            Console.WriteLine($"The http status code is {httpResponse.StatusCode}");
            Console.WriteLine("----------");
            Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
            Console.WriteLine("----------");

            if (IsTimeoutError(httpResponse)) { return _errorBuilder.GetTimeoutError(); }

            JsonElement errorObject = default;
            using (var responseStream = await httpResponse.Content.ReadAsStreamAsync())
            {
                errorObject = await JsonSerializer.DeserializeAsync<JsonElement>(responseStream, _jsonSerializerOptions);
            }

            if (ErrorHasServiceVersionProperty(errorObject))
            {
                var code = int.Parse(errorObject.GetProperty("code").ToString());
                var errorMessage = errorObject.GetProperty("msg").ToString();

                switch (code)
                {
                    case 400: return _errorBuilder.GetBadRequestError(errorMessage);
                    case 500: return _errorBuilder.GetInternalServiceError(errorMessage);
                    default:  return _errorBuilder.GetUnknownError(errorMessage);
                }
            }
            
            var hasError = errorObject.TryGetProperty("error", out var error);
            if (!hasError) { return _errorBuilder.GetUnknownError("An unknown error occured."); }

            var errorCode = error.GetProperty("code").ToString();
            var apodErrorCode = GetApodErrorCode(errorCode);

            switch (apodErrorCode)
            {
                case ApodErrorCode.ApiKeyMissing: return _errorBuilder.GetApiKeyMissingError();
                case ApodErrorCode.ApiKeyInvalid: return _errorBuilder.GetApiKeyInvalidError();
                case ApodErrorCode.OverRateLimit: return _errorBuilder.GetOverRateLimitError();
                default:                          return _errorBuilder.GetUnknownError();
            }
        }

        private ApodErrorCode GetApodErrorCode(string errorCode)
        {
            switch (errorCode)
            {
                case "API_KEY_MISSING": return ApodErrorCode.ApiKeyMissing;
                case "API_KEY_INVALID": return ApodErrorCode.ApiKeyInvalid;
                case "OVER_RATE_LIMIT": return ApodErrorCode.OverRateLimit;
                default:                return ApodErrorCode.Unknown;
            }
        }

        private bool ErrorHasServiceVersionProperty(JsonElement errorObject)
            => errorObject.TryGetProperty("service_version", out var _);


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

        private bool CountIsInRange(int count)
            => count > 0 && count <= 100;
    }
}
