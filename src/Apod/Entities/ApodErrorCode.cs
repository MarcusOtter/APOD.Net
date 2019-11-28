namespace Apod
{
    /// <summary>
    /// The error code of an <see cref="ApodError"/>.
    /// </summary>
    /// <remarks>
    /// Not to be confused with <see cref="ApodStatusCode"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// var error = response.Error;
    /// switch (error.ErrorCode)
    /// {
    ///     case ApodErrorCode.DateOutOfRange:
    ///         Console.WriteLine(error.ErrorMessage);
    ///         break;
    ///
    ///     case ApodErrorCode.Timeout:
    ///         Console.WriteLine("Try with dates that are closer together.");
    ///         break;
    ///
    ///     case ApodErrorCode.OverRateLimit:
    ///         Console.WriteLine("You have made too many requests. Come back in a bit.");
    ///         break;
    /// 
    ///     default:
    ///         Console.WriteLine("An error occured. Try again.");
    ///         break;
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="ApodError"/>
    /// <seealso cref="ApodStatusCode"/>
    public enum ApodErrorCode
    {
        /// <summary>
        /// The previous request was successful.
        /// </summary>
        None,

        /// <summary>
        /// <para>The API returned a BadRequest error that this library did not catch. </para>
        /// <para>If you get an error with this code, please file a bug report at 
        /// <a href="https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&amp;labels=bug&amp;template=bug_report.md&amp;title=">
        ///     https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&amp;labels=bug&amp;template=bug_report.md&amp;title=
        /// </a></para>
        /// </summary>
        BadRequest,

        /// <summary>
        /// One of the dates you provided were outside of the allowed range.
        /// </summary>
        DateOutOfRange,

        /// <summary>
        /// The start date was after the end date.
        /// </summary>
        StartDateAfterEndDate,

        /// <summary>
        /// The count that you provided was outside of the allowed range.
        /// </summary>
        CountOutOfRange,

        /// <summary>
        /// <para>The server had an internal error.</para>
        /// <para>This could for example mean that it timed out without giving a proper timeout error
        /// or that NASA's API servers are down currently.</para>
        /// </summary>
        InternalServiceError,

        /// <summary>
        /// You did not provide an API key.
        /// </summary>
        ApiKeyMissing,

        /// <summary>
        /// Your API key was invalid.
        /// </summary>
        ApiKeyInvalid,

        /// <summary>
        /// The server timed out. Try making smaller requests or try again later.
        /// </summary>
        Timeout,

        /// <summary>
        /// You have done too many requests in the past hour.
        /// </summary>
        OverRateLimit,

        /// <summary>
        /// <para>You have encountered a completely unkown bug. Congratulations!</para>
        /// <para>Please take a few seconds and tell us what you did here:
        /// <a href="https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&amp;labels=bug&amp;template=unknown-error.md&amp;title=Unknown+error">
        ///     https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&amp;labels=bug&amp;template=unknown-error.md&amp;title=Unknown+error
        /// </a></para>
        /// </summary>
        Unknown
    }
}
