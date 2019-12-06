using System;
using System.Threading.Tasks;

namespace GetTodaysApod
{
    public class Program
    {
        public static async Task Main()
        {
            using var imageDownloader = new ImageDownloader();

            var folder = "images";
            var fileName = "test";
            var url = "https://cdn.discordapp.com/attachments/458291463663386646/592779619212460054/Screenshot_20190624-201411.jpg?query&with.dots";

            await imageDownloader.DownloadImageAsync(folder, fileName, new Uri(url));
        }
    }
}
