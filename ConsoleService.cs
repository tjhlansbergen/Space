using System;

namespace Space;

internal class ConsoleService
{
	private readonly LogService _logService;

	public ConsoleService(LogService logService)
	{
		_logService = logService;
	}

	public void Go(string[] args)
	{
		var howMany = args.Length > 1 && int.TryParse(args[1], out var i) ? i : 42;

		switch (args[0])
		{
			case "events":
				WriteEvents(howMany);
				break;

			case "sats":
				WriteSats(howMany);
				break;

			case "stats":
				WriteStats();
				break;

			default:
				_logService.Log($"Unknown parameter {args[0]}", [], store: false, console: true);
				break;
		}
	}

	private void WriteEvents(int howMany)
	{
		var events = DataService.ReadEvents(howMany);
		foreach (var e in events)
		{
			_logService.Log(e.Details, [], store: false, console: true, e.TimeStamp);
		}
	}

	private void WriteSats(int howMany)
	{
		var currColor = Console.ForegroundColor;

		var sats = DataService.ReadSats(howMany);
		var groups = sats.GroupBy(s => s.Category).OrderBy(g => g.Key);

		foreach (var group in groups)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine();
			Console.WriteLine(group.Key);

			foreach (var s in group.OrderBy(s => s.SatName))
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write($"\t{s.SatId,-8}");
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.Write($"- {s.SatName,-25}");
				Console.ForegroundColor = ConsoleColor.White;
				var locationIcon = s.Location != null ? " 🌍" : "";
				Console.WriteLine($"🚀 {Fd(s.LaunchDate)} 📡 {Fd(s.FirstSeen)} 🛰️  {Fd(s.LastSeen)} {locationIcon}");
			}
		}

		Console.ForegroundColor = currColor;
	}

	private void WriteStats()
	{
		var stats = DataService.GetSpaceStats();

		if (stats == null)
		{
			return;
		}

		var currColor = Console.ForegroundColor;

		Console.WriteLine();

		Console.Write("Satellites:".PadRight(25));
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(stats.SatelliteCount);

		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("  - with location:".PadRight(25));
		Console.ForegroundColor = ConsoleColor.DarkBlue;
		Console.WriteLine(stats.LocationCount);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.Write("  - categories:".PadRight(25));
		Console.ForegroundColor = ConsoleColor.DarkBlue;
		Console.WriteLine(stats.CategoryCount);

		Console.ForegroundColor = currColor;
		Console.Write("Events:".PadRight(25));
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(stats.EventCount);

		Console.ForegroundColor = currColor;

		Console.WriteLine();
	}

	private static string Fd(DateTime dt)
	{
		return dt.ToString("yyyy-MM-dd HH:mm");
	}

}
