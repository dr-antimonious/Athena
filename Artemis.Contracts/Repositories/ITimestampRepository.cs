using Artemis.Contracts.Entities;

namespace Artemis.Contracts.Repositories
{
    public interface ITimestampRepository<T> : IRepository<T> where T : class
    {
        Task<T?> GetByTimestamp(DateTime timestamp);
    }
}
