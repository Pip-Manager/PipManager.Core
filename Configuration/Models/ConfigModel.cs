﻿using System.Text.Json.Serialization;
using PipManager.Core.Configuration.Models.Common;

namespace PipManager.Core.Configuration.Models.Cli;

[JsonSerializable(typeof(ConfigModel))]
public partial class CliConfigModelContext : JsonSerializerContext;

public class ConfigModel
{
    [JsonPropertyName("selectedEnvironment")]
    public EnvironmentModel? SelectedEnvironment { get; set; }
    [JsonPropertyName("environments")]
    public List<EnvironmentModel> Environments { get; set; } = [];
    [JsonPropertyName("packageSource")]
    public PackageSourceModel PackageSource { get; set; } = new();
}