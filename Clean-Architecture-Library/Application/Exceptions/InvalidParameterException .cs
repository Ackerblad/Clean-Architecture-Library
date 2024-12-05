namespace Application.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string parameterName)
            : base($"Invalid parameter '{parameterName}'.") { }
    }
}
