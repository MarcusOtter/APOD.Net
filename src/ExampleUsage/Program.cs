using System;
using System.Threading.Tasks;
using Apod;

namespace ApodExample
{
    public class Program
    {
        private static async Task Main()
        {
            var apodClient = new ApodClient("DEMO_KEY");
            var apod = await apodClient.FetchApodAsync();

            Console.WriteLine(apod.Title);
            Console.WriteLine(apod.Explanation);
            Console.WriteLine(apod.ContentUrlHD);
        }
    }
}
