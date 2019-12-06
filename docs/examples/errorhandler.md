# Override the IErrorHandler
This example shows how to override the <xref:Apod.Logic.Errors.IErrorHandler> in the <xref:Apod.ApodClient> for your needs. You can use this guide as a reference for overloading any other interface in the library. Intermediate knowledge in C# is assumed.

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

### Creating the custom IErrorHandler
To override the <xref:Apod.Logic.Errors.IErrorHandler> in the <xref:Apod.ApodClient>, we'll create a new class that implements <xref:Apod.Logic.Errors.IErrorHandler>. In Visual Studio, add a new class (`Project` > `Add Class...`) and name it something suitable for your use case. We'll go with `CustomErrorHandler.cs` for this example.

First off, add the using directive for `Apod.Logic.Errors`.
```cs
using Apod.Logic.Errors;
```

<br>

Next, imlpement <xref:Apod.Logic.Errors.IErrorHandler> (which should add the following code for you).
```cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apod;
using Apod.Logic.Errors;

namespace Example
{
    public class CustomErrorHandler : IErrorHandler
    {
        public ApodError ValidateCount(int count)
        {
            throw new NotImplementedException();
        }

        public ApodError ValidateDate(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ApodError ValidateDateRange(DateTime startDate, DateTime endDate = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            throw new NotImplementedException();
        }
    }
}
```

<br>

Now we can start writing our custom behaviour for the methods. It's very useful to use the [source code of the default implementation](https://github.com/LeMorrow/APOD.Net/blob/master/src/Apod/Logic/Errors/ErrorHandler.cs) of <xref:Apod.Logic.Errors.IErrorHandler> as a reference if you're not sure about what a method is supposed to do. It can also be useful to look how the <xref:Apod.Logic.Errors.IErrorHandler> is used in the [source code of the ApodClient](https://github.com/LeMorrow/APOD.Net/blob/master/src/Apod/ApodClient.cs).

For this example, let's say we need `ValidateCount` to depend on the current day of the month. The users can only get 5 or less APODs on the 5th, 14 or less APODs on the 14th, and so on.
```cs
public ApodError ValidateCount(int count)
{
    var today = DateTime.Today;
    var countIsValid = today.Day <= count;

    if (countIsValid) 
    {
        return new ApodError(ApodErrorCode.None);
    }

    if (DateTime.DaysInMonth(today.Year, today.Month) < count)
    {
        return new ApodError(ApodErrorCode.CountOutOfRange, 
            $"There aren't {count} days in {today.ToString("MMMM")}");
    }

    return new ApodError(ApodErrorCode.CountOutOfRange, 
        $"You have to wait until the {count} {today.ToString("MMMM")} to get that many APODs.");
}
```

<br>

Next, we'll implement `ValidateHttpResponseAsync`. For this example, we don't care which type of error it was, if any. We just want to know if there was an error or not, which we can do synchronously.
```cs
public ValueTask<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
{
    if (httpResponse.IsSuccessStatusCode) 
    {
        var noError = new ApodError(ApodErrorCode.None);
        return new ValueTask<ApodError>(noError);
    }

    var error = new ApodError(ApodErrorCode.BadRequest, "An error occured.");
    return new ValueTask<ApodError>(error);
}
```

<br>

We'll assume `ValidateDate()` and `ValidateDateRange()` are never going to be called and leave them as is.

`CustomErrorHandler.cs` result:
```cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apod;
using Apod.Logic.Errors;

namespace Example
{
    public class CustomErrorHandler : IErrorHandler
    {
        public ApodError ValidateCount(int count)
        {
            var today = DateTime.Today;
            var countIsValid = count <= today.Day;

            if (countIsValid) 
            {
                return new ApodError(ApodErrorCode.None);
            }

            if (DateTime.DaysInMonth(today.Year, today.Month) < count)
            {
                return new ApodError(ApodErrorCode.CountOutOfRange, 
                    $"There aren't {count} days in {today.ToString("MMMM")}.");
            }

            return new ApodError(ApodErrorCode.CountOutOfRange, 
                $"You have to wait until {today.ToString("MMMM")} {count} to get that many APODs.");
        }

        public ApodError ValidateDate(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ApodError ValidateDateRange(DateTime startDate, DateTime endDate = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) 
            {
                var noError = new ApodError(ApodErrorCode.None);
                return new ValueTask<ApodError>(noError);
            }

            var error = new ApodError(ApodErrorCode.BadRequest, "An error occured.");
            return new ValueTask<ApodError>(error);
        }
    }
}
```

<br>

### Using our custom error handler
Overriding the error handler in the client is easy. Back in our `Program.cs`, we create a new instance of our `CustomErrorHandler` and then inject it with the [ApodClient(String, IHttpRequester, IHttpResponseParser, IErrorHandler)](xref:Apod.ApodClient#Apod_ApodClient__ctor_System_String_Apod_Logic_Net_IHttpRequester_Apod_Logic_Net_IHttpResponseParser_Apod_Logic_Errors_IErrorHandler_) constructor.
```cs
public static async Task Main()
{
    var errorHandler = new CustomErrorHandler();
    using var client = new ApodClient("DEMO_KEY", errorHandler: errorHandler);
}
```
The client will now use our custom IErrorHandler to validate the requests.

<br>

## Full example code
Below is an example project that uses the `CustomErrorHandler`. We won't go into detail how it's built, but it's similar to a very simplified version of [Download all APODs between two dates](download.md).


```cs
// Program.cs
using System;
using System.Threading.Tasks;
using Apod;

namespace Example
{
    public class Program
    {
        public static async Task Main()
        {
            var errorHandler = new CustomErrorHandler();
            using var client = new ApodClient("DEMO_KEY", errorHandler: errorHandler);

            var amount = GetValidInt("How many astronomy pictures would you like?");
            var response = await client.FetchApodAsync(amount);

            if (response.StatusCode != ApodStatusCode.OK)
            {
                Console.WriteLine(response.Error.ErrorMessage);
                return;
            }

            foreach (var apod in response.AllContent)
            {
                Console.WriteLine(apod.ContentUrl);
            }
        }

        private static int GetValidInt(string prompt)
        {
            Console.WriteLine(prompt);

            int result;
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("That is not a valid integer. Try again.");
            }

            return result;
        }
    }
}
```
```cs
// CustomErrorHandler.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Apod;
using Apod.Logic.Errors;

namespace Example
{
    public class CustomErrorHandler : IErrorHandler
    {
        public ApodError ValidateCount(int count)
        {
            var today = DateTime.Today;
            var countIsValid = count <= today.Day;

            if (countIsValid) 
            {
                return new ApodError(ApodErrorCode.None);
            }

            if (DateTime.DaysInMonth(today.Year, today.Month) < count)
            {
                return new ApodError(ApodErrorCode.CountOutOfRange, 
                    $"There aren't {count} days in {today.ToString("MMMM")}.");
            }

            return new ApodError(ApodErrorCode.CountOutOfRange,
                $"You have to wait until {today.ToString("MMMM")} {count} to get that many APODs.");
        }

        public ApodError ValidateDate(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public ApodError ValidateDateRange(DateTime startDate, DateTime endDate = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ApodError> ValidateHttpResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse.IsSuccessStatusCode) 
            {
                var noError = new ApodError(ApodErrorCode.None);
                return new ValueTask<ApodError>(noError);
            }

            var error = new ApodError(ApodErrorCode.BadRequest, "An error occured.");
            return new ValueTask<ApodError>(error);
        }
    }
}
```