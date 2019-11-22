<p align="center">
  <a href="#"><img alt="APOD.Net, an unofficial " src="docs/images/banner.jpg" /></a>
  <em><a href="https://www.nasa.gov/image-feature/revealing-the-milky-way-s-center">Image Credit: NASA, JPL-Caltech, Susan Stolovy (SSC/Caltech) et al.</a></em><br><br>
  <a href="https://github.com/LeMorrow/APOD.Net/actions?query=workflow%3ABuild"><img src="https://github.com/LeMorrow/APOD.Net/workflows/Build/badge.svg" alt="Build status"></a>
  <a href='https://coveralls.io/github/LeMorrow/APOD.Net?branch=master'><img src='https://coveralls.io/repos/github/LeMorrow/APOD.Net/badge.svg?branch=master&service=github' alt='Coverage Status' /></a>
  <a href="https://github.com/LeMorrow/APOD.Net/blob/master/LICENSE"><img src="https://img.shields.io/badge/License-MIT-blue.svg" alt="MIT License badge"></a>
</p>

# APOD.Net
:warning: Warning :warning:<br>
This repository is currently undergoing big changes and may not work as expected.

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
More example usages will eventually be found [here](examples/).

### Simple example
```cs
// Set up the client using the default example api key ("DEMO_KEY")
var apodClient = new ApodClient();

// Fetch the current Astronomy Picture of the Day
var apodResponse = await apodClient.FetchApodAsync();

// If an error occurs, stop executing
if (apodResponse.StatusCode != ApodStatusCode.OK) { return; }

// Store the information about the Astronomy Picture of the Day
var apod = apodResponse.Content;

Console.WriteLine(apod.Title);
Console.WriteLine(apod.Explanation);
Console.WriteLine(apod.ContentUrl);
``` 
<details>
<summary>See output</summary>
<em>Example from November 6, 2019</em>
<p>

```
21st Century M101
One of the last entries in Charles Messier's famous catalog, big, beautiful spiral galaxy M101 is definitely not one of the least. About 170,000 light-years across, this galaxy is enormous, almost twice the size of our own Milky Way Galaxy. M101 was also one of the original spiral nebulae observed with Lord Rosse's large 19th century telescope, the Leviathan of Parsonstown. In contrast, this multiwavelength view of the large island universe is a composite of images recorded by space-based telescopes in the 21st century. Color coded from X-rays to infrared wavelengths (high to low energies), the image data was taken from the Chandra X-ray Observatory (purple), the Galaxy Evolution Explorer (blue), Hubble Space Telescope(yellow), and the Spitzer Space Telescope(red). While the X-ray data trace the location of multimillion degree gas around M101's exploded stars and neutron star and black hole binary star systems, the lower energy data follow the stars and dust that define M101's grand spiral arms. Also known as the Pinwheel Galaxy, M101 lies within the boundaries of the northern constellation Ursa Major, about 25 million light-years away.
https://apod.nasa.gov/apod/image/1911/M101_nasaMultiW1024.jpg
```

</p>
</details>

### Advanced example
```cs
// Set up the client using your own API key (recommended)
var apodClient = new ApodClient("YOUR_API_KEY_HERE");

// Ask for the Astronomy Pictures of the Day between October 29, 2008 and November 2, 2008
var startDate = new DateTime(2008, 10, 29);
var endDate = new DateTime(2008, 11, 02);
var apodResponse = await apodClient.FetchApodAsync(startDate, endDate);

// If an error occurs, write the error code and error message to the console and then stop executing
if (apodResponse.StatusCode != ApodStatusCode.OK) 
{
    Console.WriteLine("An error occured.");
    Console.WriteLine(apodResponse.Error.ErrorCode);
    Console.WriteLine(apodResponse.Error.ErrorMessage);
    return; 
}

// Iterate through every single returned APOD and write their dates and titles to the console
foreach (var apod in apodResponse.AllContent)
{
    var date = apod.Date.ToString("MMMM d, yyyy");
    Console.WriteLine($"- {date}: \"{apod.Title}\".");
}

// Release the unmanaged resources and disposes of the managed resources used by the System.Net.Http.HttpMessageInvoker.
apodClient.Dispose();
```
<details>
<summary>See output</summary>
<p>

```
The title of the most recent APOD is "Spicules: Jets on the Sun".
- October 29, 2008: "Mirach's Ghost".
- October 30, 2008: "Haunting the Cepheus Flare".
- October 31, 2008: "A Witch by Starlight".
- November 1, 2008: "A Spectre in the Eastern Veil".
- November 2, 2008: "Spicules: Jets on the Sun".
```

</p>
</details>
