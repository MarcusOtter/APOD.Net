using System;
using System.Text;
using Xunit;
using Apod.Logic.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Apod;
using Apod.Logic.Net.Dtos;

namespace ApodTests
{
    public class HttpResponseParserTests
    {
        private string _singleApodExampleContent => @"{""copyright"":""R Jay Gabany"",""date"":""2019-11-16"",""explanation"":""Grand tidal streams of stars seem to surround galaxy NGC 5907. The arcing structures form tenuous loops extending more than 150,000 light-years from the narrow, edge-on spiral, also known as the Splinter or Knife Edge Galaxy.Recorded only in very deep exposures, the streams likely represent the ghostly trail of a dwarf galaxy - debris left along the orbit of a smaller satellite galaxy that was gradually torn apart and merged with NGC 5907 over four billion years ago.Ultimately this remarkable discovery image, from a small robotic observatory in New Mexico, supports the cosmological scenario in which large spiral galaxies, including our own Milky Way, were formed by the accretion of smaller ones. NGC 5907 lies about 40 million light-years distant in the northern constellation Draco."",""hdurl"":""https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl.jpg"",""media_type"":""image"",""service_version"":""v1"",""title"":""The Star Streams of NGC 5907"",""url"":""https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl1024.jpg""}";
        private string _multipleApodExampleContent => @"[{""date"":""2006-11-07"",""explanation"":""Janus is one of the stranger moons of Saturn.First, Janus travels in an unusual orbit around Saturn where it periodically trades places with its sister moon Epimetheus, which typically orbits about 50 kilometers away.Janus, although slightly larger than Epimetheus, is potato-shaped and has a largest diameter of about 190 kilometers.Next, Janus is covered with large craters but strangely appears to lack small craters.One possible reason for this is a fine dust that might cover the small moon, a surface also hypothesized for Pandora and Telesto.Pictured above, Janus was captured in front of the cloud tops of Saturn in late September."",""hdurl"":""https://apod.nasa.gov/apod/image/0611/janus_cassini_big.jpg"",""media_type"":""image"",""service_version"":""v1"",""title"":""Janus: Potato Shaped Moon of Saturn"",""url"":""https://apod.nasa.gov/apod/image/0611/janus_cassini.jpg""},{""copyright"":""Misti\nMountain Observatory"",""date"":""2005-07-28"",""explanation"":""Ghostly in appearance, Abell 39 is a remarkably simple, spherical nebula about five light-years across. Well within our own Milky Way galaxy, the cosmic sphere is roughly 7,000 light-years distant toward the constellation Hercules. Abell 39 is a planetary nebula, formed as a once sun-like star's outer atmosphere was expelled over a period of thousands of years. Still visible, the nebula's central star is evolving into a hot white dwarf. Although faint, the nebula's simple geometry has proven to be a boon to astronomers exploring the chemical abundances and life cycles of stars. In this deep image recorded under dark night skies, very distant background galaxies can be found -- some visible right through the nebula itself."",""hdurl"":""https://apod.nasa.gov/apod/image/0507/abell39_misti_f.jpg"",""media_type"":""image"",""service_version"":""v1"",""title"":""Spherical Planetary Nebula Abell 39"",""url"":""https://apod.nasa.gov/apod/image/0507/abell39_misti_c50.jpg""}]";

