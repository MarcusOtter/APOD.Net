using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Apod;

namespace GetTodaysApod
{
    public static class AllContentExample
    {
        public static async Task Run()
        {
            using var client = new ApodClient();
            var response1 = await client.FetchApodAsync(new DateTime(2000, 01, 01));
            var response2 = await client.FetchApodAsync(new DateTime(2019, 02, 22), new DateTime(2019, 02, 24));
            var response3 = await client.FetchApodAsync(5);

            Console.WriteLine(response1.AllContent?.Length);
            Console.WriteLine(response2.AllContent?.Length);
            Console.WriteLine(response3.AllContent?.Length);
        }
    }
}
