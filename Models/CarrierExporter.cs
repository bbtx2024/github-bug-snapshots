using System.IO;
using Newtonsoft.Json;
using Quick.Models;

public static class CarrierExporter
{
    public static void SaveSnapshotsToJsonFile(Carrier carrier, string folderPath = "logs")
    {
        Directory.CreateDirectory(folderPath); 

        string fileName = $"{carrier.SnCode ?? "Unknown"}_{carrier.CreatedAt:yyyyMMdd_HHmmss}.json";
        string fullPath = Path.Combine(folderPath, fileName);

        var exportData = new
        {
            CarrierSn = carrier.SnCode,
            CreatedAt = carrier.CreatedAt,
            Snapshots = carrier.SnapshotHistory
        };

        File.WriteAllText(fullPath, JsonConvert.SerializeObject(exportData, Formatting.Indented));
    }
}