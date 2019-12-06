using System;
using System.Text.Json.Serialization;

namespace Apod
{
    /// <summary>
    /// Contains information about the digital content of an Astronomy Picture of the Day.
    /// </summary>
    /// <seealso cref="ApodResponse"/>
    /// <seealso cref="ApodError"/>
    public class ApodContent : IEquatable<ApodContent>
    {
        /// <summary>
        /// The name of the copyright holder.
        /// </summary>
        /// <remarks>
        /// This will be <see langword="null"/> For APODs without copyright.
        /// </remarks>
        /// <value>
        /// The name of the copyright holder.
        /// </value>
        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        /// <summary>
        /// The date when this was the Astronomy Picture of the Day.
        /// </summary>
        /// <value>
        /// The date when this was the Astronomy Picture of the Day.
        /// </value>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// An explanation of the content.
        /// </summary>
        /// <value>
        /// An explanation of the content.
        /// </value>
        [JsonPropertyName("explanation")]
        public string Explanation { get; set; }

        /// <summary>
        /// The URL for the high-definition variant of the content.
        /// </summary>
        /// <remarks>
        /// Will sometimes be the same as <see cref="ContentUrl"/>.
        /// </remarks>
        /// <value>
        /// The URL for the high-definition variant of the content.
        /// </value>
        [JsonPropertyName("hdurl")] 
        public string ContentUrlHD { get; set; }

        /// <summary>
        /// The type of media.
        /// </summary>
        /// <value>
        /// The type of media.
        /// </value>
        [JsonPropertyName("media_type")]
        public MediaType MediaType { get; set; }

        /// <summary>
        /// The NASA API service version used.
        /// </summary>
        /// <remarks>
        /// You cannot influence the value of this property.
        /// </remarks>
        /// <value>
        /// The NASA API service version used.
        /// </value>
        [JsonPropertyName("service_version")]
        public string ServiceVersion { get; set; }

        /// <summary>
        /// The title of the content.
        /// </summary>
        /// <value>
        /// The title of the content.
        /// </value>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// The URL for the content. Remember that this can be both image and video content.
        /// </summary>
        /// <value>
        /// The URL for the content.
        /// </value>
        // .. and in very rare cases, other content (one known instance of interactive content at 2018-10-07, API returns internal service error) 
        [JsonPropertyName("url")]
        public string ContentUrl { get; set; }

        /// <summary>
        /// Check if this <see cref="ApodContent"/> and another <see cref="object"/> <paramref name="obj"/> represent the same Astronomy Picture of the Day.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>Whether or not these objects represent the same Astronomy Picture of the Day.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ApodContent apodContent)
            {
                return Equals(apodContent);
            }

            return false;
        }

        /// <summary>
        /// Check if two <see cref="ApodContent"/> <paramref name="a"/> and <paramref name="b"/> represent the same Astronomy Picture of the Day.
        /// </summary>
        /// <param name="a">The first <see cref="ApodContent"/>.</param>
        /// <param name="b">The second <see cref="ApodContent"/>.</param>
        /// <returns>Whether or not <paramref name="a"/> and <paramref name="b"/> represent the same Astronomy Picture of the Day.</returns>
        public static bool operator ==(ApodContent a, ApodContent b)
        {
            if (ReferenceEquals(a, b)) { return true; }
            if (a is null) { return false; }
            return a.Equals(b);
        }

        /// <summary>
        /// Check if two <see cref="ApodContent"/> <paramref name="a"/> and <paramref name="b"/> represent different Astronomy Pictures of the Day.
        /// </summary>
        /// <param name="a">The first <see cref="ApodContent"/>.</param>
        /// <param name="b">The second <see cref="ApodContent"/>.</param>
        /// <returns>Whether or not <paramref name="a"/> and <paramref name="b"/> represent different Astronomy Pictures of the Day.</returns>
        public static bool operator !=(ApodContent a, ApodContent b)
            => !(a == b);

        /// <summary>
        /// Check if this <see cref="ApodContent"/> and another <see cref="ApodContent"/> <paramref name="other"/> represent the same Astronomy Picture of the Day.
        /// </summary>
        /// <param name="other">The <see cref="ApodContent"/> to compare with.</param>
        /// <returns>Whether or not this <see cref="ApodContent"/> and <paramref name="other"/> represent the same Astronomy Picture of the Day.</returns>
        public bool Equals(ApodContent other)
            => other is object
            && (Copyright, Date, Explanation, ContentUrlHD, MediaType, ServiceVersion, Title, ContentUrl)
            .Equals((other.Copyright, other.Date, other.Explanation, other.ContentUrlHD, other.MediaType, other.ServiceVersion, other.Title, other.ContentUrl));

        public override int GetHashCode()
            => (Copyright, Date, Explanation, ContentUrlHD, MediaType, ServiceVersion, Title, ContentUrl).GetHashCode();
    }
}
