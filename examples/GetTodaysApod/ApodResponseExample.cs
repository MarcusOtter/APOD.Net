using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Apod;

namespace GetTodaysApod
{
    public static class ApodResponseExample
    {
        public static async Task Run()
        {
            var client = new ApodClient();
            var response = await client.FetchApodAsync();

            if (response.StatusCode != ApodStatusCode.OK)
            {
                var error = response.Error;
                Console.WriteLine(error.ErrorMessage);
                return;
            }

            Console.WriteLine("");

            client.Dispose();
        }
    }
}
