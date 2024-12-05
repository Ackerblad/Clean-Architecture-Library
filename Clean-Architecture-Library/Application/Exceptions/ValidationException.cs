namespace Application.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<string> errors)
            : base($"Validation failed: {string.Join("; ", errors)}") { }
    }
}
