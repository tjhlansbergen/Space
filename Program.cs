using System;
using Microsoft.Extensions.DependencyInjection;

namespace Space;

public static class Program
{
	public static async Task Main()
	{
		// set up DI container
		var services = new ServiceCollection();

		// register typed httpclient
		services.AddHttpClient<N2yoClient>(client => client.BaseAddress = new System.Uri("https://api.n2yo.com"));

		// build provider, resolve client
		using var provider = services.BuildServiceProvider();
		var client = provider.GetRequiredService<N2yoClient>();
		
		var result = await client.Above(70, 18);
		Console.WriteLine(result);
	}
}
