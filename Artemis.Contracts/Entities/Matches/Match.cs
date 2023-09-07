using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Interfaces;
using Artemis.Contracts.Entities.Managers;
using Artemis.Contracts.Exceptions;

namespace Artemis.Contracts.Entities.Matches
{
    public abstract class Match : IMatch, IConvertable<MatchOutputDto>
    {
        public static Dictionary<string, int> TotalShots
        {
            get
            {
                return new Dictionary<string, int>
                {
                    {"3P50", 120},
                    {"AP10", 60},
                    {"AR10", 60},
                    {"P25", 60},
                    {"RFP25", 60},
                    {"TS", 125}
                };
            }
        }

        public static Dictionary<string, Func<MatchCreateRequestDto, Match>> CreateMatch
        {
            get
            {
                return new Dictionary<string, Func<MatchCreateRequestDto, Match>>
                {
                    {"3P50", x => new _3P50Match(x)},
                    {"AP10", x => new AP10Match(x)},
                    {"AR10", x => new AR10Match(x)},
                    {"P25", x => new P25Match(x)},
                    {"RFP25", x => new RFP25Match(x)},
                    {"TS", x => new TSMatch(x)}
                };
            }
        }

        public static Dictionary<string, Func<MatchUpdateRequestDto, Match>> UpdateMatch
        {
            get
            {
                return new Dictionary<string, Func<MatchUpdateRequestDto, Match>>
                {
                    {"3P50", x => new _3P50Match(x)},
                    {"AP10", x => new AP10Match(x)},
                    {"AR10", x => new AR10Match(x)},
                    {"P25", x => new P25Match(x)},
                    {"RFP25", x => new RFP25Match(x)},
                    {"TS", x => new TSMatch(x)}
                };
            }
        }

        public static Dictionary<string, Func<MatchOutputDto, Match>> ConvertMatch
        {
            get
            {
                return new Dictionary<string, Func<MatchOutputDto, Match>>
                {
                    {"3P50", x => new _3P50Match(x)},
                    {"AP10", x => new AP10Match(x)},
                    {"AR10", x => new AR10Match(x)},
                    {"P25", x => new P25Match(x)},
                    {"RFP25", x => new RFP25Match(x)},
                    {"TS", x => new TSMatch(x)}
                };
            }
        }

        public static Dictionary<Type, string> TypeConversion
        {
            get
            {
                return new Dictionary<Type, string>
                {
                    {typeof(_3P50Match), "3P50"},
                    {typeof(AP10Match), "AP10"},
                    {typeof(AR10Match), "AR10"},
                    {typeof(P25Match), "P25"},
                    {typeof(RFP25Match), "RFP25"},
                    {typeof(TSMatch), "TS"}
                };
            }
        }

        protected virtual IMatchManager Manager => TSMatchManager.Instance;

        public List<Shot> Shots;

        [Key]
        public string Id { get; }

        public User Shooter { get; set; } = null!;

        public Timestamp StartTimestamp { get; set; } = null!;

        public Timestamp EndTimestamp { get; set; } = null!;

        public Location Location { get; set; } = null!;

        public double? AirTemperature { get; set; }

        public double? AirPressure { get; set; }

        public double? WindSpeed { get; set; }

        public string? WindDirection { get; set; }

        public string? EnvironmentNotes { get; set; }

        public string? EquipmentNotes { get; set; }

        public string? ShooterNotes { get; set; }

        protected virtual int ShotsInSeries => 10;

        protected virtual int SeriesInPhase => 6;

        public MatchOutputDto Convert()
            => CreateDto();

        public MatchOutputDto CreateDto()
            => new(this);

        public int GetNumberOfShotsInSeries() => ShotsInSeries;

        public virtual int GetNumberOfSeriesInPhase()
            => throw new PhasesNotSupportedException(
                nameof(GetNumberOfSeriesInPhase),
                this.GetType().ToString());

        public virtual int GetNumberOfPhases()
            => throw new PhasesNotSupportedException(
                nameof(GetNumberOfPhases),
                this.GetType().ToString());

        public virtual int GetNumberOfSeries() => SeriesInPhase;

        public virtual int GetNumberOfShotsInPhase()
            => throw new PhasesNotSupportedException(
                nameof(GetNumberOfShotsInPhase),
                this.GetType().ToString());

        public virtual int GetNumberOfShots()
            => ShotsInSeries * SeriesInPhase;

        public Shot GetShotAt(int index) => Shots[index];

        public List<Shot> GetAllShots() => new(Shots);

        public void AddShot(Shot shot) => Shots.Add(shot);

        public void AddAllShots(List<Shot> shots) => Shots = shots;

