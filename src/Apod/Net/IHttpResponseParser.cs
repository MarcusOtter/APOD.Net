using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Net
{
    public interface IHttpResponseParser
    {
        Task<ApodResponse> ParseSingleApodAsync(HttpResponseMessage httpResponse);
        Task<ApodResponse> ParseMultipleApodAsync(HttpResponseMessage httpResponse);
    }
}
