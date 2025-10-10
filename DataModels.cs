using System;

namespace Space;

public record Event
{
	public int Id { get; init; }
	public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
	public required string Details { get; init; }
}

public record Sat
{
	public int Id { get; init; }    // db internal id
	public required int SatId { get; init; } // real life id retrieved from the api
	public required string SatName { get; init; }
	public required string IntDesignator { get; init; }
	public required DateTime LaunchDate { get; init; }
	public required DateTime FirstSeen { get; init; }
	public required DateTime LastSeen { get; set; }
	public required string Category { get; init; }
	public SatLocation? Location { get; set; }
}

public record SatLocation
{
	public required double Latitude { get; init; }
	public required double Longitude { get; init; }
	public required double Altitude { get; init; }
}

public record SpaceStats
{
	public long SatelliteCount { get; init; }
	public long CategoryCount { get; init; }
	public long LocationCount { get; init; }

	public long EventCount { get; init; }
}