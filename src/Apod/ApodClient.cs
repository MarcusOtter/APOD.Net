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

        public async Task<AstronomyContent> FetchApodAsync()
        {
            var responseMessage = await FetchApiDataAsync();
            return await ResponseToAstronomyContentAsync(responseMessage);
        }

        public async Task<AstronomyContent> FetchApodAsync(DateTime date)
        {
            var queryParameter = $"date={date.ToString("yyyy-MM-dd")}";
            var responseMessage = await FetchApiDataAsync(queryParameter);
            return await ResponseToAstronomyContentAsync(responseMessage);
        }

        private async Task<HttpResponseMessage> FetchApiDataAsync(params string[] queryParameters)
        {
            var requestUri = BuildFullQueryString(queryParameters);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            return await _httpClient.SendAsync(requestMessage);
        }

        private async Task<AstronomyContent> ResponseToAstronomyContentAsync(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonSerializerOptions);
                errorResponse.ThrowInformativeError();
            }

            return JsonSerializer.Deserialize<AstronomyContent>(responseContent, _jsonSerializerOptions);
        }

        private string BuildFullQueryString(params string[] queryParameters)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder
                .Append(Constants.BaseUrl)
                .Append("?api_key=").Append(_apiKey);

            foreach (var parameter in queryParameters)
            {
                stringBuilder.Append("&");
                stringBuilder.Append(parameter);
            }

            return stringBuilder.ToString();
        }
    }
}
