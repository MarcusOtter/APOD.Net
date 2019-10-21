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
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        /// <summary>Create a new instance of an Astronomy Picture of the Day client.</summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov.</param>
        public ApodClient(string apiKey)
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<AstronomyContent> FetchApodAsync()
            => await FetchApiData();

        public async Task<AstronomyContent> FetchApodAsync(DateTime date)
            => await FetchApiData($"date={date.ToString("yyyy-MM-dd")}");

        private async Task<AstronomyContent> FetchApiData(params string[] queryParameters)
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

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, stringBuilder.ToString());
            var response = await _httpClient.SendAsync(httpRequest);


            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonSerializer.Deserialize<ErrorResponse>(responseContent, options);
                if (error.Error == null)
                {
                    throw new ArgumentException("One of the queryParameters were invalid... I think.", nameof(queryParameters));
                }

                throw new HttpRequestException($"Unsuccessful HTTP request.\nError code: {error.Error.Code}\nError message: {error.Error.Message}\n");
            }

            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<AstronomyContent>(responseContent, options);
        }
    }
}
