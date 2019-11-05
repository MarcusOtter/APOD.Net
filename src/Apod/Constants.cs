using System;

namespace Apod
{
    // Should be removed completely
    internal static class Constants
    {
        // can be injected to the error handler (and the error builder)
        /// <summary>The publication date of the first Astronomy Picture of the Day.</summary>
        internal static readonly DateTime FirstApodDate = new DateTime(1995, 06, 16);
    }
}