        [Fact]
        public async Task ParseSingleApodAsync_HttpResponseIsDisposed()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_singleApodExampleContent, Encoding.UTF8, "application/json")
            };

            await httpResponseParser.ParseSingleApodAsync(input);

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await input.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ParseSingleApodAsync_HasOneApod()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_singleApodExampleContent, Encoding.UTF8, "application/json")
            };

            var result = await httpResponseParser.ParseSingleApodAsync(input);

            Assert.Single(result.AllContent);
        }

        [Fact]
        public async Task ParseSingleApodAsync_CorrectContent()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_singleApodExampleContent, Encoding.UTF8, "application/json")
            };

            var expected = new ApodContent()
            {
                Copyright = "R Jay Gabany",
                Date = new DateTime(2019, 11, 16),
                Explanation = "Grand tidal streams of stars seem to surround galaxy NGC 5907. The arcing structures form tenuous loops extending more than 150,000 light-years from the narrow, edge-on spiral, also known as the Splinter or Knife Edge Galaxy.Recorded only in very deep exposures, the streams likely represent the ghostly trail of a dwarf galaxy - debris left along the orbit of a smaller satellite galaxy that was gradually torn apart and merged with NGC 5907 over four billion years ago.Ultimately this remarkable discovery image, from a small robotic observatory in New Mexico, supports the cosmological scenario in which large spiral galaxies, including our own Milky Way, were formed by the accretion of smaller ones. NGC 5907 lies about 40 million light-years distant in the northern constellation Draco.",
                ContentUrlHD = "https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl.jpg",
                MediaType = MediaType.Image,
                ServiceVersion = "v1",
                Title = "The Star Streams of NGC 5907",
                ContentUrl = "https://apod.nasa.gov/apod/image/1911/ngc5907_gabany_rcl1024.jpg"
            };

            var actual = (await httpResponseParser.ParseSingleApodAsync(input)).Content;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ParseSingleApodAsync_CorrectStatusCode()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_singleApodExampleContent, Encoding.UTF8, "application/json")
            };

            var expected = ApodStatusCode.OK;
            var actual = (await httpResponseParser.ParseSingleApodAsync(input)).StatusCode;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ParseMultipleApodsAsync_HttpResponseIsDisposed()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_multipleApodExampleContent, Encoding.UTF8, "application/json")
            };

            await httpResponseParser.ParseMultipleApodsAsync(input);

            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await input.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ParseMultipleApodsAsync_CorrectAmountOfApods()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_multipleApodExampleContent, Encoding.UTF8, "application/json")
            };

            var result = await httpResponseParser.ParseMultipleApodsAsync(input);

            var expectedLength = 2; // _multipleApodExampleContent contains 2 apods.
            var actualLength = result.AllContent.Length;

            Assert.Equal(expectedLength, actualLength);
        }

        [Fact]
        public async Task ParseMultipleApodsAsync_CorrectContent()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_multipleApodExampleContent, Encoding.UTF8, "application/json")
            };

            var expected = new ApodContent[]
            {
                new ApodContent()
                {
                    Date = new DateTime(2006, 11, 07),
                    Explanation = "Janus is one of the stranger moons of Saturn.First, Janus travels in an unusual orbit around Saturn where it periodically trades places with its sister moon Epimetheus, which typically orbits about 50 kilometers away.Janus, although slightly larger than Epimetheus, is potato-shaped and has a largest diameter of about 190 kilometers.Next, Janus is covered with large craters but strangely appears to lack small craters.One possible reason for this is a fine dust that might cover the small moon, a surface also hypothesized for Pandora and Telesto.Pictured above, Janus was captured in front of the cloud tops of Saturn in late September.",
                    ContentUrlHD = "https://apod.nasa.gov/apod/image/0611/janus_cassini_big.jpg",
                    MediaType = MediaType.Image,
                    ServiceVersion = "v1",
                    Title = "Janus: Potato Shaped Moon of Saturn",
                    ContentUrl = "https://apod.nasa.gov/apod/image/0611/janus_cassini.jpg"
                },
                new ApodContent()
                {
                    Copyright = "Misti\nMountain Observatory",
                    Date = new DateTime(2005, 07, 28),
                    Explanation = "Ghostly in appearance, Abell 39 is a remarkably simple, spherical nebula about five light-years across. Well within our own Milky Way galaxy, the cosmic sphere is roughly 7,000 light-years distant toward the constellation Hercules. Abell 39 is a planetary nebula, formed as a once sun-like star's outer atmosphere was expelled over a period of thousands of years. Still visible, the nebula's central star is evolving into a hot white dwarf. Although faint, the nebula's simple geometry has proven to be a boon to astronomers exploring the chemical abundances and life cycles of stars. In this deep image recorded under dark night skies, very distant background galaxies can be found -- some visible right through the nebula itself.",
                    ContentUrlHD = "https://apod.nasa.gov/apod/image/0507/abell39_misti_f.jpg",
                    MediaType = MediaType.Image,
                    ServiceVersion = "v1",
                    Title = "Spherical Planetary Nebula Abell 39",
                    ContentUrl = "https://apod.nasa.gov/apod/image/0507/abell39_misti_c50.jpg"
                }
            };

            var actual = (await httpResponseParser.ParseMultipleApodsAsync(input)).AllContent;

            Assert.Equal(expected, actual);
            Assert.Equal(expected[1], actual[1]);
        }

        [Fact]
        public async Task ParseMultipleApodsAsync_CorrectStatusCode()
        {
            var httpResponseParser = new HttpResponseParser();
            var input = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_multipleApodExampleContent, Encoding.UTF8, "application/json")
            };

            var expected = ApodStatusCode.OK;
            var actual = (await httpResponseParser.ParseMultipleApodsAsync(input)).StatusCode;

            Assert.Equal(expected, actual);
        }
    }
}
