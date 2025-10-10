# Space Tracker Web Dashboard

## Running the Application

### Console Mode (Original)
```bash
# Run the main loop (collect satellite data)
dotnet run

# View satellites
dotnet run -- sats

# View statistics  
dotnet run -- stats

# View events
dotnet run -- events
```

### Web Dashboard Mode
```bash
# Start web server with dashboard
dotnet run -- web
```

Then open your browser to: `http://localhost:5000`

## Web Dashboard Features

- **Real-time Statistics**: Total satellites, satellites with location data, categories, and events
- **Live Satellite List**: Grouped by category with location indicators (🌍)
- **Recent Events**: Latest activity and updates
- **Auto-refresh**: Updates every minute automatically
- **Responsive Design**: Works on desktop and mobile devices

## API Endpoints

The web mode exposes the following REST endpoints:

- `GET /api/stats` - Returns satellite and event statistics
- `GET /api/satellites?limit=100` - Returns satellite list (default limit: 100)
- `GET /api/events?limit=50` - Returns recent events (default limit: 50)

## Cross-Platform Compatibility

Works on:
- Windows (PowerShell, CMD)
- Linux (bash, zsh)
- macOS (Terminal)

## Dependencies

- .NET 9.0
- LiteDB for data storage
- ASP.NET Core for web hosting
- Vanilla JavaScript (no external frameworks required)