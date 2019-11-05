namespace Apod
{
    public interface IErrorBuilder
    {
        ApodResponse GetDateOutOfRangeError();
        ApodResponse GetStartDateAfterEndDateError();
    }
}
