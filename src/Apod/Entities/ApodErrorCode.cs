namespace Apod
{
    public enum ApodErrorCode
    {
        None,
        BadRequest, // This should be removed eventually and be replaced with more specific error codes
        DateOutOfRange,
        InternalServiceError,
        ApiKeyMissing,
        ApiKeyInvalid,
        Timeout
    }
}
