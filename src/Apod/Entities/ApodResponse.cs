namespace Apod
{
    public class ApodResponse
    {
        // TODO: Add ints for ratelimit limit and ratelimit remaining
        // TODO: Add apod url (won't work for multiple content..)
        public readonly ApodStatusCode StatusCode;
        public readonly ApodContent[] Content;
        public readonly ApodError Error;

        public ApodResponse(ApodStatusCode statusCode, ApodContent[] content = null, ApodError error = null)
        {
            StatusCode = statusCode;
            Content = content;
            Error = error;
        }
    }
}
