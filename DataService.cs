using System;
using LiteDB;

namespace Space;

public class DataService
{
	private static readonly string _db = "db.db";
	private static readonly object _dbLock = new object();

	public static void CreateEvent(Event evt)
	{
		try
		{
			lock (_dbLock)
			{
				using var db = new LiteDatabase(_db);

				var events = db.GetCollection<Event>("events");
				events.Insert(evt);
			}
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
			lock (_dbLock)
			{
				using var db = new LiteDatabase(_db);

				var events = db.GetCollection<Event>("events");
				return events.Query()
					.OrderByDescending(e => e.TimeStamp)
					.Limit(howMany)
					.ToArray();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return [];
		}
	}
	public static SpaceStats GetSpaceStats()
	{
		try
		{
			lock (_dbLock)
			{
				using var db = new LiteDatabase(_db);

				var events = db.GetCollection<Event>("events");
				var sats = db.GetCollection<Sat>("sats");

				long eventCount = events.Count();
				long satCount = sats.Count();
				
				// Get all sats once and perform operations in memory to avoid multiple queries
				var allSats = sats.FindAll().ToArray();
				long categoryCount = allSats.GroupBy(s => s.Category).Count();
				long locationCount = allSats.Count(s => s.Location != null);

				return new SpaceStats
				{
					EventCount = eventCount,
					SatelliteCount = satCount,
					CategoryCount = categoryCount,
					LocationCount = locationCount
				};
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return new SpaceStats { EventCount = 0, SatelliteCount = 0, CategoryCount = 0 };
		}
	}
	public static int UpsertSat(Sat sat)
	{
		return UpsertSats(new[] { sat });
	}

	public static int UpsertSats(Sat[] satellites)
	{
		int inserted = 0;

		try
		{
			lock (_dbLock)
			{
				using var db = new LiteDatabase(_db);
				var sats = db.GetCollection<Sat>("sats");

				foreach (var sat in satellites)
				{
					var existing = sats.FindOne(s => s.SatId == sat.SatId);

					if (existing != null)
					{
						// Update
						existing.LastSeen = DateTime.UtcNow;
						existing.Location = sat.Location;
						sats.Update(existing);
					}
					else
					{
						// Insert new record
						sats.Insert(sat);
						inserted++;
					}
				}
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
			lock (_dbLock)
			{
				using var db = new LiteDatabase(_db);

				var sats = db.GetCollection<Sat>("sats");
				return sats.Query()
					.OrderBy(s => s.SatId)
					.Limit(howMany)
					.ToArray();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
			return [];
		}
	}
}
