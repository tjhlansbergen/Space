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
		services.AddTransient<MainLoop>();
		services.AddTransient<ConsoleService>();

		using var provider = services.BuildServiceProvider();
		
		if (args.Length > 0)
		{
			var consoleService = provider.GetRequiredService<ConsoleService>();
			consoleService.Go(args);
		}
		else
		{
			// resolve client
			var mainLoop = provider.GetRequiredService<MainLoop>();	

			// and launch
			await mainLoop.Start();
		}
	}
}
