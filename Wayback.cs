using System;
using System.Threading.Tasks;
using System.Net.Http;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace WikiLinks;

internal class Wayback
{
    private static readonly HttpClient Client = new();
    private static readonly DateTime LastBetaDate = new(2025, 11, 14, 0, 0, 0, DateTimeKind.Utc);
    
    internal static async Task<string> GetWaybackUrl(string url)
    {
        var timestamp = LastBetaDate.ToUniversalTime().ToString("yyyyMMdd");
        var apiUrl = $"https://archive.org/wayback/available?url={Uri.EscapeDataString(url)}&timestamp={timestamp}";

        try
        {
            var json = await Client.GetStringAsync(apiUrl);
            var result = JsonConvert.DeserializeObject<WaybackResponse>(json);

            return result.ArchivedSnapshots?.Closest?.Available == true ? result.ArchivedSnapshots.Closest.Url : null;
        }
        catch
        {
            return null;
        }
    }
    
    internal class ArchivedSnapshots
    {
        [JsonProperty("closest")] [CanBeNull]
        public ClosestSnapshot Closest { get; set; }
    }

    internal class ClosestSnapshot
    {
        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    internal class WaybackResponse
    {
        [JsonProperty("archived_snapshots")]
        public ArchivedSnapshots ArchivedSnapshots { get; set; }
    }
}