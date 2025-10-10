using Microsoft.Extensions.DependencyInjection;

namespace Space;

public static class Program
{
	public static async Task Main(string[] args)
	{
		// Check if running in web mode
		if (args.Length > 0 && args[0] == "web")
		{
			await RunWebMode(args);
		}
		else if (args.Length > 0)
		{
			RunConsoleMode(args);
		}
		else
		{
			await RunMainLoop();
		}
	}

	private static async Task RunWebMode(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		
		// Add services
		builder.Services.AddHttpClient<N2yoClient>(client => 
			client.BaseAddress = new Uri("https://api.n2yo.com"));
		builder.Services.AddTransient<LogService>();
		builder.Services.AddTransient<MainLoop>();
		builder.Services.AddTransient<ConsoleService>();
		
		// Add CORS
		builder.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
					  .AllowAnyMethod()
					  .AllowAnyHeader();
			});
		});

		var app = builder.Build();
		
		// Configure pipeline
		app.UseCors();
		app.UseDefaultFiles();  // Enable default file mapping (index.html)
		app.UseStaticFiles();
		
		// API endpoints
		app.MapGet("/api/stats", () => DataService.GetSpaceStats());
		
		app.MapGet("/api/satellites", (int? limit) => 
			DataService.ReadSats(limit ?? int.MaxValue));
			
		app.MapGet("/api/events", (int? limit) => 
			DataService.ReadEvents(limit ?? 50));

		// Start background service
		var mainLoop = app.Services.GetRequiredService<MainLoop>();
		_ = Task.Run(async () => await mainLoop.Start());

		await app.RunAsync();
	}

	private static void RunConsoleMode(string[] args)
	{
		var services = new ServiceCollection();
		services.AddHttpClient<N2yoClient>(client => 
			client.BaseAddress = new Uri("https://api.n2yo.com"));
		services.AddTransient<LogService>();
		services.AddTransient<ConsoleService>();

		using var provider = services.BuildServiceProvider();
		var consoleService = provider.GetRequiredService<ConsoleService>();
		consoleService.Go(args);
	}

	private static async Task RunMainLoop()
	{
		var services = new ServiceCollection();
		services.AddHttpClient<N2yoClient>(client => 
			client.BaseAddress = new Uri("https://api.n2yo.com"));
		services.AddTransient<LogService>();
		services.AddTransient<MainLoop>();

		using var provider = services.BuildServiceProvider();
		var mainLoop = provider.GetRequiredService<MainLoop>();
		await mainLoop.Start();
	}
}
