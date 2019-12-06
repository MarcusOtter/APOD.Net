using Apod;
using System;
using System.Threading.Tasks;

namespace GetTodaysApod
{
    public static class WriteLineIfError
    {
        public static async Task Run()
        {
            var client = new ApodClient();
            var response = await client.FetchApodAsync();

            if (response.StatusCode != ApodStatusCode.OK)
            {
                Console.WriteLine(response.Error.ErrorMessage);
            }

            client.Dispose();
        }
    }
}
