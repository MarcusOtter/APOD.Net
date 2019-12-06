using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Logic.Net
{
    public interface IHttpRequester : IDisposable
    {
        ValueTask<HttpResponseMessage> SendHttpRequestAsync();
        ValueTask<HttpResponseMessage> SendHttpRequestAsync(DateTime dateTime);
        ValueTask<HttpResponseMessage> SendHttpRequestAsync(DateTime startDate, DateTime endDate = default);
        ValueTask<HttpResponseMessage> SendHttpRequestAsync(int count);
    }
}
