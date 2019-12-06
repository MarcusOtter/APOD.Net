using System.Net.Http;
using System.Threading.Tasks;

namespace Apod.Logic.Net
{
    public interface IHttpResponseParser
    {
        ValueTask<ApodResponse> ParseSingleApodAsync(HttpResponseMessage httpResponse);
        ValueTask<ApodResponse> ParseMultipleApodsAsync(HttpResponseMessage httpResponse);
    }
}
