using System;
using LiteDB;

namespace Space;

public class DataService
{
	private static readonly string _db = "db.db";

	public void CreateEvent(Event evt)
	{
		try
		{
			var db = new LiteDatabase(_db);

			var events = db.GetCollection<Event>("events");
			events.Insert(evt);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}

	public string[] ReadEvents()
	{
		try
		{
			var db = new LiteDatabase(_db);

			var events = db.GetCollection<Event>("events");
			return events.Query()
				.OrderByDescending(e => e.TimeStamp)
				.Limit(42)
				.ToList()
				.Select(e => e.Details)
				.ToArray();	
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return new string[0];
		}
	}
}
