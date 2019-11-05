using System;
using System.Text;

namespace Apod.Net
{
    public class ApodUriBuilder : IApodUriBuilder
    {
        private readonly string _apiKey;
        /// <summary>The base URI to make the HTTP GET request to.</summary>
        private readonly string _baseUri;
        private readonly string _dateFormat;

        private readonly StringBuilder _stringBuilder;

        public ApodUriBuilder(string apiKey, string baseUri = "https://api.nasa.gov/planetary/apod", string dateFormat = "yyyy-MM-dd")
        {
            _apiKey = apiKey;
            _baseUri = baseUri;
            _stringBuilder = new StringBuilder();
        }

        public string GetApodUri() 
            => BuildUri();

        public string GetApodUri(DateTime dateTime) 
            => BuildUri($"date={dateTime.ToString(_dateFormat)}");

        public string GetApodUri(DateTime startDate, DateTime endDate = default)
            => BuildUri($"start_date={startDate.ToString(_dateFormat)}",
                endDate == default ? string.Empty : $"end_date={endDate.ToString(_dateFormat)}");

        public string GetApodUri(int count)
            => BuildUri($"count={count}");

        private string BuildUri(params string[] queryParameters)
        {
            _stringBuilder.Append(_baseUri).Append("?api_key=").Append(_apiKey);

            foreach (var parameter in queryParameters)
            {
                if (string.IsNullOrWhiteSpace(parameter)) { continue; }
                _stringBuilder.Append("&");
                _stringBuilder.Append(parameter);
            }

            return _stringBuilder.ToString();
        }
    }
}
