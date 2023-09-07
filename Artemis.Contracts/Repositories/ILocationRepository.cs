namespace Artemis.Contracts.Repositories
{
    public interface ILocationRepository<T> : ICityRepository<T>, ICountryRepository<T> where T : class
    {
    }
}
