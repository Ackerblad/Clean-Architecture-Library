namespace Application.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string entityName)
            : base($"{entityName} already exists.") { }
    }
}
