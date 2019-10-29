using Apod;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApodTests
{
    public class ApodClientTests
    {
        private readonly string _testApiKey = Environment.GetEnvironmentVariable("NASA_API_KEY");

        private readonly DateTime _firstAllowedDate = new DateTime(1995, 06, 16);
        private readonly DateTime _lastAllowedDate = DateTime.Today;

        private readonly ApodClient _client;

        public ApodClientTests()
        {
            _client = new ApodClient(_testApiKey);
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
        public async Task ApodClient_FetchApodAsync_SpecificDate_NotNull()
        {
            var date = new DateTime(2008, 10, 29); // Random date in range
            var apodContent = await _client.FetchApodAsync(date);
            Assert.NotNull(apodContent);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_SpecificDate_CorrectLowerBound()
        {
            var expectedOutOfRange = _firstAllowedDate.AddDays(-1);
            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(expectedOutOfRange));

            var result = await _client.FetchApodAsync(_firstAllowedDate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_SpecificDate_CorrectUpperBound()
        {
            var expectedOutOfRange = _lastAllowedDate.AddDays(1);
            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(expectedOutOfRange));

            var result = await _client.FetchApodAsync(_lastAllowedDate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_SpecificDate_ThrowsForDateOutOfRange()
        {
            var dateOutOfRange1 = new DateTime(1993, 06, 14);
            var dateOutOfRange2 = _lastAllowedDate.AddDays(391);

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(dateOutOfRange1));
            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(dateOutOfRange2));
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_StartDateCorrectLowerBound()
        {
            // The end date has to be the same date as the first allowed date, because 1995-06-17 is not valid. This shouldn't affect the outcome.
            // Ultimately the test should be from the _firstAllowedDate to the _firstAllowedDate += about 5 days.
            // In practice we can only test the same date since the api wasn't consistent with daily pictures during the beginning.
            var endDate = _firstAllowedDate;

            var expectedOutOfRange = _firstAllowedDate.AddDays(-1);
            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(expectedOutOfRange, endDate));

            var result = await _client.FetchApodAsync(_firstAllowedDate, endDate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_EndDateCorrectUpperBound()
        {
            var startDate = _lastAllowedDate.AddDays(-5); // Random date in range

            var expectedOutOfRange = _lastAllowedDate.AddDays(1);
            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(startDate, expectedOutOfRange));

            var result = await _client.FetchApodAsync(startDate, _lastAllowedDate);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_StartDateThrowsWhenOutOfRange()
        {
            var startDate = new DateTime(1995, 06, 10); // Random date out of range
            var endDate = new DateTime(1995, 06, 28); // Random date in range

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(startDate, endDate));
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_EndDateThrowsForOutOfRange()
        {
            var startDate = DateTime.Today.AddDays(-10); // Random date in range
            var endDate = DateTime.Today.AddDays(3); // Random date out of range

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(startDate, endDate));
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_EndDateBeforeStartDateThrows()
        {
            var startDate = new DateTime(2007, 10, 12); // Random date in range
            var endDate = new DateTime(2007, 10, 04); // Random date before the startDate

            await Assert.ThrowsAsync<DateOutOfRangeException>(async () => await _client.FetchApodAsync(startDate, endDate));
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_SameDateReturnsOneResult()
        {
            var startDate = new DateTime(2012, 12, 12); // Random date in range
            var endDate = startDate;

            var result = await _client.FetchApodAsync(startDate, endDate);

            Assert.Single(result);
        }

        [Fact]
        public async Task ApodClient_FetchApodAsync_DateSpan_ReturnsCorrectAmountOfResults()
        {
            var startDate = DateTime.Today.AddDays(-2);
            var endDate = DateTime.Today;

            var result = await _client.FetchApodAsync(startDate, endDate);

            // If the date is 2019-10-27, the start date is 2019-10-27 and the end date is 2019-10-25
            // There are three expected results, 2019-10-25, 2019-10-26 and 2019-10-27. 
            const int expectedResults = 3;

            Assert.Equal(expectedResults, result.Length);
        }
    }
}