        public List<Shot> GetShotsOfSeries(int index)
            => new(Shots.GetRange(
                ShotsInSeries * index,
                ShotsInSeries));

        public virtual List<Shot> GetShotsOfPhase(int index)
            => throw new PhasesNotSupportedException(
                nameof(GetShotsOfPhase),
                this.GetType().ToString());

        public ITuple GetSeriesResults(int index)
            => Manager.GetSeriesResults(this, index);

        public List<ITuple> GetAllSeriesResults()
        {
            List<ITuple> results = new();
            for (int i = 0; i < GetNumberOfShots() / ShotsInSeries; i++)
                results.Add(GetSeriesResults(i));
            return results;
        }

        public virtual List<ITuple> GetSeriesResultsOfPhase(int index)
            => throw new PhasesNotSupportedException(
                nameof(GetSeriesResultsOfPhase),
                this.GetType().ToString());

        public virtual ITuple GetPhaseResults(int index)
            => throw new PhasesNotSupportedException(
                nameof(GetPhaseResults),
                this.GetType().ToString());

        public virtual List<ITuple> GetAllPhaseResults()
            => throw new PhasesNotSupportedException(
                nameof(GetAllPhaseResults),
                this.GetType().ToString());

        public ITuple GetMatchResult()
            => Manager.GetMatchResult(this);

        public virtual int GetTotalBullseyeCount()
            => throw new BullseyeNotSupportedException(
                nameof(GetTotalBullseyeCount),
                this.GetType().ToString());

        public virtual int GetBullseyeCountOfShots(List<Shot> shots)
            => throw new BullseyeNotSupportedException(
                nameof(GetBullseyeCountOfShots),
                this.GetType().ToString());

        public Match()
        {
            Id = Guid.NewGuid().ToString();
            Shots = new();
        }

        protected Match(
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
            : this()
        {
            Shooter = shooter;
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            Location = location;
            AirTemperature = airTemperature;
            AirPressure = airPressure;
            WindSpeed = windSpeed;
            WindDirection = windDirection;
            EnvironmentNotes = environmentNotes;
            EquipmentNotes = equipmentNotes;
            ShooterNotes = shooterNotes;
        }

        protected Match(
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
        {
            Id = id;
            Shooter = shooter;
            StartTimestamp = startTimestamp;
            EndTimestamp = endTimestamp;
            Location = location;
            AirTemperature = airTemperature;
            AirPressure = airPressure;
            WindSpeed = windSpeed;
            WindDirection = windDirection;
            EnvironmentNotes = environmentNotes;
            EquipmentNotes = equipmentNotes;
            ShooterNotes = shooterNotes;
            Shots = shots;
        }

        protected Match(MatchCreateRequestDto createRequest)
        {
            Id = Guid.NewGuid().ToString();
            AirTemperature = createRequest.AirTemperature;
            AirPressure = createRequest.AirPressure;
            WindSpeed = createRequest.WindSpeed;
            WindDirection = createRequest.WindDirection;
            EnvironmentNotes = createRequest.EnvironmentNotes;
            EquipmentNotes = createRequest.EquipmentNotes;
            ShooterNotes = createRequest.ShooterNotes;
            Shots = createRequest.Shots.Convert<Shot, ShotDto>();
        }

        protected Match(MatchUpdateRequestDto matchUpdateRequest)
        {
            Id = matchUpdateRequest.Id;
            AirTemperature = matchUpdateRequest.AirTemperature;
            AirPressure = matchUpdateRequest.AirPressure;
            WindSpeed = matchUpdateRequest.WindSpeed;
            WindDirection = matchUpdateRequest.WindDirection;
            EnvironmentNotes = matchUpdateRequest.EnvironmentNotes;
            EquipmentNotes = matchUpdateRequest.EquipmentNotes;
            ShooterNotes = matchUpdateRequest.ShooterNotes;
            Shots = matchUpdateRequest.Shots.Convert<Shot, ExtendedShotDto>();
        }

        protected Match(MatchOutputDto matchOutput)
        {
            Id = matchOutput.Id;
            StartTimestamp = matchOutput.StartTimestamp;
            EndTimestamp = matchOutput.EndTimestamp;
            Location = matchOutput.Location;
            AirTemperature = matchOutput.AirTemperature;
            AirPressure = matchOutput.AirPressure;
            WindSpeed = matchOutput.WindSpeed;
            WindDirection = matchOutput.WindDirection;
            EnvironmentNotes = matchOutput.EnvironmentNotes;
            EquipmentNotes = matchOutput.EquipmentNotes;
            ShooterNotes = matchOutput.ShooterNotes;
            Shots = matchOutput.Shots.Convert<Shot, ExtendedShotDto>();
        }
    }
}
