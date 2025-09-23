using System;
using Microsoft.Extensions.DependencyInjection;

namespace Space;

public static class Program
{
	public static async Task Main(string[] args)
	{
		// set up DI container
		var services = new ServiceCollection();

		// register types httpclient & service
		services.AddHttpClient<N2yoClient>(client => client.BaseAddress = new System.Uri("https://api.n2yo.com"));
		services.AddTransient<LogService>();
		services.AddTransient<DataService>();
		services.AddTransient<MainLoop>();

		using var provider = services.BuildServiceProvider();
		
		if (args.Length > 0)
		{
			var dataService = provider.GetRequiredService<DataService>();
			Arguments(args, dataService);
		}
		else
		{
			// resolve client
			var mainLoop = provider.GetRequiredService<MainLoop>();	

			// and launch
			await mainLoop.Start();
		}
	}

	public static void Arguments(string[] args, DataService dataService)
	{
		var lines = args[0] switch {
			"events" => dataService.ReadEvents(),
			_ => new[] { $"Unknown parameter {args[0]}" },

			//TODO errors, sats, logs
		};
		
		foreach (var line in lines)
		{
			// TODO formatting and timestamps
			Console.WriteLine(line);
		}
	}
}
