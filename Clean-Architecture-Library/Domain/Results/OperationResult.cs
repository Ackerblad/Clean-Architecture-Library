namespace Domain.Results
{
    public class OperationResult<T>
    {
        public bool IsSuccessful { get; private set; }
        public T? Data { get; private set; }
        public string Message { get; private set; }
        public string ErrorMessage { get; private set; }

        private OperationResult(bool isSuccessful, T? data, string message, string errorMessage)
        {
            IsSuccessful = isSuccessful;
            Data = data;
            Message = message;
            ErrorMessage = errorMessage;
        }

        public static OperationResult<T> Successful(T data, string message)
        {
            return new OperationResult<T>(true, data, message, null!);
        }

        public static OperationResult<T> Failure(string errorMessage, string message)
        {
            return new OperationResult<T>(false, default, message, errorMessage);
        }
    }
}
