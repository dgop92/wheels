namespace SharedKernel.Errors;

using SharedKernel.Utils;

public abstract class BaseException : Exception
{
    public string Level { get; init; }
    public string UserMessage { get; init; }
    public string Code { get; init; }
    public object? ErrorParams { get; init; }

    public BaseException(
        string level,
        string userMessage,
        string code,
        object? errorParams
    ) : base($"[{DateService.NowAsString}] - [{level}]: {userMessage}")
    {
        Level = level;
        UserMessage = userMessage;
        Code = code;
        ErrorParams = errorParams;
    }

    public BaseException(
        string level,
        string userMessage,
        string code
    ) : base($"[{DateService.NowAsString}] - [{level}]: {userMessage}")
    {
        Level = level;
        UserMessage = userMessage;
        Code = code;
    }

}
