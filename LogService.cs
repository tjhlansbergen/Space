using System;

namespace Space;

internal class LogService
{
	public void Log(string message, string[] values, bool store = true, bool console = true, DateTime? timeStamp = null)
	{	
		var val = string.Join(", ", values);

		var timestampProvided = timeStamp.HasValue;
		timeStamp ??= DateTime.Now;
		
		if (console)
		{
			_console(message, val, timeStamp.Value, timestampProvided);
		}

		if (store)
		{
			_database(message, val, timeStamp.Value);
		}
	}

	private void _database(string message, string val, DateTime time)
	{
		var evt = new Event { Details = $"{message} {val}", TimeStamp = time };
	        DataService.CreateEvent(evt);
	}

	private void _console(string message, string val, DateTime time, bool timestampProvided)
	{

		var currColor = Console.ForegroundColor;

		if (timestampProvided)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write($"{time:yyyy-MM-dd HH:mm:ss} | ");
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write($"{time:HH:mm:ss.f} | ");
		}

		Console.ForegroundColor = currColor;
		Console.Write($"{message} ");
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(val);
		Console.ForegroundColor = currColor;
	}
}
