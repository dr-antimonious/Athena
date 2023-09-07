using System.Runtime.CompilerServices;
using Artemis.Contracts.DTOs;

namespace Artemis.Contracts.Entities.Matches
{
    public abstract class PhasedBullseyeMatch : BullseyeMatch
    {
        protected override int SeriesInPhase => 3;

        protected virtual int PhasesInMatch => 2;

        public override int GetNumberOfSeriesInPhase() => SeriesInPhase;

        public override int GetNumberOfPhases() => PhasesInMatch;

        public override int GetNumberOfSeries() => SeriesInPhase * PhasesInMatch;

        public override int GetNumberOfShotsInPhase() => ShotsInSeries * SeriesInPhase;

        public override int GetNumberOfShots() => ShotsInSeries * SeriesInPhase * PhasesInMatch;

        public override List<Shot> GetShotsOfPhase(int index)
            => new(Shots.GetRange(
                SeriesInPhase * ShotsInSeries * index,
                ShotsInSeries * SeriesInPhase));

        public override List<ITuple> GetSeriesResultsOfPhase(int index)
        {
            List<ITuple> results = new();
            for (int i = SeriesInPhase * index; i < SeriesInPhase * (index + 1); i++)
                results.Add(GetSeriesResults(i));
            return results;
        }

        public override ITuple GetPhaseResults(int index)
            => Manager.GetPhaseResults(this, index);

        public override List<ITuple> GetAllPhaseResults()
        {
            List<ITuple> results = new();
            for (int i = 0; i < PhasesInMatch; i++)
                results.Add(GetPhaseResults(i));
            return results;
        }

        protected PhasedBullseyeMatch() : base()
        {
        }

        protected PhasedBullseyeMatch(
            User shooter,
            Timestamp startTimestamp,
            Timestamp endTimestamp,
            Location location,
            double? airTemperature = null,
            double? airPressure = null,
            double? windSpeed = null,
            string? windDirection = null,
            string? environmentNotes = null,
            string? equipmentNotes = null,
            string? shooterNotes = null)
            : base(
                shooter,
                startTimestamp,
                endTimestamp,
                location,
                airTemperature,
                airPressure,
                windSpeed,
                windDirection,
                environmentNotes,
                equipmentNotes,
                shooterNotes)
        {
        }

        protected PhasedBullseyeMatch(
            string id,
            User shooter,
            Timestamp startTimestamp,
            Timestamp endTimestamp,
            Location location,
            List<Shot> shots,
            double? airTemperature = null,
            double? airPressure = null,
            double? windSpeed = null,
            string? windDirection = null,
            string? environmentNotes = null,
            string? equipmentNotes = null,
            string? shooterNotes = null)
            : base(
                id,
                shooter,
                startTimestamp,
                endTimestamp,
                location,
                shots,
                airTemperature,
                airPressure,
                windSpeed,
                windDirection,
                environmentNotes,
                equipmentNotes,
                shooterNotes)
        {
        }

        protected PhasedBullseyeMatch(MatchCreateRequestDto createRequest)
            : base(createRequest)
        {
        }

        protected PhasedBullseyeMatch(MatchUpdateRequestDto matchUpdateRequest)
            : base(matchUpdateRequest)
        {
        }

        protected PhasedBullseyeMatch(MatchOutputDto matchOutput)
            : base(matchOutput)
        {
        }
    }
}
