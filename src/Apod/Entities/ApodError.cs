namespace Apod
{
    /// <summary>
    /// Contains information about errors that occured in the request.
    /// </summary>
    /// <remarks>
    /// The <see cref="ApodError"/> aims to give you more information than the API would about what went wrong and what could be done to fix the problem.
    /// If you feel like it the information could be more concise in any way, make sure to open a pull request with your
    /// proposed changes to the <see cref="Logic.Errors.ErrorBuilder"/> and/or the <see cref="Logic.Errors.ErrorHandler"/> here:
    /// <a href="https://github.com/LeMorrow/APOD.Net/compare">https://github.com/LeMorrow/APOD.Net/compare</a>
    /// </remarks>
    /// <example>
    /// <code>
    /// if (response.StatusCode != ApodStatusCode.OK)
    /// {
    ///     var error = response.Error;
    ///     Console.WriteLine("An error occured.");
    ///     Console.WriteLine(error.ErrorCode);
    ///     Console.WriteLine(error.ErrorMessage);
    /// }
    /// </code>
    /// 
    /// Example output (where the current date is November 28 2019)
    /// <code>
    /// An error occured.
    /// DateOutOfRange
    /// Dates must be between June 16 1995 and November 28 2019.
    /// </code>
    /// </example>
    /// <seealso cref="ApodResponse"/>
    /// <seealso cref="ApodErrorCode"/>
    public class ApodError
    {
        /// <summary>
        /// The error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        /// <remarks>
        /// <para>Useful if you want to make different behaviours depending on the type of error.</para>
        /// <para>If you get the error code <see cref="ApodErrorCode.Unknown"/>, make sure to open an issue here: 
        /// <a href="https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&amp;labels=bug&amp;template=unknown-error.md&amp;title=Unknown+error">
        ///     https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&amp;labels=bug&amp;template=unknown-error.md&amp;title=Unknown+error
        /// </a>
        /// </para>
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
        public readonly ApodErrorCode ErrorCode;

        /// <summary>
        /// Information regarding what went wrong and hints for how to resolve it.
        /// </summary>
        /// <value>
        /// Information regarding what went wrong and hints for how to resolve it.
        /// </value>
        /// <remarks>
        /// If you feel like this message could be more concise, make sure to open a pull request with your
        /// proposed changes to the <see cref="Logic.Errors.ErrorBuilder"/> here:
        /// <a href="https://github.com/LeMorrow/APOD.Net/compare">https://github.com/LeMorrow/APOD.Net/compare</a>
        /// </remarks>
        /// <example>
        /// <code>
        /// if (response.StatusCode != ApodStatusCode.OK)
        /// {
        ///     Console.WriteLine("An error occured.");
        ///     Console.WriteLine(response.Error.ErrorMessage);
        /// }
        /// </code>
        /// 
        /// Example output
        /// <code>
        /// An error occured.
        /// The count must be positive and cannot exceed 100.
        /// </code>
        /// </example>
        public readonly string ErrorMessage;

        /// <summary>
        /// The default constructor for an <see cref="ApodError"/>.
        /// </summary>
        /// <remarks>
        /// Unless you are overriding internal behaviour, you should not need to call this constructor.
        /// </remarks>
        /// <param name="errorCode">The <see cref="ErrorCode"/> value.</param>
        /// <param name="errorMessage">The <see cref="ErrorMessage"/> value.</param>
        public ApodError(ApodErrorCode errorCode, string errorMessage = "")
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Creates a new <see cref="ApodResponse"/> with a <see cref="ApodStatusCode"/> of <see cref="ApodStatusCode.Error"/> 
        /// and an <see cref="ApodError"/> of this instance.
        /// </summary>
        /// <remarks>
        /// Unless you are overriding internal behaviour, you should not need to call this method.
        /// </remarks>
        /// <returns>
        /// The new <see cref="ApodResponse"/>.
        /// </returns>
        public ApodResponse ToApodResponse()
            => new ApodResponse(ApodStatusCode.Error, error: this);
    }
}
