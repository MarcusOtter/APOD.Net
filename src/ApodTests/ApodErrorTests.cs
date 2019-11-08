using Xunit;
using Apod;

namespace ApodTests
{
    public class ApodErrorTests
    {
        [Theory]
        [InlineData(ApodErrorCode.ApiKeyInvalid, "Example error message.")]
        [InlineData(ApodErrorCode.None, "No error!")]
        [InlineData(ApodErrorCode.BadRequest, "")]
        [InlineData(ApodErrorCode.InternalServiceError, null)]
        public void ToApodResponse_CorrectApodResponse(ApodErrorCode errorCode, string errorMessage)
        {
            var error = new ApodError(errorCode, errorMessage);

            var expected = new ApodResponse(ApodStatusCode.Error, error: error);

            var actual = error.ToApodResponse();

            Assert.Equal(expected.StatusCode, actual.StatusCode);
            Assert.Equal(expected.Error.ErrorCode, actual.Error.ErrorCode);
            Assert.Equal(expected.Error.ErrorMessage, actual.Error.ErrorMessage);
            Assert.Null(actual.Content);
            Assert.Null(actual.AllContent);
        }
    }
}
