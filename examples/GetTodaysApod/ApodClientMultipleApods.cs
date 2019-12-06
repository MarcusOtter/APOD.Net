using System;
using System.Threading.Tasks;
using Apod;

namespace GetTodaysApod
{
    public static class ApodClientMultipleApods
    {
        public static async Task Run() 
        {
            using var client = new ApodClient();

            var startDate = new DateTime(2008, 10, 29);
            var endDate = new DateTime(2008, 11, 02);
            var response = await client.FetchApodAsync(startDate, endDate);

            if (response.StatusCode != ApodStatusCode.OK) { return; }

            foreach (var apod in response.AllContent)
            {
                Console.WriteLine($"{apod.Date}: {apod.Title}");
            }
        }
    }
}
