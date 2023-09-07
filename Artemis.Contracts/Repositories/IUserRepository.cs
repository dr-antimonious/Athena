namespace Artemis.Contracts.Repositories
{
    public interface IUserRepository<T> : IRemovableRepository<T> where T : class 
    {
        Task<T?> GetByEmailAsync(string email);
    }
}
