using System.Text.Json;
using PipManager.Core.Configuration.Models.Cli;

namespace PipManager.Core.Configuration;

public static class Configuration
{
    public static readonly string DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PipManager");
    public static readonly string ConfigPath = Path.Combine(DataFolder, "config.json");
    
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = CliConfigModelContext.Default
    };
    
    public static ConfigModel? AppConfig { get; set; }

    public static void Initialize()
    {
        if (!Directory.Exists(DataFolder))
        {
            Directory.CreateDirectory(DataFolder);
        }
        
        if (!File.Exists(ConfigPath))
        {
            AppConfig = new ConfigModel();
            Save();
        }
        else
        {
            AppConfig = JsonSerializer.Deserialize(File.ReadAllText(ConfigPath), typeof(ConfigModel), CliConfigModelContext.Default) as ConfigModel;
        }
    }
    
    public static void Save()
    {
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(AppConfig, typeof(ConfigModel), SerializerOptions));
    }
}