using Apod.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Apod
{
    public static class DependencyInversionFactory
    {
        public static IHttpRequester GetHttpRequester(string apiKey) 
            => new HttpRequester(new HttpClient(), apiKey);

        public static IErrorHandler GetErrorHandler()
            => new ErrorHandler();

        // Can be private and moved to HttpRequester
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }
    }
}
