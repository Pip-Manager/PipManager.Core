using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using PipManager.Core.Configuration.Models;
using PipManager.Core.Enums;
using PipManager.Core.PyEnvironment.Helpers;

namespace PipManager.Core.PyEnvironment;

public static class Detector
{
    public static Response<List<EnvironmentModel>> ByEnvironmentVariable()
    {
        var environmentItems = new List<EnvironmentModel>();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var value = Environment.GetEnvironmentVariable("Path")!.Split(';');
            foreach (var item in value)
            {
                if (!File.Exists(Path.Combine(item, "python.exe")))
                    continue;
                var environmentItem = WindowsSpecified.GetEnvironment(Path.Combine(item, "python.exe"));
                if (environmentItem == null) continue;
                environmentItems.Add(environmentItem);
            }
        }
        else
        {
            return new Response<List<EnvironmentModel>>([], ResponseMessageType.OsNotSupported);
        }
        return new Response<List<EnvironmentModel>>(environmentItems, ResponseMessageType.Success);
    }
    
    public static Response<EnvironmentModel?> ByPythonPath(string pythonPath)
    {
        if (!File.Exists(pythonPath))
        {
            return new Response<EnvironmentModel?>(null, ResponseMessageType.Error, "Python path does not exist");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new Response<EnvironmentModel?>(WindowsSpecified.GetEnvironment(pythonPath), ResponseMessageType.Success);
        }
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return new Response<EnvironmentModel?>(UnixSpecified.GetEnvironment(pythonPath), ResponseMessageType.Success);
        }
        return new Response<EnvironmentModel?>(null, ResponseMessageType.OsNotSupported);
    }
}