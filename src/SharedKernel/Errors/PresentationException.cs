namespace SharedKernel.Errors;

public class PresentationException : BaseException
{
    public PresentationException(
        string userMessage,
        string code,
        object? errorParams
    ) : base(ErrorLevel.Presentation, userMessage, code, errorParams)
    {
    }

    public PresentationException(
        string userMessage,
        string code
    ) : base(ErrorLevel.Presentation, userMessage, code)
    {
    }
}
