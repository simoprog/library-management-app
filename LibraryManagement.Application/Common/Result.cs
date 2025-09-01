namespace LibraryManagement.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T Value { get; private set; }
    public string Error { get; private set; }
    public List<string> Errors { get; private set; }

    private Result(bool isSuccess, T value, string error, List<string> errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? string.Empty;
        Errors = errors ?? new List<string>();
    }

    public static Result<T> Success(T value) => new(true, value, string.Empty, new List<string>());
    
    public static Result<T> Failure(string error) => new(false, default(T)!, error, new List<string> { error });
    
    public static Result<T> Failure(List<string> errors) => new(false, default(T)!, string.Empty, errors);
}

public class Result
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }
    public List<string> Errors { get; private set; }

    private Result(bool isSuccess, string error, List<string> errors)
    {
        IsSuccess = isSuccess;
        Error = error ?? string.Empty;
        Errors = errors ?? new List<string>();
    }

    public static Result Success() => new(true, string.Empty, new List<string>());
    
    public static Result Failure(string error) => new(false, error, new List<string> { error });
    
    public static Result Failure(List<string> errors) => new(false, string.Empty, errors);
}