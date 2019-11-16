using Apod.Logic.Net;
using System;
using Xunit;

namespace ApodTests
{
    public class ApodUriBuilderTests
    {
        [Theory]
        [InlineData("test", "https://api.nasa.gov/planetary/apod?api_key=test")]
        [InlineData("exampleKey", "https://api.nasa.gov/planetary/apod?api_key=exampleKey")]
        [InlineData("", "https://api.nasa.gov/planetary/apod?api_key=")]
        public void GetApodUri_CorrectApiKey(string apiKey, string expected)
        {
            var uriBuilder = new ApodUriBuilder(apiKey);

            var actual = uriBuilder.GetApodUri();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("exampleKey", "https://api.nasa.gov/planetary/apod/v2", "https://api.nasa.gov/planetary/apod/v2?api_key=exampleKey")]
        [InlineData("exampleKey", "https://example.com", "https://example.com?api_key=exampleKey")]
        [InlineData("exampleKey", "", "?api_key=exampleKey")]
        public void GetApodUri_CorrectBaseUri(string apiKey, string baseUri, string expected)
        {
            var uriBuilder = new ApodUriBuilder(apiKey, baseUri);

            var actual = uriBuilder.GetApodUri();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("yyyy-dd-MM", "https://api.nasa.gov/planetary/apod?api_key=exampleKey&date=2014-28-01")]
        [InlineData("ddMMyyyy", "https://api.nasa.gov/planetary/apod?api_key=exampleKey&date=28012014")]
        [InlineData("MM_dd_yy", "https://api.nasa.gov/planetary/apod?api_key=exampleKey&date=01_28_14")]
        public void GetApodUri_Date_CorrectCustomDateFormat(string dateFormat, string expected)
        {
            var date = new DateTime(2014, 01, 28);
            var uriBuilder = new ApodUriBuilder("exampleKey", dateFormat: dateFormat);

            var actual = uriBuilder.GetApodUri(date);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("yyyy-dd-MM", "https://api.nasa.gov/planetary/apod?api_key=exampleKey&start_date=2000-29-12&end_date=2001-04-01")]
        [InlineData("ddMMyyyy", "https://api.nasa.gov/planetary/apod?api_key=exampleKey&start_date=29122000&end_date=04012001")]
        [InlineData("dd-MM_yyyy", "https://api.nasa.gov/planetary/apod?api_key=exampleKey&start_date=29-12_2000&end_date=04-01_2001")]
        public void GetApodUri_DateRange_CorrectCustomDateFormat(string dateFormat, string expected)
        {
            var startDate = new DateTime(2000, 12, 29);
            var endDate = new DateTime(2001, 01, 04);
            var uriBuilder = new ApodUriBuilder("exampleKey", dateFormat: dateFormat);

            var actual = uriBuilder.GetApodUri(startDate, endDate);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetApodUri_Today_CorrectUri()
        {
            var uriBuilder = new ApodUriBuilder("1234567890");
            var expected = "https://api.nasa.gov/planetary/apod?api_key=1234567890";

            var actual = uriBuilder.GetApodUri();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetApodUri_Date_CorrectUri()
        {
            var date = new DateTime(2018, 05, 09);
            var uriBuilder = new ApodUriBuilder("exampleKey");

            var expected = "https://api.nasa.gov/planetary/apod?api_key=exampleKey&date=2018-05-09";

            var actual = uriBuilder.GetApodUri(date);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetApodUri_DateRange_CorrectUri()
        {
            var startDate = new DateTime(1998, 03, 24);
            var endDate = new DateTime(1998, 04, 02);
            var uriBuilder = new ApodUriBuilder("exampleKey");

            var expected = "https://api.nasa.gov/planetary/apod?api_key=exampleKey&start_date=1998-03-24&end_date=1998-04-02";

            var actual = uriBuilder.GetApodUri(startDate, endDate);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetApodUri_DateRange_DefaultEndDate_CorrectUri()
        {
            var startDate = new DateTime(2019, 10, 29);

            var uriBuilder = new ApodUriBuilder("exampleKey");

            var expected = "https://api.nasa.gov/planetary/apod?api_key=exampleKey&start_date=2019-10-29";

            var actual = uriBuilder.GetApodUri(startDate: startDate);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(34, "https://api.nasa.gov/planetary/apod?api_key=exampleKey&count=34")]
        [InlineData(0, "https://api.nasa.gov/planetary/apod?api_key=exampleKey&count=0")]
        [InlineData(-3, "https://api.nasa.gov/planetary/apod?api_key=exampleKey&count=-3")]
        public void GetApodUri_Count_CorrectUri(int count, string expected)
        {
            var uriBuilder = new ApodUriBuilder("exampleKey");

            var actual = uriBuilder.GetApodUri(count);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetApodUri_SameResultEveryTime()
        {
            var uriBuilder = new ApodUriBuilder("exampleKey");

            var expected = "https://api.nasa.gov/planetary/apod?api_key=exampleKey";

            // Run 3 times and make sure the result doesn't change
            for (int i = 0; i < 3; i++)
            {
                var actual = uriBuilder.GetApodUri();
                Assert.Equal(expected, actual);
            }            
        }
    }
}
