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
            var apodResponse = await apodClient.FetchApodAsync(new DateTime(2000, 06, 04));

            if (apodResponse.StatusCode == ApodStatusCode.OK)
            {
                Console.WriteLine(apodResponse.Content[0].Title);
                Console.WriteLine(apodResponse.Content[0].Explanation);
                Console.WriteLine(apodResponse.Content[0].ContentUrlHD);
            }
            else
            {
                Console.WriteLine("The client got notified that an error occured.");
                Console.WriteLine($"Error code: {apodResponse.Error.ErrorCode}");
                Console.WriteLine($"Error message: {apodResponse.Error.ErrorMessage}");
            }
        }
    }
}
