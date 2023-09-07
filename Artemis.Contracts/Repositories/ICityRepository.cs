namespace Artemis.Contracts.Repositories
{
    public interface ICityRepository<T> : INameRepository<T> where T : class
    {
        Task<List<T>> GetByCountryNameAsync(string country);
    }
}
