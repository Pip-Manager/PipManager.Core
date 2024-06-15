using System.Text.Json.Serialization;
using PipManager.Core.Enums;

namespace PipManager.Core.Configuration.Models.Common;

[JsonSerializable(typeof(PackageSourceModel))]
public partial class PackageSourceModelContext : JsonSerializerContext;

public class PackageSourceModel
{
    [JsonPropertyName("default")]
    public PackageSourceType Default { get; set; } = PackageSourceType.PyPI;
    [JsonPropertyName("custom")]
    public string Custom { get; set; } = string.Empty;
}