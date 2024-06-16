using PipManager.Core.Configuration.Models;
using PipManager.Core.Enums;
using static PipManager.Core.Configuration.Configuration;

namespace PipManager.Core.PyEnvironment;

public static class Search
{
    public static EnvironmentModel? FindEnvironmentByIdentifier(string identifier)
    {
        var environments = AppConfig!.Environments;
        var environment = environments.FirstOrDefault(x => x.Identifier == identifier);
        return environment;
    }
    
    public static EnvironmentModel? FindEnvironmentByPythonPath(string pythonPath)
    {
        var environments = AppConfig!.Environments;
        var environment = environments.FirstOrDefault(x => x.PythonPath == pythonPath);
        return environment;
    }
}