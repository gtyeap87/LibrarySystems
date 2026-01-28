namespace Library.Dto
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; init; }
        public T? Value { get; init; }

        public IDictionary<string, string[]>? Errors { get; init; }

        public static ResultDto<T> Success(T value) => new() { IsSuccess = true, Value = value };

        public static ResultDto<T> Failure(IDictionary<string, string[]> errors) => new() { IsSuccess = false, Errors = errors };
    }
}