using System;
using LiteDB;

namespace Space;

internal class DataService
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
}
