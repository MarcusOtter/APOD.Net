using Apod.Logic.Errors;
using Apod.Logic.Net;

namespace Apod.Logic
{
    // Factory used for dependency inversion
    public static class DefaultsFactory
    {
        public static IHttpRequester GetHttpRequester(string apiKey)
            => new HttpRequester(GetUriBuilder(apiKey));

        public static IErrorHandler GetErrorHandler()
            => new ErrorHandler(GetErrorBuilder());

        public static IHttpResponseParser GetHttpResponseParser()
            => new HttpResponseParser();

        private static IErrorBuilder GetErrorBuilder()
            => new ErrorBuilder();

        private static IApodUriBuilder GetUriBuilder(string apiKey)
            => new ApodUriBuilder(apiKey);
    }
}
