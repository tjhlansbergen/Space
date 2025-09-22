using System;

namespace Space;

internal class LogService
{
	private DataService _dataService;

	public LogService(DataService dataService)
	{
		_dataService = dataService;
	}

	public void Log(string message, string[] values, bool console = true)
	{	
		var val = string.Join(", ", values);
		var now = DateTime.UtcNow;
		
		if (console)
		{
			_console(message, val, now);
		}

		_database(message, val, now);
	}

	private void _database(string message, string val, DateTime time)
	{
		var evt = new Event { Details = $"{message} {val}", TimeStamp = time };
		_dataService.CreateEvent(evt);
	}

	private void _console(string message, string val, DateTime time)
	{

		var currColor = Console.ForegroundColor;

		Console.ForegroundColor = ConsoleColor.Green;
		Console.Write($"{time.TimeOfDay} | ");
		Console.ForegroundColor = currColor;
		Console.Write(message);
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(val);
		Console.ForegroundColor = currColor;
	}
}
