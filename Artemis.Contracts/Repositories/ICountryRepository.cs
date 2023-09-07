namespace Artemis.Contracts.Repositories
{
    public interface ICountryRepository<T> : INameRepository<T> where T : class
    {
        Task<List<T>> GetByCityNameAsync(string city);
    }
}
