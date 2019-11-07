using System;
using System.Text.Json.Serialization;

namespace Apod.Logic.Net.Dtos
{
    /// <summary>Contains information about the digital content of an Astronomy Picture of the Day.</summary>
    public class ApodContent
    {
        /// <summary>The name of the copyright holder.</summary>
        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        /// <summary>The date when this was the Astronomy Picture of the Day.</summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>A description of the content.</summary>
        [JsonPropertyName("explanation")]
        public string Explanation { get; set; }

        /// <summary>The URL for the high-definition variant of the content.</summary>
        [JsonPropertyName("hdurl")] 
        public string ContentUrlHD { get; set; }

        /// <summary>The type of media.</summary>
        [JsonPropertyName("media_type")]
        public MediaType MediaType { get; set; }

        [JsonPropertyName("service_version")]
        public string ServiceVersion { get; set; }

        /// <summary>The title of the content.</summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>The URL for the content. Remember that this can be both image and video content.</summary>
        [JsonPropertyName("url")]
        public string ContentUrl { get; set; }
    }
}
