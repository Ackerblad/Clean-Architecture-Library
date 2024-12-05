namespace Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, Guid id)
            : base($"The {entityName} with ID {id} was not found.") { }
    }
}
