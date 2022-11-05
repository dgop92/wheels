namespace SharedKernel.Errors;

public class RepositoryException : BaseException
{
    public RepositoryException(
        string userMessage,
        string code,
        object? errorParams
    ) : base(ErrorLevel.Repository, userMessage, code, errorParams)
    {
    }

    public RepositoryException(
        string userMessage,
        string code
    ) : base(ErrorLevel.Repository, userMessage, code)
    {
    }
}
