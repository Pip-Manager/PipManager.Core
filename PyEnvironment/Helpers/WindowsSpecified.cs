using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using PipManager.Core.Configuration.Models.Common;

namespace PipManager.Core.PyEnvironment.Helpers;

public static partial class WindowsSpecified
{
    [SupportedOSPlatform("Windows")]
    public static EnvironmentModel? GetEnvironment(string pythonPath)
    {
        var pythonVersion = FileVersionInfo.GetVersionInfo(pythonPath).FileVersion!;
        var pythonDirectory = Directory.GetParent(pythonPath)!.FullName;
        var pipDirectory = GetPipDirectories(pythonDirectory);

        if (pipDirectory == null)
        {
            return null;
        }

        var pipVersion = GetPipVersionInInitFile().Match(File.ReadAllText(Path.Combine(pipDirectory, "__init__.py"))).Groups[1].Value;
        return new EnvironmentModel { Identifier = "", PipVersion = pipVersion, PythonPath = pythonDirectory, PythonVersion = pythonVersion};
    }
    
    private static string? GetPipDirectories(string pythonDirectory)
    {
        var sitePackageDirectory = Path.Combine(pythonDirectory, @"Lib\site-packages");
        if (!Directory.Exists(sitePackageDirectory))
        {
            return null;
        }

        var pipDirectory = Path.Combine(sitePackageDirectory, "pip");
        return !Directory.Exists(pipDirectory) ? null : pipDirectory;
    }
    
    [GeneratedRegex("__version__ = \"(.*?)\"", RegexOptions.IgnoreCase, "zh-CN")]
    private static partial Regex GetPipVersionInInitFile();
}