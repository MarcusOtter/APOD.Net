using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Apod.Net
{
    internal class HttpRequester : IHttpRequester
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public HttpRequester(string apiKey, HttpClient httpClient)
        {
            _apiKey = apiKey;
            _httpClient = httpClient;
        }
    }
}
