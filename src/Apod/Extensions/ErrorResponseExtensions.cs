using System;
using System.Net.Http;

namespace Apod
{
    internal static class ErrorResponseExtensions
    {
        internal static void ThrowInformativeError(this ErrorResponse errorResponse)
        {
            // This should be okay to remove before using it in production.
            // I'm pretty sure this condition is only met if there's something wrong in this library, the user should not be able to make this happen
            if (errorResponse.Error == null)
            {
                // Also happens on timeout
                throw new ArgumentException("One of the queryParameters were invalid... I think.");
            }

            throw new HttpRequestException($"Unsuccessful HTTP request.\nError code: {errorResponse.Error.Code}\nError message: {errorResponse.Error.Message}\n");
        }
    }
}
