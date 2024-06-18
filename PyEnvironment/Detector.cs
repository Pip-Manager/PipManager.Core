using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using PipManager.Core.Configuration.Models;
using PipManager.Core.Enums;
using PipManager.Core.PyEnvironment.Helpers;

namespace PipManager.Core.PyEnvironment;

public static class Detector
{
    public static List<EnvironmentModel> ByEnvironmentVariable()
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
                if (environmentItem == null)
                {
                    continue;
                }
                environmentItems.Add(environmentItem);
            }
        }
        else
        {
            throw new PlatformNotSupportedException("OS is not supported.");
        }
        return environmentItems;
    }
    
    public static EnvironmentModel? ByPythonPath(string pythonPath)
    {
        if (!File.Exists(pythonPath))
        {
            throw new FileNotFoundException("Python path is not found.");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return WindowsSpecified.GetEnvironment(pythonPath);
        }
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return UnixSpecified.GetEnvironment(pythonPath);
        }
        throw new PlatformNotSupportedException("OS is not supported.");
    }
}