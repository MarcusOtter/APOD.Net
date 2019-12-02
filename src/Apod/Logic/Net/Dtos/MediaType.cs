namespace Apod.Logic.Net.Dtos
{
    /// <summary>
    /// Information about an <see cref="ApodContent"/>'s type of media.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There is currently only one date which does not have media content of type
    /// <see cref="Image"/> or <see cref="Video"/>.
    /// </para>
    /// <para>
    /// When one tries to fetch the data for the Astronomy Picture of the Day for 2018-10-07
    /// through the API, it returns an "Internal Service Error". 
    /// This is because they decided to have interactive flash content that day, which wasn't supported by the API.
    /// This may become supported by this library in a later version.
    /// </para>
    /// </remarks>
    /// <seealso cref="ApodContent"/>
    public enum MediaType
    {
        /// <summary>
        /// The content is a static image.
        /// </summary>
        Image,

        /// <summary>
        /// The content is a video, typically hosted on youtube or vimeo.
        /// </summary>
        Video
    }
}
