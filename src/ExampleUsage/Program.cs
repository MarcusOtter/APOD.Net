using System;
using System.Threading.Tasks;
using Apod;

namespace ApodExample
{
    public class Program
    {
        private static async Task Main()
        {
            // Set up the client using your own API key (recommended)
            var apodClient = new ApodClient();

            // Ask for the Astronomy Pictures of the Day between October 29, 2008 and November 2, 2008
            var apodResponse = await apodClient.FetchApodAsync(new DateTime(2008, 10, 29), new DateTime(2008, 11, 02));

            // If an error occurs, write the error code and error message to the console and then stop executing
            if (apodResponse.StatusCode != ApodStatusCode.OK) 
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(apodResponse.Error.ErrorCode);
                Console.WriteLine(apodResponse.Error.ErrorMessage);
                return; 
            }

            // Write the title of the most recent APOD (from the results) to the console
            Console.WriteLine($"The title of the most recent APOD is \"{apodResponse.Content.Title}\".");

            // Iterate through every single returned APOD and write their dates and titles to the console
            foreach (var apod in apodResponse.AllContent)
            {
                var date = apod.Date.ToString("MMMM dd, yyyy");
                Console.WriteLine($"- {date}: \"{apod.Title}\".");
            }
        }
    }
}