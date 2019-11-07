using System;
using System.Threading.Tasks;
using Apod;

namespace ApodExample
{
    public class Program
    {
        private static async Task Main()
        {
            var apodClient = new ApodClient();
            var apodResponse = await apodClient.FetchApodAsync(DateTime.Today.AddDays(-2), DateTime.Today);

            if (apodResponse.StatusCode != ApodStatusCode.OK)
            {
                Console.WriteLine($"An error occured.\n{apodResponse.Error.ErrorCode}: {apodResponse.Error.ErrorMessage}");
                return;
            }

            foreach (var apod in apodResponse.Content)
            {
                Console.WriteLine(Array.IndexOf(apodResponse.Content, apod));
                Console.WriteLine(apod.Title);
                Console.WriteLine(apod.Explanation);
                Console.WriteLine(apod.ContentUrlHD);
                Console.WriteLine();
            }
        }
    }
}
