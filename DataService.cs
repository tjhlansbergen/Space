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

	public static int UpsertSat(Sat sat)
	{
		int inserted = 0;

		try
		{
			using var db = new LiteDatabase(_db);

			var sats = db.GetCollection<Sat>("sats");
			var existing = sats.FindOne(s => s.SatId == sat.SatId);

    			if (existing != null)
    			{
        			// Update
        			existing.LastSeen = DateTime.UtcNow;
        			sats.Update(existing);
    			}
    			else
    			{
        			// Insert new record
        			sats.Insert(sat);
				inserted = 1;
    			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}

		return inserted;
	}	
	
	public static Sat[] ReadSats(int howMany)
	{
		try
		{
			using var db = new LiteDatabase(_db);

			var sats = db.GetCollection<Sat>("sats");
			return sats.Query()
				.OrderBy(s => s.SatId)
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
