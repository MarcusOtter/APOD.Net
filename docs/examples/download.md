# Download all APODs between two dates
This example shows how to download all APOD images between two dates to a folder on your computer. Intermediate knowledge in C# is assumed.

The project targets .NET Core 3.0 but will work with any platform version that [supports .NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) (small changes may be required for different target frameworks).

## Creating the project
Start by creating a .NET Core (or .NET framework) Console App.
Read [this quickstart guide](https://docs.microsoft.com/en-us/visualstudio/ide/quickstart-csharp-console?view=vs-2019) if you are unsure how to do this.

## Installing APOD.Net
There are multiple ways of installing a NuGet package. 
See [Adding APOD.Net to your project](https://github.com/LeMorrow/APOD.Net#-adding-apodnet-to-your-project) in the README for other examples.

1. Open your new project in Visual Studio and open the Package Manager Console (`Tools` > `NuGet Package Manager` > `Package Manager Console`).
2. In the Package Manager Console window, write the following command to add the latest version of APOD.Net to your project.
    ```powershell
    Install-Package APOD.Net
    ```
<br>

Once the install finishes you are ready to start using APOD.Net.

## Implementation
### Setup
To get started with APOD.Net and to verify the installation, add the [using directive](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive) for the `Apod` namespace in your `Program.cs`.
```cs
using Apod;
```
If you get errors, run [`dotnet restore`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-restore), restart visual studio and try again. If it still doesn't work, feel free to [open an issue](https://github.com/LeMorrow/APOD.Net/issues/new?assignees=LeMorrow&labels=bug&template=bug_report.md).

<br>

Since APOD.Net is an asynchronous library we are going to make our `Main()` method asynchronous, which is a language feature from C# 7.1. If you don't have access to C# 7.1, read [this article](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-1#async-main) for workarounds.
```cs
using System;
using System.Threading.Tasks;
using Apod;

namespace Example
{
    public class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```
Note that we added another using directive for `System.Threading.Tasks`.

<br>

### Downloading the image
APOD.Net version 1.0 does not provide a method to download the images directly. This will likely be included in version 2.0, but for now we'll have to use our own [System.Net.HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netstandard-2.0) to download the image. If you would like this feature on the <xref:Apod.ApodClient> you can make your voice heard and track the progress in [issue #37](https://github.com/LeMorrow/APOD.Net/issues/37).

We'll create a helper method `DownloadImageToFileAsync()` that will use an [`HttpClient`](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netstandard-2.0) to download an image from a [`Uri`](https://docs.microsoft.com/en-us/dotnet/api/system.uri?view=netstandard-2.0) and then write the bytes to a file path.

Read [my post on stackoverflow](https://stackoverflow.com/questions/24797485/how-to-download-image-from-url/59167167#59167167) if you want a breakdown of this method.
```cs
private static async Task DownloadImageToFileAsync(Uri imageUri, string directoryPath,
                                                   string fileName, HttpClient httpClient)
{
    // Get the file extension
    var uriWithoutQuery = imageUri.GetLeftPart(UriPartial.Path);
    var fileExtension = Path.GetExtension(uriWithoutQuery);

    // Create file path and ensure directory exists
    var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
    Directory.CreateDirectory(directoryPath);

    // Download the image and write to the file
    var imageBytes = await httpClient.GetByteArrayAsync(imageUri);
    await File.WriteAllBytesAsync(path, imageBytes);
}
```
Note that we need to add the `System.Net.Http` namespace to use an [`HttpClient`](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netstandard-2.0) and the `System.IO` namespace to use the [`File`](https://docs.microsoft.com/en-us/dotnet/api/system.io.file?view=netstandard-2.0) class.
```cs
using System.Net.Http;
using System.IO;
``` 

<br>

### Getting user input
We want the user to specify the dates for the APODs. We'll write another helper method `GetValidDate()` that parses user input until they give a valid date and then returns that as a [`DateTime`](https://docs.microsoft.com/en-us/dotnet/api/system.datetime?view=netstandard-2.0).
```cs
private static DateTime GetValidDate(string prompt)
{
    Console.WriteLine(prompt);

    DateTime date;
    while (!DateTime.TryParse(Console.ReadLine(), out date))
    {
        Console.WriteLine("That is not a valid date. Try again.");
    }

    return date;
}
```

<br>

### Putting it all together
#### Creating the clients
We start by creating a new instance of a [System.Net.HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=netstandard-2.0) as well as an <xref:Apod.ApodClient>.
```cs
public static async Task Main()
{
    using var apodClient = new ApodClient();
    using var httpClient = new HttpClient();
}
```

<br>

#### Making the request
Then we prompt the users to input two dates using our helper method `GetValidDate()` and fetch the APODs between these two dates with <xref:Apod.ApodClient.FetchApodAsync(System.DateTime,System.DateTime)>.
```cs
 public static async Task Main()
{
    using var apodClient = new ApodClient();
    using var httpClient = new HttpClient();

    var startDate = GetValidDate("Enter a date between 1995-06-16 and today's date in an yyyy-MM-dd format.");
    var endDate = GetValidDate("Enter another date between the first date and today's date in an yyyy-MM-dd format.");

    Console.WriteLine("Fetching APOD data..");
    var response = await apodClient.FetchApodAsync(startDate, endDate);
}
```

<br>

#### Error handling
Now we need to make sure that there were no errors with the request, which we can do with the <xref:Apod.ApodResponse.StatusCode>. If an error did occur, we want to write it to the console.
```cs
public static async Task Main()
{
    using var apodClient = new ApodClient();
    using var httpClient = new HttpClient();

    var startDate = GetValidDate("Enter a date between 1995-06-16 and today's date in an yyyy-MM-dd format.");
    var endDate = GetValidDate("Enter another date between the first date and today's date in an yyyy-MM-dd format.");

    Console.WriteLine("Fetching APOD data..");
    var response = await apodClient.FetchApodAsync(startDate, endDate);

    if (response.StatusCode != ApodStatusCode.OK)
    {
        Console.WriteLine(response.Error.ErrorCode);
        Console.WriteLine(response.Error.ErrorMessage);
        return;
    }
}
```

<br>

#### Downloading the images
Lastly, we want to loop over every Astronomy Picture of the Day in <xref:Apod.ApodResponse.AllContent> and download the image with our helper method `DownloadImageToFileAsync()`. We also need to make sure that the content is actually an image, otherwise we cannot download it.
```cs
public static async Task Main()
{
    using var apodClient = new ApodClient();
    using var httpClient = new HttpClient();

    var startDate = GetValidDate("Enter a date between 1995-06-16 and today's date in an yyyy-MM-dd format.");
    var endDate = GetValidDate("Enter another date between the first date and today's date in an yyyy-MM-dd format.");

    Console.WriteLine("Fetching APOD data..");
    var response = await apodClient.FetchApodAsync(startDate, endDate);

    if (response.StatusCode != ApodStatusCode.OK)
    {
        Console.WriteLine(response.Error.ErrorCode);
        Console.WriteLine(response.Error.ErrorMessage);
        return;
    }

    foreach (var apod in response.AllContent)
    {
        if (apod.MediaType != MediaType.Image) { continue; }

        var uri = new Uri(apod.ContentUrl);
        var directoryPath = @"images/";
        var fileName = apod.Date.ToString("yyyy-MM-dd");
        Console.WriteLine($"Downloading image for {fileName}");
        await DownloadImageToFileAsync(uri, directoryPath, fileName, httpClient);
    }

    Console.WriteLine("Download complete!");
}
```

<br>

And we're done!
The application asks the user for input and downloads APOD images between the two dates. If the <xref:Apod.ApodContent.MediaType> is not <xref:Apod.MediaType.Image>, the content is not downloaded.

With the current values of `directoryPath` and `fileName`, it will place the images in `projectFolder\bin\Debug\netcoreapp3.0\images` (assuming the program is running in `Debug` mode) and name them in the format `yyyy-MM-dd.ext` where `.ext` is the extension found in the URI of the image (<xref:Apod.ApodContent.ContentUrl>).

**Example output**
![Screenshot of application running and downloaded images](../images/download-example.png)
Note: The application skips 2019-10-01 because the content is a youtube video [as you can see here](https://apod.nasa.gov/apod/ap191001.html).

<br>

## Full example code
```cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Apod;

namespace today
{
    public class Program
    {
        public static async Task Main()
        {
            using var apodClient = new ApodClient();
            using var httpClient = new HttpClient();

            var startDate = GetValidDate("Enter a date between 1995-06-16 and today's date in an yyyy-MM-dd format.");
            var endDate = GetValidDate("Enter another date between the first date and today's date in an yyyy-MM-dd format.");

            Console.WriteLine("Fetching APOD data..");
            var response = await apodClient.FetchApodAsync(startDate, endDate);

            if (response.StatusCode != ApodStatusCode.OK)
            {
                Console.WriteLine(response.Error.ErrorCode);
                Console.WriteLine(response.Error.ErrorMessage);
                return;
            }

            foreach (var apod in response.AllContent)
            {
                if (apod.MediaType != MediaType.Image) { continue; }

                var uri = new Uri(apod.ContentUrl);
                var directoryPath = @"images/";
                var fileName = apod.Date.ToString("yyyy-MM-dd");
                Console.WriteLine($"Downloading image for {fileName}");
                await DownloadImageToFileAsync(uri, directoryPath, fileName, httpClient);
            }

            Console.WriteLine("Download complete!");
        }

        private static async Task DownloadImageToFileAsync(Uri imageUri, string directoryPath, string fileName, HttpClient httpClient)
        {
            // Get the file extension
            var uriWithoutQuery = imageUri.GetLeftPart(UriPartial.Path);
            var fileExtension = Path.GetExtension(uriWithoutQuery);

            // Create file path and ensure directory exists
            var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
            Directory.CreateDirectory(directoryPath);

            // Download the image and write to the file
            var imageBytes = await httpClient.GetByteArrayAsync(imageUri);
            await File.WriteAllBytesAsync(path, imageBytes);
        }

        private static DateTime GetValidDate(string prompt)
        {
            Console.WriteLine(prompt);

            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("That is not a valid date. Try again.");
            }

            return date;
        }
    }
}
```
