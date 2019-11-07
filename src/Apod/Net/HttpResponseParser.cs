using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apod.Net
{
    public class HttpResponseParser : IHttpResponseParser
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpResponseParser(JsonSerializerOptions jsonSerializerOptions = null)
        {
            _jsonSerializerOptions = jsonSerializerOptions ?? GetDefaultJsonSerializerOptions();
        }

        private JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }

        public async Task<ApodResponse> ParseSingleApodAsync(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStreamAsync();
            var apodContent = await JsonSerializer.DeserializeAsync<ApodContent>(responseContent, _jsonSerializerOptions);

            var apodArray = new ApodContent[1] { apodContent };

            return new ApodResponse(ApodStatusCode.OK, apodArray);
        }

        public async Task<ApodResponse> ParseMultipleApodAsync(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStreamAsync();
            var apodContent = await JsonSerializer.DeserializeAsync<ApodContent[]>(responseContent, _jsonSerializerOptions);

            return new ApodResponse(ApodStatusCode.OK, apodContent);
        }
    }
}
