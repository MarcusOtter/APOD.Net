using Apod.Net;
using System; // Should move dates to error builder

namespace Apod
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
            => new ErrorBuilder(Constants.FirstApodDate, DateTime.Today.AddDays(-1));

        private static IApodUriBuilder GetUriBuilder(string apiKey)
            => new ApodUriBuilder(apiKey);
    }
}
