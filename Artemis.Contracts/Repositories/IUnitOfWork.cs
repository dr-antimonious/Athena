using Artemis.Contracts.Entities;
using Artemis.Contracts.Entities.Matches;

namespace Artemis.Contracts.Repositories
{
    public interface IUnitOfWork
    {
        ICityRepository<City> Cities { get; }

        ICountryRepository<Country> Countries { get; }

        ILocationRepository<Location> Locations { get; }

        IMatchRepository<Match> Matches { get; }

        IMultiRepository<Shot> Shots { get; }

        ITimestampRepository<Timestamp> Timestamps { get; }

        IUserRepository<User> Users { get; }

        Task SaveChangesAsync();
    }
}
