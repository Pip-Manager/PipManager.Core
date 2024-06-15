using System.Text.Json;
using PipManager.Core.Configuration.Models.Cli;

namespace PipManager.Core.Configuration;

public static class CliConfiguration
{
    public static readonly string DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PipManager");
    public static readonly string ConfigPath = Path.Combine(DataFolder, "config.cli.json");
    
    public static CliConfigModel? AppConfig { get; set; }

    public static void Initialize()
    {
        if (!Directory.Exists(DataFolder))
        {
            Directory.CreateDirectory(DataFolder);
        }
        
        if (!File.Exists(ConfigPath))
        {
            AppConfig = new CliConfigModel();
            Save();
        }
        else
        {
            AppConfig = JsonSerializer.Deserialize(File.ReadAllText(ConfigPath), typeof(CliConfigModel), CliConfigModelContext.Default) as CliConfigModel;
        }
    }
    
    public static void Save()
    {
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(AppConfig, typeof(CliConfigModel),  CliConfigModelContext.Default));
    }
}