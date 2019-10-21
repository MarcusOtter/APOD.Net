using System.Text.Json.Serialization;

namespace Apod
{
    internal class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }
}
