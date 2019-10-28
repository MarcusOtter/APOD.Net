using System;

namespace Apod
{
    internal static class Constants
    {
        /// <summary>The base URL to make the HTTP GET request to.</summary>
        internal const string BaseUrl = "https://api.nasa.gov/planetary/apod";

        /// <summary>The publication date of the very first Astronomy Picture of the Day.</summary>
        internal static readonly DateTime FirstApodDate = new DateTime(1995, 06, 16);
    }
}
