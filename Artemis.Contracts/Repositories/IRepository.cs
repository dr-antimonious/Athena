namespace Artemis.Contracts.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetAsync(string id);

        Task Create(T entity);

        Task Update(T entity);
    }
}
