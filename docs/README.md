<p align="center">
  <a href="#"><img alt="A C# wrapper for NASA's Astronomy Picture of the Day API." src="img/banner.jpg" /></a>
  <em><a href="https://www.nasa.gov/image-feature/revealing-the-milky-way-s-center" target="_blank">Image Credit: NASA, JPL-Caltech, Susan Stolovy (SSC/Caltech) et al.</a></em><br><br>
  <img src="https://github.com/LeMorrow/APOD.Net/workflows/Build/badge.svg" alt="Build status">
</p>

# APOD.Net
:warning: *This project is still in it's infancy - please understand that many features will be changed in the final product.* :warning:

APOD.Net is a .NET library used to asynchronously interface with [NASA's Astronomy Picture of the Day API](https://api.nasa.gov/) (APOD). The API features a  different image or photograph of our universe each day, along with a brief explanation written by a professional astronomer.

APOD.Net allows you to do many things with the API, for example:
* Get a random Astronomy Picture of the Day
* Get all the Astronomy Pictures of the Day between two dates
* Get the current Astronomy Picture of the Day
* Get the an Astronomy Picture of the Day for a specific date

## Planned
* Test coverage reports on every push and pull request, using [Coveralls](https://coveralls.io/) and [Coverlet](https://github.com/tonerdo/coverlet).
* Documentation using [docfx](https://github.com/dotnet/docfx).

## Example usage
See all example projects [here](../src/ExampleUsage/).
```cs
using System;
using System.Threading.Tasks;
using Apod;

namespace ApodExample
{
    public class Program
    {
        private static async Task Main()
        {
            var apodClient = new ApodClient("YOUR_API_KEY_HERE");
            var apod = await apodClient.FetchApodAsync();

            Console.WriteLine(apod.Title);
            Console.WriteLine(apod.Explanation);
            Console.WriteLine(apod.ContentUrlHD);
        }
    }
}
```
