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
	public int Id { get; init; } 	// db internal id
	public required int SatId { get; init; } // real life id retrieved from the api
	public required string SatName { get; init; } 
	public required string IntDesignator { get; init; } 
	public required DateTime LaunchDate { get; init; }
        public required DateTime FirstSeen { get; init; } 	
	public required DateTime LastSeen { get; set; }
	public required string Category { get; init; } 
}
