using System.Runtime.CompilerServices;

namespace Artemis.Contracts.Entities.Interfaces
{
    public interface IMatch
    {
        string Id { get; }

        User Shooter { get; set; }

        Timestamp StartTimestamp { get; set; }

        Timestamp EndTimestamp { get; set; }

        Location Location { get; set; }

        double? AirTemperature { get; set; }

        double? AirPressure { get; set; }

        double? WindSpeed { get; set; }

        string? WindDirection { get; set; }

        string? EnvironmentNotes { get; set; }

        string? EquipmentNotes { get; set; }

        string? ShooterNotes { get; set; }

        int GetNumberOfShotsInSeries();

        int GetNumberOfSeriesInPhase();

        int GetNumberOfPhases();

        int GetNumberOfSeries();

        int GetNumberOfShotsInPhase();

        int GetNumberOfShots();

        Shot GetShotAt(int index);

        List<Shot> GetAllShots();

        void AddShot(Shot shot);

        void AddAllShots(List<Shot> shots);

        List<Shot> GetShotsOfSeries(int index);

        List<Shot> GetShotsOfPhase(int index);

        ITuple GetSeriesResults(int index);

        List<ITuple> GetAllSeriesResults();

        List<ITuple> GetSeriesResultsOfPhase(int index);

        ITuple GetPhaseResults(int index);

        List<ITuple> GetAllPhaseResults();

        ITuple GetMatchResult();

        int GetTotalBullseyeCount();

        int GetBullseyeCountOfShots(List<Shot> shots);
    }
}
