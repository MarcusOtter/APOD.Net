using System;

namespace Apod.Net
{
    public interface IApodUriBuilder
    {
        string GetApodUri();
        string GetApodUri(DateTime dateTime);
        string GetApodUri(DateTime startDate, DateTime endDate = default);
        string GetApodUri(int count);
    }
}
