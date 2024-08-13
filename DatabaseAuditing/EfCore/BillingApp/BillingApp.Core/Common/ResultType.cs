namespace BillingApp.Core.Common;

public class ResultType<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string? ErrorMessage { get; }
    public List<string> Errors { get; }

    private ResultType(bool isSuccess, T value, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        Errors = new List<string>();
    }

    private ResultType(bool isSuccess, T value, List<string>? errors)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors ?? new List<string>();
        ErrorMessage = string.Join(", ", Errors);
    }

    public static ResultType<T> Success(T value) => new(true, value, (string)default);
    public static ResultType<T> Failure(string? errorMessage) => new(false, default, errorMessage);
    public static ResultType<T> Failure(List<string> errors) => new(false, default, errors);
}