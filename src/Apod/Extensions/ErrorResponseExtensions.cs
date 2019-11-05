using System;
using System.Net.Http;

namespace Apod
{
    internal static class ErrorResponseExtensions
    {
        //internal static void ThrowInformativeError(this ErrorResponse errorResponse)
        //{
        //    if (errorResponse.Error == null)
        //    {
        //        // Happens if some parameter is invalid, if it times out or if the link is invalid...
        //        throw new ArgumentException("One of the queryParameters were invalid... I think.");
        //    }

        //    throw new HttpRequestException($"Unsuccessful HTTP request.\nError code: {errorResponse.Error.Code}\nError message: {errorResponse.Error.Message}\n");
        //}
    }
}
