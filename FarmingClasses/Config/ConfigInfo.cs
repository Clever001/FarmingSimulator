using System.IO;
using System.Text.Json;

namespace FarmingClasses.Config;

public class ConfigInfo {
    public bool TypeLog { get; set; }
    public string LogFilePath { get; set; } = string.Empty;
    public string SerializationFormat { get; set; } = string.Empty;

    public static ConfigInfo? UnserializeConfig() {
        if (!Path.Exists("config.json")) return null;

        try {
            string json = File.ReadAllText("config.json");
            ConfigInfo? config = JsonSerializer.Deserialize<ConfigInfo>(json);
            if (config is null || config.LogFilePath is null || config.LogFilePath == "") return null;

            return config;
        }
        catch {
            return null;
        }
    }
}
