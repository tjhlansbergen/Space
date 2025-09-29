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
		var howMany = (args.Length > 1 && int.TryParse(args[1], out var i) ? i : 42);

		switch (args[0])
		{
			case "events":
				WriteEvents(howMany);
				break;

			case "sats":
				WriteSats(howMany);
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
			Console.WriteLine(group.Key);
			
			foreach (var s in group.OrderBy(s => s.SatName))
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write($"\t{s.SatId}");
				Console.ForegroundColor = ConsoleColor.DarkBlue;
				Console.Write($"\t- {s.SatName}");
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine($"\t🚀: {Fd(s.LaunchDate)}, 📡: {Fd(s.FirstSeen)}, 🛰️: {Fd(s.LastSeen)}");
			}
		}

		Console.ForegroundColor = currColor;
	}

	private static string Fd(DateTime dt)
	{
		return dt.ToString("yyyy-MM-dd");
	}

}
