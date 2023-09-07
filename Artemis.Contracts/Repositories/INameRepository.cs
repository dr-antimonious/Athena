namespace Artemis.Contracts.Repositories
{
    public interface INameRepository<T> : IRepository<T> where T : class
    {
        Task<T?> GetByExactNameMatchAsync(string name);

        Task<List<T>> GetByPartialNameMatchAsync(string name);
    }
}
