using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apod.Logic.Net
{
    public class HttpResponseParser : IHttpResponseParser
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpResponseParser()
        {
            _jsonSerializerOptions = GetDefaultJsonSerializerOptions();
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
            ApodContent apodContent = null;
            using (var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                apodContent = await JsonSerializer.DeserializeAsync<ApodContent>(responseStream, _jsonSerializerOptions).ConfigureAwait(false);
            }

            httpResponse.Dispose();

            var apodArray = new ApodContent[1] { apodContent };
            return new ApodResponse(ApodStatusCode.OK, apodArray);
        }

        public async Task<ApodResponse> ParseMultipleApodsAsync(HttpResponseMessage httpResponse)
        {
            ApodContent[] apodContent = null;
            using (var responseStream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false))
            {
                apodContent = await JsonSerializer.DeserializeAsync<ApodContent[]>(responseStream, _jsonSerializerOptions).ConfigureAwait(false);
            }

            httpResponse.Dispose();

            return new ApodResponse(ApodStatusCode.OK, apodContent);
        }
    }
}
