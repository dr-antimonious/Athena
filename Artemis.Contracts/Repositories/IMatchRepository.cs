namespace Artemis.Contracts.Repositories
{
    public interface IMatchRepository<T> : IRemovableRepository<T> where T : class
    {
        Task<List<T>> GetByUserIdAsync(string userId);

        Task DeleteMulti(List<T> entities);

        Task<List<T>> GetMulti(List<string> ids);
    }
}
