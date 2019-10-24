using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apod
{
    /// <summary>The client that is used to consume the Astronomy Picture of the Day API.</summary>
    public class ApodClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>Create a new instance of an Astronomy Picture of the Day client.</summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov.</param>
        public ApodClient(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();

            // Factory pattern necessary?
            _jsonSerializerOptions = new JsonSerializerOptions();
            _jsonSerializerOptions.PropertyNameCaseInsensitive = true;
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        /// <summary>Fetch the current Astronomy Picture of the Day.</summary>
        public async Task<AstronomyContent> FetchApodAsync()
        {
            var responseMessage = await FetchApiDataAsync();
            return await GetOneApodResult(responseMessage);
        }

        /// <summary>Fetch the Astronomy Picture of the Day for a specific date.</summary>
        /// <param name="date">The date to request the APOD for. Must be between June 16th 1995 and today's date.</param>
        public async Task<AstronomyContent> FetchApodAsync(DateTime date)
        {
            // TODO: Error handling for invalid dates

            var queryParameter = $"date={date.ToString("yyyy-MM-dd")}";
            var responseMessage = await FetchApiDataAsync(queryParameter);
            return await GetOneApodResult(responseMessage);
        }

        /// <summary>Fetch all the Astronomy Pictures of the Day between two dates.</summary>
        /// <param name="startDate">The start date. Must be between June 16th 1995 and today's date.</param>
        /// <param name="endDate">The end date. Must be between the <paramref name="startDate"/> and today's date. Defaults to <see cref="DateTime.Today"/>.</param>
        public async Task<AstronomyContent[]> FetchApodAsync(DateTime startDate, DateTime endDate = default)
        {
            if (!DateIsInRange(startDate))                     { throw new DateOutOfRangeException(nameof(startDate), startDate); }
            if (endDate != default && !DateIsInRange(endDate)) { throw new DateOutOfRangeException(nameof(endDate), endDate); }
            if (DateTime.Compare(startDate, endDate) > 0)      { throw new DateOutOfRangeException("The start date can not be after the end date."); }

            var startDateString = $"start_date={startDate.ToString("yyyy-MM-dd")}";

            var endDateString = endDate == default
                ? string.Empty 
                : $"end_date={endDate.ToString("yyyy-MM-dd")}";

            var responseMessage = await FetchApiDataAsync(startDateString, endDateString);
            return await GetMultipleApodResults(responseMessage);
        }

        private bool DateIsInRange(DateTime dateTime)
            => (DateTime.Compare(dateTime, DateTime.Today) < 0) && (DateTime.Compare(dateTime, Constants.FirstApodDate) > 0);

        private async Task<HttpResponseMessage> FetchApiDataAsync(params string[] queryParameters)
        {
            var requestUri = BuildFullQueryString(queryParameters);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(requestMessage);
        }

        private async Task<AstronomyContent> GetOneApodResult(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonSerializerOptions);
                errorResponse.ThrowInformativeError();
            }

            return JsonSerializer.Deserialize<AstronomyContent>(responseContent, _jsonSerializerOptions);
        }

        private async Task<AstronomyContent[]> GetMultipleApodResults(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonSerializerOptions);
                errorResponse.ThrowInformativeError();
            }

            return JsonSerializer.Deserialize<AstronomyContent[]>(responseContent, _jsonSerializerOptions);
        }

        private string BuildFullQueryString(string[] queryParameters)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder
                .Append(Constants.BaseUrl)
                .Append("?api_key=").Append(_apiKey);

            foreach (var parameter in queryParameters)
            {
                if (string.IsNullOrWhiteSpace(parameter)) { continue; }
                stringBuilder.Append("&");
                stringBuilder.Append(parameter);
            }

            return stringBuilder.ToString();
        }
    }
}
