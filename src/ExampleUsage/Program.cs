using System;
using System.Threading.Tasks;
using Apod;

namespace ApodExample
{
    public class Program
    {
        private static async Task Main()
        {
            var dateTimeNow = DateTime.Now;
            var utcNow = DateTime.UtcNow;

            Console.WriteLine($"The UTC time now is {utcNow}.");
            Console.WriteLine();
            Console.WriteLine($"The local time right now is {dateTimeNow.TimeOfDay}.");
            Console.WriteLine($"The time of the local date is {dateTimeNow.Date.TimeOfDay}.");
            Console.WriteLine(dateTimeNow.TimeOfDay == dateTimeNow.Date.TimeOfDay ? "They are equal." : "They are not equal.");
            Console.WriteLine($"The Timespan.Zero has the value of {TimeSpan.Zero}");
            Console.WriteLine($"It is {(dateTimeNow.Date.TimeOfDay == TimeSpan.Zero ? "exactly" : "not ")} equal to the time of the local date.");
            Console.WriteLine();

            var greenlandNow = new DateTimeOffset(dateTimeNow).ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Greenland Standard Time").BaseUtcOffset).DateTime;
            Console.WriteLine($"The time in Greenland right now is {greenlandNow.TimeOfDay}.");
            Console.WriteLine($"The time of Greenland's date is {greenlandNow.Date.TimeOfDay}.");
            Console.WriteLine(DateTime.Now.TimeOfDay == greenlandNow.Date.TimeOfDay ? "They are equal." : "They are not equal.");
            Console.WriteLine($"The Timespan.Zero has the value of {TimeSpan.Zero}");
            Console.WriteLine($"It is {(greenlandNow.Date.TimeOfDay == TimeSpan.Zero ? "exactly" : "not ")} equal to the time of the date in Greenland.");
            Console.WriteLine();

            Console.WriteLine($"Trying to convert Greenland time {greenlandNow} to another time zone: {SetUtcOffset(greenlandNow, new TimeSpan(-9, 0, 0))}.");
            Console.WriteLine();

            Console.WriteLine($"The dateTimeNow has the kind: {dateTimeNow.Kind}. The time is {dateTimeNow.TimeOfDay}");
            var dateTimeOffsetNow = new DateTimeOffset(dateTimeNow);
            Console.WriteLine($"It got converted to a DateTimeOffset with time {dateTimeOffsetNow.TimeOfDay} and UTC offset {dateTimeOffsetNow.Offset}");

            var easternTimeNow = dateTimeOffsetNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time").BaseUtcOffset);
            Console.WriteLine($"The time in current eastern time should be {easternTimeNow.TimeOfDay} and the offset should be {easternTimeNow.Offset}");

            Console.WriteLine($"The new dateTime object has the time {easternTimeNow.DateTime}");
            Console.WriteLine($"The new dateTime object has the kind {easternTimeNow.DateTime.Kind}");

            //var apodClient = new ApodClient("DEMO_KEY");
            //var apod = await apodClient.FetchApodAsync(new DateTime(2005, 03, 19), new DateTime(2012, 05, 29));

            //Console.WriteLine(apod.Title);
            //Console.WriteLine(apod.Explanation);
            //Console.WriteLine(apod.ContentUrlHD);
        }

        private static DateTime SetUtcOffset(DateTime dateTime, TimeSpan utcOffset)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            dateTimeOffset.ToOffset(utcOffset);
            return dateTimeOffset.DateTime;
        }
    }
}
