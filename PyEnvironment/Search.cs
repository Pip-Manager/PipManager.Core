using PipManager.Core.Configuration.Models.Common;
using PipManager.Core.Enums;
using static PipManager.Core.Configuration.Configuration;

namespace PipManager.Core.PyEnvironment;

public static class Search
{
    public static Response<EnvironmentModel?> FindEnvironmentByIdentifier(string identifier)
    {
        var environments = AppConfig!.Environments;
        var environment = environments.FirstOrDefault(x => x.Identifier == identifier);
        return environment == null ? new Response<EnvironmentModel?>(null, ResponseMessageType.Error) : new Response<EnvironmentModel?>(environment, ResponseMessageType.Success);
    }
    
    public static Response<EnvironmentModel?> FindEnvironmentByPythonPath(string pythonPath)
    {
        var environments = AppConfig!.Environments;
        var environment = environments.FirstOrDefault(x => x.PythonPath == pythonPath);
        return environment == null ? new Response<EnvironmentModel?>(null, ResponseMessageType.Error) : new Response<EnvironmentModel?>(environment, ResponseMessageType.Success);
    }
}