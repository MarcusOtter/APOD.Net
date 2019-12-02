using Apod.Logic.Net.Dtos;

namespace Apod
{
    /// <summary>
    /// Contains information about the response from a request to the API.
    /// </summary>
    /// <remarks>
    /// Make sure that the <see cref="StatusCode"/> is <see cref="ApodStatusCode.OK"/>
    /// before trying to access any content.
    /// </remarks>
    /// <example>
    /// <code>
    /// using var client = new ApodClient();
    /// var response = await client.FetchApodAsync();
    ///
    /// if (response.StatusCode != ApodStatusCode.OK)
    /// {
    ///     Console.WriteLine(response.Error.ErrorMessage);
    ///     return;
    /// }
    ///
    /// Console.WriteLine(response.Content.Title);
    /// Console.WriteLine(response.Content.Explanation);
    /// </code>
    /// </example>
    /// <seealso cref="ApodContent"/>
    /// <seealso cref="ApodStatusCode"/>
    /// <seealso cref="ApodError"/>
    public class ApodResponse
    {
        /// <summary>
        /// The status code of the request. 
        /// Always make sure this is equal to <see cref="ApodStatusCode.OK"/> before trying to access any content.
        /// </summary>
        /// <value>
        /// The status code of the request.
        /// </value>
        public readonly ApodStatusCode StatusCode;

        /// <summary>
        /// All the ApodContent of the response from the API request.
        /// </summary>
        /// <value>
        /// All the ApodContent of the response from the API request.
        /// </value>
        /// <remarks>
        /// If the <see cref="StatusCode"/> is <see cref="ApodStatusCode.OK"/> 
        /// you can safely assume that this array is initialized.
        /// </remarks>
        /// <example>
        /// <code>
        /// var date = new DateTime(2000, 01, 01);
        /// var response = await client.FetchApodAsync(date);
        /// Console.WriteLine(response1.AllContent?.Length);
        /// </code>
        /// <para>Output: <c>1</c></para>
        /// <br/>
        /// </example>
        /// <example>
        /// <code>
        /// var response = await client.FetchApodAsync(5);
        /// Console.WriteLine(response.AllContent?.Length);
        /// </code>
        /// <para>Output: <c>5</c></para>
        /// <br/>
        /// </example>
        public readonly ApodContent[] AllContent;

        /// <summary>
        /// The ApodContent of the response from the API request.
        /// </summary>
        /// <value>
        /// The ApodContent of the response from the API request.
        /// </value>
        /// <remarks>
        /// If the response contains more than one ApodContent, use <see cref="AllContent"/> to access all of them.
        /// If you still use this field, it will contain the most recent Astronomy Picture of the Day.
        /// </remarks>
        public readonly ApodContent Content;

        /// <summary>
        /// Information about the error with the request, if any. 
        /// </summary>
        /// <value>
        /// Information about the error with the request.
        /// </value>
        /// <remarks>
        /// This will be null if the <see cref="StatusCode"/> is <see cref="ApodStatusCode.OK"/>.
        /// </remarks>
        public readonly ApodError Error;

        /// <summary>
        /// The default constructor for an <see cref="ApodResponse"/>.
        /// </summary>
        /// <remarks>
        /// Unless you are overriding internal behaviour, you should not need to call this constructor.
        /// </remarks>
        /// <param name="statusCode">The <see cref="StatusCode"/> value.</param>
        /// <param name="allContent">The <see cref="AllContent"/> value.</param>
        /// <param name="error">The <see cref="Error"/> value.</param>
        public ApodResponse(ApodStatusCode statusCode, ApodContent[] allContent = null, ApodError error = null)
        {
            StatusCode = statusCode;
            AllContent = allContent;
            // Set the Content to the latest entry from AllContent
            Content = AllContent?[AllContent.Length > 1 ? AllContent.Length - 1 : 0];
            Error = error;
        }
    }
}
