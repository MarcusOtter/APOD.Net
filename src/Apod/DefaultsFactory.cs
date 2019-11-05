using Apod.Net;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Apod
{
    // Factory used for dependency inversion
    public static class DefaultsFactory
    {
        public static IHttpRequester GetHttpRequester(string apiKey) 
            => new HttpRequester(apiKey, new HttpClient());

        public static IErrorHandler GetErrorHandler()
            => new ErrorHandler(GetErrorBuilder());

        private static IErrorBuilder GetErrorBuilder()
            => new ErrorBuilder(Constants.FirstApodDate, DateTime.Today.AddDays(-1));

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
