using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Net
{
    public interface IHttpRequester
    {
        Task<HttpResponseMessage> SendHttpRequestAsync();
        Task<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime);
        Task<HttpResponseMessage> SendHttpRequestAsync(DateTime startDate, DateTime endDate = default);
        Task<HttpResponseMessage> SendHttpRequestAsync(int count);
    }
}
