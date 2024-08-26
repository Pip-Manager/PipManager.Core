using System.Collections.ObjectModel;
using System.Text.Json;
using PipManager.Core.Configuration.Models;
using PipManager.Core.PyEnvironment;
using CliConfigModelContext = PipManager.Core.Configuration.Models.CliConfigModelContext;

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
    
    public static ConfigModel? AppConfig { get; private set; }

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
    
    public static void UpdateSelectedEnvironment()
    {
        if(AppConfig!.SelectedEnvironment == null)
        {
            return;
        }
        var updatedEnvironment = Detector.ByPythonPath(AppConfig.SelectedEnvironment.PythonPath)!;
        var currentEnvironmentIndex = AppConfig.Environments.FindIndex(x => x.PythonPath == AppConfig.SelectedEnvironment.PythonPath);
        AppConfig.Environments[currentEnvironmentIndex] = updatedEnvironment;
        AppConfig.SelectedEnvironment = updatedEnvironment;
        Save();
    }
    
    public static void Save()
    {
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(AppConfig, typeof(ConfigModel), SerializerOptions));
    }
    
    public static readonly ReadOnlyDictionary<string, string> PackageSources = new(new Dictionary<string, string>
    {
        ["default"] = "https://pypi.org/simple",
        ["tsinghua"] = "https://pypi.tuna.tsinghua.edu.cn/simple",
        ["aliyun"] = "https://mirrors.aliyun.com/pypi/simple",
        ["douban"] = "https://pypi.doubanio.com/simple"
    });
}