using System;
using Microsoft.Extensions.DependencyInjection;

namespace Space;

public static class Program
{
	public static async Task Main()
	{
		// set up DI container
		var services = new ServiceCollection();

		// register types httpclient & service
		services.AddHttpClient<N2yoClient>(client => client.BaseAddress = new System.Uri("https://api.n2yo.com"));
		services.AddTransient<LogService>();
		services.AddTransient<DataService>();
		services.AddTransient<MainLoop>();

		// build provider, resolve client
		using var provider = services.BuildServiceProvider();
		var mainLoop = provider.GetRequiredService<MainLoop>();	

		// and launch
		await mainLoop.Start();
	}
}
