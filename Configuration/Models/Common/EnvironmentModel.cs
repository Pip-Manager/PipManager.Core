using System.Text.Json.Serialization;

namespace PipManager.Core.Configuration.Models.Common;

[JsonSerializable(typeof(EnvironmentModel))]
public partial class EnvironmentModelContext : JsonSerializerContext;

public class EnvironmentModel
{
    public required string PythonPath { get; set; }
    public required string PipVersion { get; set; }
    public required string PythonVersion { get; set; }
}