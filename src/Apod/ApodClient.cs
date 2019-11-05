using Apod.Net;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Apod
{
    /// <summary>The client that is used to consume the Astronomy Picture of the Day API.</summary>
    public class ApodClient : IApodClient
    {
        private readonly string _apiKey;
        private readonly IErrorHandler _errorHandler;
        private readonly IHttpRequester _httpRequester;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>Creates a new instance of an Astronomy Picture of the Day client, using the demo API key.</summary>
        /// <remarks>
        /// The API key "DEMO_KEY" has an hourly limit of 30 requests per IP adress and a daily limit of 50 requests per IP address.
        /// To prevent rate limiting, you are encouraged to sign up for your own API key at https://api.nasa.gov and use the other constructor.
        /// </remarks>
        public ApodClient() : this("DEMO_KEY") { }

        /// <summary>Creates a new instance of an Astronomy Picture of the Day client.</summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov. </param>
        public ApodClient(string apiKey)
            : this(apiKey, DefaultsFactory.GetHttpRequester(apiKey), DefaultsFactory.GetErrorHandler()) { }

        /// <summary>Creates a new instance of an Astronomy Picture of the Day client.</summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov.</param>
        /// <param name="httpRequester">The <see cref="IHttpRequester"/> to use for interacting with the API.</param>
        public ApodClient(string apiKey, IHttpRequester httpRequester)
            : this(apiKey, httpRequester, DefaultsFactory.GetErrorHandler()) { }

        /// <summary>Creates a new instance of an Astronomy Picture of the Day client.</summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov. </param>
        /// <param name="errorHandler">The <see cref="IErrorHandler"/> to handle any errors with the request.</param>
        public ApodClient(string apiKey, IErrorHandler errorHandler)
            : this(apiKey, DefaultsFactory.GetHttpRequester(apiKey), errorHandler) { }

        /// <summary>Creates a new instance of an Astronomy Picture of the Day client.</summary>
        /// <param name="apiKey">Your API key from https://api.nasa.gov. </param>
        /// <param name="httpRequester">The <see cref="IHttpRequester"/> to use for interacting with the API.</param>
        /// <param name="errorHandler">The <see cref="IErrorHandler"/> to handle any errors with the request.</param>        
        public ApodClient(string apiKey, IHttpRequester httpRequester, IErrorHandler errorHandler)
        {
            _apiKey = apiKey;
            _httpRequester = httpRequester;
            _errorHandler = errorHandler;
            _jsonSerializerOptions = DefaultsFactory.GetJsonSerializerOptions();
        }

        /// <summary>Fetch the current Astronomy Picture of the Day.</summary>
        public async Task<ApodResponse> FetchApodAsync()
        {
            var responseMessage = await FetchApiDataAsync();
            return await GetOneApodResult(responseMessage);
        }

        /// <summary>Fetch the Astronomy Picture of the Day for a specific date.</summary>
        /// <param name="dateTime">The date to request the APOD for. Must be between June 16th 1995 and today's date.</param>
        public async Task<ApodResponse> FetchApodAsync(DateTime dateTime)
        {
            if (dateTime.Date == DateTime.Today) { return await FetchApodAsync(); }

            var apodResponse = _errorHandler.ValidateDate(dateTime);
            if (apodResponse.StatusCode != ApodStatusCode.OK) { return apodResponse; }

            var httpResponse = await _httpRequester.SendHttpRequestAsync(dateTime);

            // apodResponse = _errorHandler.ValidateHttpResponse(httpResponse);
            // return _httpResponseParser.Parse(httpResponse);

            //if (!DateIsInRange(date)) { throw new DateOutOfRangeException(nameof(date), date); }

            //var queryParameter = $"date={date.ToString("yyyy-MM-dd")}";
            //var responseMessage = await FetchApiDataAsync(queryParameter);
            //return await GetOneApodResult(responseMessage);
        }

        /// <summary>Fetch all the Astronomy Pictures of the Day between two dates.</summary>
        /// <param name="startDate">The start date. Must be between June 16th 1995 and today's date.</param>
        /// <param name="endDate">The end date. Must be between the <paramref name="startDate"/> and today's date. Defaults to <see cref="DateTime.Today"/>.</param>
        public async Task<ApodContent[]> FetchApodAsync(DateTime startDate, DateTime endDate = default)
        {
            if (!DateIsInRange(startDate)) { throw new DateOutOfRangeException(nameof(startDate), startDate); }
            if (endDate != default && !DateIsInRange(endDate)) { throw new DateOutOfRangeException(nameof(endDate), endDate); }
            if (DateTime.Compare(startDate, endDate) > 0) { throw new DateOutOfRangeException("The start date can not be after the end date."); }

            var startDateString = $"start_date={startDate.ToString("yyyy-MM-dd")}";

            var endDateString = endDate == default
                ? string.Empty
                : $"end_date={endDate.ToString("yyyy-MM-dd")}";

            var responseMessage = await FetchApiDataAsync(startDateString, endDateString);
            return await GetMultipleApodResults(responseMessage);
        }

        private async Task<HttpResponseMessage> FetchApiDataAsync(params string[] queryParameters)
        {
            var requestUri = BuildFullQueryString(queryParameters);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var date = DateTime.Today;
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); // Both EDT and EST, automatically checks

            return await _httpClient.SendAsync(requestMessage);
        }

        private async Task<ApodContent> GetOneApodResult(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonSerializerOptions);
                errorResponse.ThrowInformativeError();
            }

            return JsonSerializer.Deserialize<ApodContent>(responseContent, _jsonSerializerOptions);
        }

        private async Task<ApodContent[]> GetMultipleApodResults(HttpResponseMessage responseMessage)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            Console.WriteLine($"ERROR CONTENT: {responseContent}");

            if (!responseMessage.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonSerializerOptions);
                errorResponse.ThrowInformativeError();
            }

            return JsonSerializer.Deserialize<ApodContent[]>(responseContent, _jsonSerializerOptions);
        }

        private string BuildFullQueryString(string[] queryParameters)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder
                .Append(Constants.BaseUrl)
                .Append("?api_key=").Append(_apiKey);

            foreach (var parameter in queryParameters)
            {
                if (string.IsNullOrWhiteSpace(parameter)) { continue; }
                stringBuilder.Append("&");
                stringBuilder.Append(parameter);
            }

            return stringBuilder.ToString();
        }
    }
}
