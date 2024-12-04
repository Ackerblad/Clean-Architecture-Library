namespace Application.Interfaces.RepositoryInterfaces
{
    public interface IQueryRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
