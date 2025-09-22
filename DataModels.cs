using System;

namespace Space;

public record Event
{
	public int Id { get; init; }
	public DateTime TimeStamp { get; init; } = DateTime.UtcNow;
	public required string Details { get; init; }
}

