using System;
using System.Diagnostics;

namespace Space;

internal class MainLoop
{
	private const int DELAY_MS = 10_000;
	private const int CATEGORY_COUNT = 57;

	private readonly N2yoClient _client;
	private readonly LogService _logService;
	private readonly Random _rnd;

	public MainLoop(N2yoClient client, LogService logService)
	{
		_client = client;
		_logService = logService;
		_rnd = new Random();
	}

	public async Task Start()
	{
		_logService.Log("🚀 Main loop started on", new [] { System.Environment.MachineName }); 

		while (true)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var category = _rnd.Next(1, CATEGORY_COUNT + 1);
			var result = await _client.Above(70, category);

			var message = string.Empty;
			var values = Array.Empty<string>();
			var sats = Array.Empty<Sat>();

			if (result?.Above != null && result.Above.Count > 0)
			{
				sats = Fill(result).ToArray();
				message = $"Got {sats.Length} sat(s) of category '{sats[0].Category}'";
				values = sats.Select(s => s.SatName).ToArray();

			}
			else
			{
				message = $"Got zero (0) sats for category {category}";
			}

			_logService.Log(message, values, store: true, console: true);
			
			if (sats.Length > 0)
			{
				var inserted = 0;

				// Store
				foreach(var sat in sats)
				{
					inserted += DataService.UpsertSat(sat);
				}

				_logService.Log($"Updated {sats.Length - inserted} satellites, new satellites inserted:", new [] { inserted.ToString() }, store: true, console: true);
			}
			stopwatch.Stop();

			var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
			_logService.Log($"Duration: {elapsedMs}ms, transaction count: {result?.Info?.TransactionsCount ?? 0}", [], store: false, console: true);

			await Task.Delay(DELAY_MS - (int)elapsedMs);
		}
	}

	private IEnumerable<Sat> Fill(AboveResult result)
	{
		return result.Above.Select(s => new Sat
		{
			SatId = s.SatId,
			SatName = s.SatName,
			IntDesignator = s.IntDesignator,
			LaunchDate = s.LaunchDate,
			FirstSeen = DateTime.UtcNow,
			LastSeen = DateTime.UtcNow,
			Category = result?.Info?.Category ?? string.Empty,
		});
	}
}
