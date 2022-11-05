namespace SharedKernel.Errors;

public static class ErrorCode
{
    public static readonly string InvalidInput = "INVALID_INPUT";
    public static readonly string InvalidId = "INVALID_ID";
    public static readonly string InvalidOperation = "INVALID_OPERATION";
    public static readonly string DuplicatedRecord = "DUPLICATED_RECORD";
    public static readonly string IdNotProvided = "ID_NOT_PROVIDED";
    public static readonly string NotFound = "NOT_FOUND";
    public static readonly string Unauthorized = "UNAUTHORIZED";
    public static readonly string Forbidden = "FORBIDDEN";
    public static readonly string ApplicationIntegrityError = "APPLICATION_INTEGRITY_ERROR";
}