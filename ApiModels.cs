using System.Text.Json.Serialization;

public record AboveResult(
    Info Info,
    List<Above> Above
);

public record Info(
    string Category,
    int TransactionsCount,
    int SatCount
);

public record Above(
    int SatId,
    string SatName,
    string IntDesignator,
    DateTime LaunchDate,
    double SatLat,
    double SatLng,
    double SatAlt
);
