using System;
using System.Diagnostics;

namespace Space;

internal class MainLoop
{
	private readonly N2yoClient _client;
	private readonly LogService _logService;

	public MainLoop(N2yoClient client, LogService logService)
	{
		_client = client;
		_logService = logService;
	}

	public async Task Start()
	{
		_logService.Log("🚀 Main loop started on", new [] { System.Environment.MachineName }); 

		while (true)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var result = await _client.Above(70, 18);
			var message = $"Got {result.Info?.SatCount ?? 0} sat(s) in category {result.Info?.Category ?? "unknown"}";
			var values = result.Above.Select(a => a.SatName).ToArray();

			_logService.Log(message, values, true);
			stopwatch.Stop();

			var elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
			_logService.Log($"Duration: {elapsedMs}ms, transaction count: {result.Info.TransactionsCount}", new string[0]);

			await Task.Delay(60_000 - (int)elapsedMs);
		}
	}
}
