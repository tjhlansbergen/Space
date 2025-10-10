using System.Net.Http;
using System.Net.Http.Json;

namespace Space;

internal class N2yoClient
{
	const string API = "rest/v1/satellite";
	const string KEY = "&apiKey=6YY9ZH-N68FKZ-V56AWM-5KL7";

	private readonly HttpClient _httpClient;
	private readonly LogService _logService;

	public N2yoClient(HttpClient httpClient, LogService logService)
	{
		_httpClient = httpClient;
		_logService = logService;
	}

	public async Task<AboveResult?> Above(int search_radius, int category_id)
	{
		var path = "/above";
		var coord = GenerateRandomGpsCoordinate();
		var url = $"{_httpClient.BaseAddress}{API}{path}/{coord.Latitude}/{coord.Longitude}/0/{search_radius}/{category_id}/";

		_logService.Log("Calling: ", new[] { url }, store: false, console: true );

		var result = await _httpClient.GetFromJsonAsync<AboveResult>(url + KEY);
		return result;
	}

	public static (double Latitude, double Longitude) GenerateRandomGpsCoordinate()
	{
		var random = new Random();
		
		// Latitude: -90 to +90 degrees, 3 decimal places
		var latitude = Math.Round((random.NextDouble() * 180.0) - 90.0, 3);
		
		// Longitude: -180 to +180 degrees, 3 decimal places
		var longitude = Math.Round((random.NextDouble() * 360.0) - 180.0, 3);
		
		return (latitude, longitude);
	}
	
}
