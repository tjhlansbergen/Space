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
		if (args[0] == "events")
		{
			WriteEvents(args.Length > 1 && int.TryParse(args[1], out var howMany) ? howMany : 42);
		}
		else
		{
			_logService.Log($"Unknown parameter {args[0]}", [], store: false, console: true);
		}
	}

	private void WriteEvents(int howMany)
	{
		var events = DataService.ReadEvents(howMany);
		foreach (var e in events)
		{
			_logService.Log(e.Details, [], store: false, console:true, e.TimeStamp);
		}
	}
}