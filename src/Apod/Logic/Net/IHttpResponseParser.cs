using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Logic.Net
{
    public interface IHttpResponseParser
    {
        Task<ApodResponse> ParseSingleApodAsync(HttpResponseMessage httpResponse);
        Task<ApodResponse> ParseMultipleApodsAsync(HttpResponseMessage httpResponse);
    }
}
