# This is how you use the thing!
```cs
var client = new ApodClient();
var response = await client.FetchApodAsync();

if (response.StatusCode != ApodStatusCode.OK)
{
    Console.WriteLine(response.Error.ErrorMessage);
}

client.Dispose();
```