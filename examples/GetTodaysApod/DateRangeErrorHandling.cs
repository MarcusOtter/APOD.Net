using System;
using System.Threading.Tasks;
using Apod;

namespace GetTodaysApod
{
    public static class DateRangeErrorHandling
    {
        public static async Task Run()
        {
            Console.WriteLine("This program gives you the links to all the APODs between two dates.");

            ApodError error = null;
            var client = new ApodClient();
            do
            {
                Console.WriteLine("Enter the first date");
                var startDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter the last date");
                var endDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Getting results...");
                var response = await client.FetchApodAsync(startDate, endDate);

                if (response.StatusCode is ApodStatusCode.OK)
                {
                    foreach (var apod in response.AllContent)
                    {
                        var dateString = apod.Date.ToShortDateString();
                        Console.WriteLine($"{dateString}: {apod.ContentUrl}");
                    }

                    client.Dispose();
                    return;
                }

                error = response.Error;
                switch (error.ErrorCode)
                {
                    case ApodErrorCode.DateOutOfRange:
                        Console.WriteLine(error.ErrorMessage);
                        break;

                    case ApodErrorCode.Timeout:
                        Console.WriteLine("Try with dates that are closer together.");
                        break;

                    default:
                        Console.WriteLine("An error occured. Try again.");
                        break;
                }
            }
            while (error != null);

            client.Dispose();
        }
    }
}
