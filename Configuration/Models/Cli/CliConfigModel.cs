using System.Text.Json.Serialization;
using PipManager.Core.Configuration.Models.Common;

namespace PipManager.Core.Configuration.Models.Cli;

[JsonSerializable(typeof(CliConfigModel))]
public partial class CliConfigModelContext : JsonSerializerContext;

public class CliConfigModel
{
    public List<EnvironmentModel> Environments { get; set; } = [];
}