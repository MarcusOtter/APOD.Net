using Apod;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApodTests
{
    public class ApodClientTests
    {
        private const string testApiKey = "DEMO_KEY";
        private readonly ApodClient _client;

        public ApodClientTests()
        {
            _client = new ApodClient(testApiKey);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_Today_NotNull()
        {
            var apodContent = await _client.FetchApodAsync();
            Assert.NotNull(apodContent);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_Today_IsDateTimeToday()
        {
            var apodContent = await _client.FetchApodAsync();
            var expected = DateTime.Today;

            Assert.Equal(expected, apodContent.Date);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_StartDateThrowsWhenOutOfRange()
        {
            var startDate = new DateTime(1995, 06, 10); // Random date out of range
            var endDate = new DateTime(1995, 06, 28); // Random date in range

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(startDate, endDate));
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_StartDateCorrectLowerBound()
        {
            var lowerBoundDate = new DateTime(1995, 06, 16); // First allowed date

            // The end date has to be the same date, because 1995-06-17 is not valid. This shouldn't affect the outcome.
            // In theory, the test should be from the lowerBoundDate to the lowerBoundDate + ~10 days.
            // In practice we can only test the same date since the api wasn't consistently uploading during the beginning.
            var endDate = new DateTime(1995, 06, 16);

            var expectedOutOfRange = lowerBoundDate.AddDays(-1);

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(expectedOutOfRange, endDate));

            var result = await _client.FetchApodAsync(lowerBoundDate, endDate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_UpperboundThrowsForOutOfRange()
        {
            var lowerBound = DateTime.Today.AddDays(-10); // Random date in range
            var upperBound = DateTime.Today.AddDays(3); // Random date out of range

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(lowerBound, upperBound));
        }

        

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_ReturnsCorrectAmountOfResults()
        {

        }

        // -- Ideas for future tests --
        // Same dates not null
        // Returns correct amount of results
        // end date before start date throws
        // upper and lower bounds are exactly like they should (upper bound still untested)
    }
}
