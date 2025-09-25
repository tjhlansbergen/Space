using System;
using LiteDB;

namespace Space;

public class DataService
{
	private static readonly string _db = "db.db";

	public static void CreateEvent(Event evt)
	{
		try
		{
			using var db = new LiteDatabase(_db);

			var events = db.GetCollection<Event>("events");
			events.Insert(evt);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}

	public static Event[] ReadEvents(int howMany)
	{
		try
		{
			using var db = new LiteDatabase(_db);

			var events = db.GetCollection<Event>("events");
			return events.Query()
				.OrderBy(e => e.TimeStamp)
				.Limit(howMany)
				.ToArray();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return [];
		}
	}
}
