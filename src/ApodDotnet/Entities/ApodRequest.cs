using System;

namespace ApodDotnet.Entities
{
    // I am currently thinking about how to make the usage of a request as frictionless as possible.
    // This whole class is going to change (and maybe be deleted).

    public class ApodRequest
    {
        /// <summary>The date to request the APOD for. Must be 1995-06-20 or after. Defaults to DateTime.Today.</summary>
        public readonly DateTime Date;

        // Cannot be used with date, startDate or endDate
        // Add summary for this later.
        public readonly int Count;

        // Cannot be used with date
        // Add summaries later.
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;

        /// <summary>Make a request for today's Astronomy Picture of the Day.</summary>
        public ApodRequest()
        {
            Date = DateTime.Today;
        }

        /// <summary>Make a request for the Astronomy Picture of the Day for a specific date.</summary>
        /// <param name="date">The date to request the APOD for. Must be 1995-06-20 or after.</param>
        public ApodRequest(DateTime date)
        {
            Date = date;
        }

        // TODO: Test what happens if endDate is before startDate.
        /// <summary>Make a request for all the APODs between two dates.</summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public ApodRequest(DateTime startDate, DateTime endDate)
        {

        }

        // I don't know if this is a good way to go about it, since this request would return an array..?
        // The same goes for the range, actually.
        public ApodRequest(int count)
        {

        }
    }
}
