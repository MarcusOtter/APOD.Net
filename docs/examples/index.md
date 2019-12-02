# Information about the examples
Information you should know before reading the examples on the left.

## Using an API key
To make the examples straight forward, the <xref:Apod.ApodClient> will be initialized with a string literal as an API key.
```cs
var client = new ApodClient("YOUR_API_KEY_HERE");
```
You **should not** include the API key as a string literal in your source code. There are many better ways, a few examples listed below. Remember to add all files with sensitive data to your `.gitignore`.
- Write an App.config file and use the [ConfigurationManager.AppSettings property](https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.appsettings?viewFallbackFrom=netstandard-2.0)
- Read from a custom file containing the API key with the [System.IO.File.ReadAllText method](https://docs.microsoft.com/en-us/dotnet/api/system.io.file.readalltext?view=netstandard-2.0#System_IO_File_ReadAllText_System_String_)
- Set up an environment variable and use [Environment.GetEnvironmentVariable](https://docs.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable?view=netstandard-2.0)
- Pass your API key as a command line argument and access it with the [string\[\] args](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/main-and-command-args/command-line-arguments) in your `Main` method


## Disposing the client
There are many different ways of disposing objects in C#. To keep a consistent style in the examples, the alternative syntax for the [using statement](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement) (demonstrated below) will be used, which was introduced with C# 8 . If you do not have access to C# 8, refer to the chapter "[Disposing the client](https://github.com/LeMorrow/APOD.Net#disposing-the-client)" in the README for more examples on how to accomplish the same functionality.

```cs
using var client = new ApodClient();
```
