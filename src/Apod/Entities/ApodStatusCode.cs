namespace Apod
{
    /// <summary>
    /// The status code of an <see cref="ApodResponse"/>.
    /// </summary>
    /// <remarks>
    /// Not to be confused with <see cref="ApodErrorCode"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// var response = await client.FetchApodAsync();
    /// if (response.StatusCode != ApodStatusCode.OK)
    /// {
    ///     // handle errors
    ///     return;
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="ApodResponse"/>
    /// <seealso cref="ApodError"/>
    /// <seealso cref="ApodErrorCode"/>
    public enum ApodStatusCode
    {
        /// <summary>
        /// The response contains at least one Astronomy Picture of the Day.
        /// </summary>
        OK,

        /// <summary>
        /// The response does not contain any APODs. 
        /// See <see cref="ApodResponse.Error"/> for more information about the error.
        /// </summary>
        Error
    }
}
