namespace Shared.Wrapper;

public sealed record Result
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> Errors { get; init; } = new();

    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
    public static Result Failure(List<string> errors) => new() { IsSuccess = false, Errors = errors };
}

public sealed record Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public string? ErrorMessage { get; init; }
    public List<string> Errors { get; init; } = new();

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static Result<T> Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
    public static Result<T> Failure(List<string> errors) => new() { IsSuccess = false, Errors = errors };
}