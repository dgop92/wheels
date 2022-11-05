namespace SharedKernel.Errors;

public class DomainException : BaseException
{
    public DomainException(
        string userMessage,
        string code,
        object? errorParams
    ) : base(ErrorLevel.Domain, userMessage, code, errorParams)
    {
    }

    public DomainException(
        string userMessage,
        string code
    ) : base(ErrorLevel.Domain, userMessage, code)
    {
    }
}
