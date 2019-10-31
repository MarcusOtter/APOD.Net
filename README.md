<p align="center">
  <a href="#"><img alt="APOD.Net, an unofficial " src="docs/img/banner.jpg" /></a>
  <em><a href="https://www.nasa.gov/image-feature/revealing-the-milky-way-s-center">Image Credit: NASA, JPL-Caltech, Susan Stolovy (SSC/Caltech) et al.</a></em><br><br>
  <a href="https://github.com/LeMorrow/APOD.Net/actions?query=workflow%3ABuild"><img src="https://github.com/LeMorrow/APOD.Net/workflows/Build/badge.svg" alt="Build status"></a>
  <a href='https://coveralls.io/github/LeMorrow/APOD.Net?branch=master'><img src='https://coveralls.io/repos/github/LeMorrow/APOD.Net/badge.svg?branch=master' alt='Coverage Status' /></a>
  <a href="https://github.com/LeMorrow/APOD.Net/blob/master/LICENSE"><img src="https://img.shields.io/badge/License-MIT-blue.svg" alt="MIT License badge"></a>
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
* Documentation using [docfx](https://github.com/dotnet/docfx).
* Nuget package & auto deployment using Github Actions

## Example usage
See all example projects [here](src/ExampleUsage/).
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
