namespace Artemis.Contracts.Repositories
{
    public interface IMultiRepository<T> : IRemovableRepository<T> where T : class
    {
        Task CreateMulti(List<T> entities);

        Task UpdateMulti(List<T> entities);

        Task<List<T>> GetMulti(List<string> entities);
    }
}
