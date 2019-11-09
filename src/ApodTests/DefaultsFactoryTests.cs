using Apod.Logic;
using Xunit;

namespace ApodTests
{
    public class DefaultsFactoryTests
    {
        [Theory]
        [InlineData("DEMO_KEY")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("123456789012345678901234567890")]
        public void GetHttpRequester_NotNull(string apiKey)
        {
            var result = DefaultsFactory.GetHttpRequester(apiKey);

            Assert.NotNull(result);
        }

        [Fact]
        public void GetErrorHandler_NotNull()
        {
            var result = DefaultsFactory.GetErrorHandler();

            Assert.NotNull(result);
        }

        [Fact]
        public void GetHttpResponseParser_NotNull()
        {
            var result = DefaultsFactory.GetHttpResponseParser();

            Assert.NotNull(result);
        }
    }
}
