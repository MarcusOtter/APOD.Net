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
            var apodResponse = await apodClient.FetchApodAsync(new DateTime(1995, 06, 13));

            if (apodResponse.StatusCode != ApodStatusCode.OK)
            {
                var error = apodResponse.Error;
                Console.WriteLine($"An error occured.\n{error.ErrorCode}: {error.ErrorMessage}");
                return;
            }

            Console.WriteLine(apodResponse.Content[0].Title);
            Console.WriteLine(apodResponse.Content[0].Explanation);
            Console.WriteLine(apodResponse.Content[0].ContentUrlHD);
        }
    }
}
