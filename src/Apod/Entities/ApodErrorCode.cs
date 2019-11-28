namespace Apod
{
    public enum ApodErrorCode
    {
        None,
        BadRequest,
        DateOutOfRange,
        StartDateAfterEndDate,
        CountOutOfRange,
        InternalServiceError,
        ApiKeyMissing,
        ApiKeyInvalid,
        Timeout,
        OverRateLimit,
        Unknown
    }
}
