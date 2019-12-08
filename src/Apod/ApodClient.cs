using Apod.Logic;
using Apod.Logic.Errors;
using Apod.Logic.Net;
using System;
using System.Threading.Tasks;

namespace Apod
{
    /// <summary>
    /// A client for interfacing with NASA's Astronomy Picture of the Day API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Remember to call <see cref="Dispose"/> on the client after you're done using it. 
    /// Read <a href="https://github.com/LeMorrow/APOD.Net#disposing-the-client">https://github.com/LeMorrow/APOD.Net#disposing-the-client</a> to learn more.
    /// </para>
    /// <para>
    /// NASA's Astronomy Picture of the Day API can be found at <a href="https://api.nasa.gov#apod">https://api.nasa.gov#apod</a>.
    /// </para>
    /// </remarks>
    /// <seealso cref="ApodResponse"/>
    /// <seealso cref="ApodError"/>
    /// <seealso cref="ApodContent"/>
    public class ApodClient : IApodClient, IDisposable
    {
        private bool _disposed;

        private readonly IHttpRequester _httpRequester;
        private readonly IHttpResponseParser _httpResponseParser;
        private readonly IErrorHandler _errorHandler;

        /// <summary>
        /// Creates a new instance of an Astronomy Picture of the Day client, using the demo API key.
        /// </summary>
        /// <remarks>
        /// The API key "DEMO_KEY" has an hourly limit of 30 requests per IP adress and a daily limit of 50 requests per IP address.
        /// To prevent rate limiting, it is recommended to sign up for your own API key at <a href="https://api.nasa.gov">https://api.nasa.gov</a> and use the other constructor.
        /// </remarks>
        /// <example>
        /// <code language="csharp">
        /// var client = new ApodClient();
        /// </code>
        /// </example>
        public ApodClient() : this("DEMO_KEY") { }

        /// <summary>
        /// Creates a new instance of an Astronomy Picture of the Day client.
        /// </summary>
        /// <param name="apiKey">
        /// Your API key from <a href="https://api.nasa.gov">https://api.nasa.gov</a>.
        /// </param>
        /// <example>
        /// <code language="csharp">
        /// var client = new ApodClient("YOUR_API_KEY_HERE");
        /// </code>
        /// </example>
        public ApodClient(string apiKey) : this(apiKey, null, null, null) { }

        /// <summary>
        /// Creates a new instance of an Astronomy Picture of the Day client, overriding the internal logic.
        /// </summary>
        /// <remarks>
        /// This constructor can be used for overriding internal logic in the client.
        /// The average user should not need to call this constructor.
        /// </remarks>
        /// <param name="apiKey">Your API key from https://api.nasa.gov.</param>
        /// <param name="httpRequester">The <see cref="IHttpRequester"/> to use for interacting with the API.</param>
        /// <param name="httpResponseParser">The <see cref="IHttpResponseParser"/> to use for parsing the data from the <paramref name="httpRequester"/>.</param>
        /// <param name="errorHandler">The <see cref="IErrorHandler"/> to handle any errors with the request.</param>
        /// <example>
        /// See <a href="../examples/errorhandler.md">Override the IErrorHandler</a> for an example on how to implement and use any of these interfaces.
        /// </example>
        public ApodClient(string apiKey, IHttpRequester httpRequester = null, IHttpResponseParser httpResponseParser = null, IErrorHandler errorHandler = null)
        {
            _httpRequester = httpRequester ?? DefaultsFactory.GetHttpRequester(apiKey);
            _httpResponseParser = httpResponseParser ?? DefaultsFactory.GetHttpResponseParser();
            _errorHandler = errorHandler ?? DefaultsFactory.GetErrorHandler();
        }

