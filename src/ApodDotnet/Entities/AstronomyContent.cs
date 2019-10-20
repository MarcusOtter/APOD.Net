using System;
using System.Text.Json.Serialization;

namespace ApodDotnet
{
    /// <summary>Contains information about the digital content of an Astronomy Picture of the Day.</summary>
    public class AstronomyContent
    {
        /// <summary>The name of the copyright holder.</summary>
        public string Copyright { get; set; }

        /// <summary>The date when this was the Astronomy Picture of the Day.</summary>
        public DateTime Date { get; set; }

        /// <summary>A description of the content.</summary>
        public string Explanation { get; set; }

        /// <summary>The URL for the high-definition variant of the content.</summary>
        [JsonPropertyName("Hdurl")] 
        public string ContentUrlHD { get; set; }

        /// <summary>The type of media.</summary>
        public MediaType MediaType { get; set; }

        public string ServiceVersion { get; set; }

        /// <summary>The title of the content.</summary>
        public string Title { get; set; }

        /// <summary>The URL for the content. Remember that this can be both image and video content.</summary>
        [JsonPropertyName("Url")]
        public string ContentUrl { get; set; }
    }
}
