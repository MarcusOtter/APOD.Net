# Write today's APOD title to the console
This is a very basic example that will write the title of today's Astronomy Picture of the Day to the console window. 

This example targets .NET Core 3.0 but will work with any platform version that [supports .NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) (small changes may be required for different target frameworks).

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

### Making the request
The first thing we need is a new instance of an <xref:Apod.ApodClient>. We initialize it with our API key in our `Main()` method
```cs
public static async Task Main()
{
    using var client = new ApodClient("YOUR_API_KEY_HERE");
}
```

<br>

To get today's APOD, we want to use <xref:Apod.ApodClient.FetchApodAsync> and save the response.

```cs
public static async Task Main()
{
    using var client = new ApodClient("YOUR_API_KEY_HERE");
    var response = client.FetchApodAsync();
}
```

<br>

### Error handling
Now we need to make sure that there were no errors with the request, which we can do with the <xref:Apod.ApodResponse.StatusCode>. If an error did occur, we want to write it to the console.
```cs
public static async Task Main()
{
    using var client = new ApodClient("YOUR_API_KEY_HERE");
    var response = await client.FetchApodAsync();

    if (response.StatusCode != ApodStatusCode.OK)
    {
        Console.WriteLine(response.Error.ErrorCode);
        Console.WriteLine(response.Error.ErrorMessage);
        return;
    }
}
```

<br>

### Reading the response
Now we can safely read the <xref:Apod.ApodResponse.Content> and write the <xref:Apod.ApodContent.Title> to the console. We'll also add `Console.ReadLine()` to the end of the program so the console window stays open instead of instantly closing as the program terminates.
```cs
public static async Task Main()
{
    using var client = new ApodClient("YOUR_API_KEY_HERE");
    var response = await client.FetchApodAsync();

    if (response.StatusCode != ApodStatusCode.OK)
    {
        Console.WriteLine(response.Error.ErrorCode);
        Console.WriteLine(response.Error.ErrorMessage);
        return;
    }

    Console.WriteLine(response.Content.Title);
    Console.ReadLine();
}
```

<br>

And we're done! This program will write the title of today's APOD to the console and wait for you to press the `Enter` key before terminating.

There are many more properties in the <xref:Apod.ApodContent> that you can use, for example <xref:Apod.ApodContent.ContentUrl>, <xref:Apod.ApodContent.Explanation> and <xref:Apod.ApodContent.Date> to name a few.

## Full example code
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
            using var client = new ApodClient("YOUR_API_KEY_HERE");
            var response = await client.FetchApodAsync();

            if (response.StatusCode != ApodStatusCode.OK)
            {
                Console.WriteLine(response.Error.ErrorCode);
                Console.WriteLine(response.Error.ErrorMessage);
                return;
            }

            Console.WriteLine(response.Content.Title);
            Console.ReadLine();
        }
    }
}
```
