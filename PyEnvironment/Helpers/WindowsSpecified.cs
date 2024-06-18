using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using PipManager.Core.Configuration.Models;

namespace PipManager.Core.PyEnvironment.Helpers;


[SupportedOSPlatform("Windows")]
public static partial class WindowsSpecified
{
    public static EnvironmentModel? GetEnvironment(string pythonPath)
    {
        var pythonVersion = FileVersionInfo.GetVersionInfo(pythonPath).FileVersion!;
        var pipDirectory = GetPackageDirectory(Directory.GetParent(pythonPath)!.FullName);

        if (pipDirectory is null)
        {
            return null;
        }
        
        pipDirectory = Path.Combine(pipDirectory, "pip");
        var pipVersion = Common.GetPipVersionInInitFile().Match(File.ReadAllText(Path.Combine(pipDirectory, "__init__.py"))).Groups[1].Value;
        return new EnvironmentModel { Identifier = "", PipVersion = pipVersion, PythonPath = pythonPath, PythonVersion = pythonVersion};
    }
    
    private static string? GetPackageDirectory(string pythonDirectory)
    {
        var sitePackageDirectory = Path.Combine(pythonDirectory, @"Lib\site-packages");
        return !Directory.Exists(sitePackageDirectory) ? null : sitePackageDirectory;
    }
}