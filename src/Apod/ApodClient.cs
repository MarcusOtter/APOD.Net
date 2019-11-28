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
    /// NASA's Astronomy Picture of the Day API can be found at https://api.nasa.gov#apod.
    /// </remarks>
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
        /// To prevent rate limiting, it is recommended to sign up for your own API key at https://api.nasa.gov and use the other constructor.
        /// </remarks>
        public ApodClient() : this("DEMO_KEY") { }

        /// <summary>
        /// Creates a new instance of an Astronomy Picture of the Day client.
        /// </summary>
        /// <param name="apiKey">
        /// Your API key from https://api.nasa.gov.
        /// </param>
        public ApodClient(string apiKey) : this(apiKey, null, null, null) { }

        /// <summary>
        /// Creates a new instance of an Astronomy Picture of the Day client.
        /// </summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov.</param>
        /// <param name="httpRequester">The <see cref="IHttpRequester"/> to use for interacting with the API.</param>
        /// <param name="httpResponseParser">The <see cref="IHttpResponseParser"/> to use for parsing the data from the <paramref name="httpRequester"/>.</param>
        /// <param name="errorHandler">The <see cref="IErrorHandler"/> to handle any errors with the request.</param>        
        public ApodClient(string apiKey, IHttpRequester httpRequester = null, IHttpResponseParser httpResponseParser = null, IErrorHandler errorHandler = null)
        {
            _httpRequester = httpRequester ?? DefaultsFactory.GetHttpRequester(apiKey);
            _httpResponseParser = httpResponseParser ?? DefaultsFactory.GetHttpResponseParser();
            _errorHandler = errorHandler ?? DefaultsFactory.GetErrorHandler();
        }

        /// <summary>
        /// Fetch the current Astronomy Picture of the Day.
        /// </summary>
        public async Task<ApodResponse> FetchApodAsync()
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
        /// <param name="dateTime">The date to request the APOD for. Must be between June 16th 1995 and today's date.</param>
        public async Task<ApodResponse> FetchApodAsync(DateTime dateTime)
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
        /// Fetch all the Astronomy Pictures of the Day between two dates.
        /// </summary>
        /// <param name="startDate">The start date. Must be between June 16th 1995 and today's date.</param>
        /// <param name="endDate">The end date. Must be between the <paramref name="startDate"/> and today's date. Defaults to today's date.</param>
        public async Task<ApodResponse> FetchApodAsync(DateTime startDate, DateTime endDate = default)
        {
            ThrowExceptionIfDisposed();

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
        /// <param name="count">The amount of APODs to fetch. Must be positive and cannot exceed 100.</param>
        public async Task<ApodResponse> FetchApodAsync(int count)
        {
            ThrowExceptionIfDisposed();

            var countError = _errorHandler.ValidateCount(count);
            if (countError.ErrorCode != ApodErrorCode.None) { return countError.ToApodResponse(); }

            var httpResponse = await _httpRequester.SendHttpRequestAsync(count).ConfigureAwait(false);

            var responseError = await _errorHandler.ValidateHttpResponseAsync(httpResponse).ConfigureAwait(false);
            if (responseError.ErrorCode != ApodErrorCode.None) { return responseError.ToApodResponse(); }

            return await _httpResponseParser.ParseMultipleApodsAsync(httpResponse).ConfigureAwait(false);
        }

        private void ThrowExceptionIfDisposed()
        {
            if (_disposed) { throw new ObjectDisposedException(GetType().FullName); }
        }

        /// <summary>
        /// Releases the unmanaged resources and disposes of the managed resources used by the System.Net.Http.HttpMessageInvoker.
        /// </summary>
        public void Dispose()
        {
            if (_disposed) { return; }
            _httpRequester.Dispose();
            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
