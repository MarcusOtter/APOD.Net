namespace Apod
{
    public enum ApodErrorCode
    {
        None,
        BadRequest, // This should be removed eventually and be replaced with more specific error codes
        DateOutOfRange,
        StartDateAfterEndDate,
        InternalServiceError,
        ApiKeyMissing,
        ApiKeyInvalid,
        Timeout
    }
}
