using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClasses.Config;

[JsonObject(MemberSerialization.OptIn)]
public class ConfigInfo {
    [JsonProperty]
    public bool TypeLog { get; set; }
    [JsonProperty]
    public string LogFilePath { get; set; } = string.Empty;

    public static ConfigInfo? UnserializeConfig() {
        if (!Path.Exists("config.json")) return null;

        try {
            string json = File.ReadAllText("config.json");
            ConfigInfo? config = JsonConvert.DeserializeObject<ConfigInfo>(json);
            if (config is null || config.LogFilePath is null || config.LogFilePath == "") return null;

            return config;
        }
        catch {
            return null;
        }
    }
}
