namespace Artemis.Contracts.Repositories
{
    public interface IRemovableRepository<T> : IRepository<T> where T : class
    {
        Task Delete(T entity);
    }
}
