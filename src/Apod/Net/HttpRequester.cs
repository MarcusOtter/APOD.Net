using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Apod.Net
{
    internal class HttpRequester : IHttpRequester
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public HttpRequester(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }
    }
}
