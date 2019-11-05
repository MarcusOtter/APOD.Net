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

        // Might need a ParseMultiple or ParseSingle

        public async Task<ApodResponse> ParseAsync(HttpResponseMessage httpResponse)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var apodContent = JsonSerializer.Deserialize<ApodContent>(responseContent, _jsonSerializerOptions);

            var apodArray = new ApodContent[1] { apodContent };

            return new ApodResponse(ApodStatusCode.OK, apodArray);
        }
    }
}