        /// <summary>
        /// Fetch the Astronomy Picture of the Day for today's date.
        /// </summary>
        /// <remarks>
        /// "Today's date" refers to the current date in the Eastern Time Zone.
        /// Read more: <a href="https://github.com/LeMorrow/APOD.Net#when-do-new-apods-get-published-by-nasa">https://github.com/LeMorrow/APOD.Net#when-do-new-apods-get-published-by-nasa</a>.
        /// </remarks>
        /// <example>
        /// <code language="csharp">
        /// using var client = new ApodClient();
        /// 
        /// var response = await client.FetchApodAsync();
        /// if (response.StatusCode != ApodStatusCode.OK) { return; }
        ///
        /// Console.WriteLine(response.Content.Date);
        /// Console.WriteLine(response.Content.Title);
        /// </code>
        /// </example>
        /// <exception cref="ObjectDisposedException">Thrown when the client has been disposed.</exception>
        /// <returns>The response.</returns>
        public async ValueTask<ApodResponse> FetchApodAsync()
        {
            ThrowExceptionIfDisposed();

            var httpResponse = await _httpRequester.SendHttpRequestAsync().ConfigureAwait(false);

            var responseError = await _errorHandler.ValidateHttpResponseAsync(httpResponse).ConfigureAwait(false);
            if (responseError.ErrorCode != ApodErrorCode.None) { return responseError.ToApodResponse(); }

            return await _httpResponseParser.ParseSingleApodAsync(httpResponse).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch the Astronomy Picture of the Day for a specific date.
        /// </summary>
        /// <remarks>
        /// <para>
        /// You do not need to consider time zone differences between your application and NASA's API.
        /// If <paramref name="dateTime"/> has today's date (according to your local time zone), the method will take care of it accordingly.
        /// </para>
        /// <para>
        /// Note that this means that if you are ahead of the Eastern Time Zone and ask for today's date after midnight
        /// you would get an APOD with (for you) yesterday's date, since that is the most recent one.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code language="csharp">
        /// using var client = new ApodClient();
        ///
        /// var date = new DateTime(2004, 04, 09);
        /// var response = await client.FetchApodAsync(date);
        ///
        /// if (response.StatusCode != ApodStatusCode.OK) { return; }
        ///
        /// var content = response.Content;
        ///
        /// Console.WriteLine(content.Date);
        /// Console.WriteLine(content.Title);
        /// </code>
        /// </example>
        /// <exception cref="ObjectDisposedException">Thrown when the client has been disposed.</exception>
        /// <param name="dateTime">The date to request the APOD for. Must be between June 16th 1995 and today's date (inclusive).</param>
        /// <returns>The response.</returns>
        public async ValueTask<ApodResponse> FetchApodAsync(DateTime dateTime)
        {
            if (dateTime.Date == DateTime.Today) { return await FetchApodAsync().ConfigureAwait(false); }

            ThrowExceptionIfDisposed();

            var dateError = _errorHandler.ValidateDate(dateTime);
            if (dateError.ErrorCode != ApodErrorCode.None) { return dateError.ToApodResponse(); }

            var httpResponse = await _httpRequester.SendHttpRequestAsync(dateTime).ConfigureAwait(false);

            var responseError = await _errorHandler.ValidateHttpResponseAsync(httpResponse).ConfigureAwait(false);
            if (responseError.ErrorCode != ApodErrorCode.None) { return responseError.ToApodResponse(); }

            return await _httpResponseParser.ParseSingleApodAsync(httpResponse).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch all the Astronomy Pictures of the Day between two dates (inclusive).
        /// </summary>
        /// <remarks>
        /// <para>
        /// You do not need to consider time zone differences between your application and NASA's API.
        /// If <paramref name="endDate"/> has today's date (according to your local time zone), the method will take care of it accordingly.
        /// </para>
        /// <para>
        /// Note that this means that if you are ahead of the Eastern Time Zone and ask for all APODs between yesterday and today just after midnight (locally),
        /// you would only get one APOD with (for you) yesterday's date, since a picture for your "today" doesn't exist yet.
        /// </para>
        /// <para>
        /// It is therefore not possible to safely assume the amount of APODs this method will return,
        /// since it depends on time zone differences and the current time.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code language="csharp">
        /// using var client = new ApodClient();
        ///
        /// var startDate = new DateTime(2008, 10, 29);
        /// var endDate = new DateTime(2008, 11, 02);
        /// var response = await client.FetchApodAsync(startDate, endDate);
        ///
        /// if (response.StatusCode != ApodStatusCode.OK) { return; }
        ///
        /// foreach (var apod in response.AllContent)
        /// {
        ///     Console.WriteLine($"{apod.Date}: {apod.Title}");
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ObjectDisposedException">Thrown when the client has been disposed.</exception>
        /// <param name="startDate">The start date. Must be between June 16th 1995 and today's date (inclusive).</param>
        /// <param name="endDate">The end date. Must be between the <paramref name="startDate"/> and today's date (inclusive). Defaults to today's date.</param>
        /// <returns>The response.</returns>
        public async ValueTask<ApodResponse> FetchApodAsync(DateTime startDate, DateTime endDate = default)
        {
            ThrowExceptionIfDisposed();

            if (endDate.Date == DateTime.Today) { endDate = default; }

            var dateError = _errorHandler.ValidateDateRange(startDate, endDate);
            if (dateError.ErrorCode != ApodErrorCode.None) { return dateError.ToApodResponse(); }

            var httpResponse = await _httpRequester.SendHttpRequestAsync(startDate, endDate).ConfigureAwait(false);

            var responseError = await _errorHandler.ValidateHttpResponseAsync(httpResponse).ConfigureAwait(false);
            if (responseError.ErrorCode != ApodErrorCode.None) { return responseError.ToApodResponse(); }

            return await _httpResponseParser.ParseMultipleApodsAsync(httpResponse).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch an amount of random Astronomy Pictures of the Day.
        /// </summary>
        /// <example>
        /// <code language="csharp">
        /// using var client = new ApodClient();
        ///
        /// var amount = 3;
        /// var response = await client.FetchApodAsync(amount);
        ///
        /// if (response.StatusCode != ApodStatusCode.OK) { return; }
        ///
        /// foreach (var apod in response.AllContent)
        /// {
        ///     Console.WriteLine($"{apod.Date}: {apod.Title}");
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ObjectDisposedException">Thrown when the client has been disposed.</exception>
        /// <param name="count">The amount of APODs to fetch. Must be positive and cannot exceed 100.</param>
        /// <returns>The response.</returns>
        public async ValueTask<ApodResponse> FetchApodAsync(int count)
        {
            ThrowExceptionIfDisposed();

            var countError = _errorHandler.ValidateCount(count);
            if (countError.ErrorCode != ApodErrorCode.None) { return countError.ToApodResponse(); }

            var httpResponse = await _httpRequester.SendHttpRequestAsync(count).ConfigureAwait(false);

            var responseError = await _errorHandler.ValidateHttpResponseAsync(httpResponse).ConfigureAwait(false);
            if (responseError.ErrorCode != ApodErrorCode.None) { return responseError.ToApodResponse(); }

            return await _httpResponseParser.ParseMultipleApodsAsync(httpResponse).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the apod.nasa.gov permalink for an Astronomy Picture of the Day. Example response:
        /// <a href="https://apod.nasa.gov/apod/ap191208.html">https://apod.nasa.gov/apod/ap191208.html</a>.
        /// </summary>
        /// <param name="apodContent">The Astronomy Picture of the Day to get the permalink for.</param>
        /// <returns>The permalink URL to this APOD.</returns>
        public string GetPermalink(ApodContent apodContent)
        {
            var date = apodContent.Date;
            var year = date.ToString("yy");
            var month = date.ToString("MM");
            var day = date.ToString("dd");

            return $"https://apod.nasa.gov/apod/ap{year}{month}{day}.html";
        }

        private void ThrowExceptionIfDisposed()
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }
        }

        /// <summary>
        /// Releases the unmanaged resources and disposes of the managed resources used by the <see cref="System.Net.Http.HttpMessageInvoker"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Remember to call this method once you are done using the client.
        /// </para>
        /// <para>
        /// Read <a href="https://github.com/LeMorrow/APOD.Net#disposing-the-client">https://github.com/LeMorrow/APOD.Net#disposing-the-client</a> to learn more.
        /// </para>
        /// </remarks>
        public void Dispose()
        {
            if (_disposed) { return; }
            _httpRequester.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
