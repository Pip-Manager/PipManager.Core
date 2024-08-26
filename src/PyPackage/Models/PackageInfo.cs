using System.Text.Json.Serialization;

namespace PipManager.Core.PyPackage.Models;

public class PackageInfo
{
    [JsonPropertyName("releases")]
    public Dictionary<string, List<PackageInfoRelease>>? Releases;
}

public class PackageInfoRelease
{
    [JsonPropertyName("filename")]
    public string? Filename;

    [JsonPropertyName("upload_time")]
    public string? UploadTime;

    [JsonPropertyName("digests")]
    public PackageInfoDigest? Digests;
}

public class PackageInfoDigest
{
    [JsonPropertyName("blake2b_256")]
    public string? Blake2B256;

    [JsonPropertyName("md5")]
    public string? Md5;

    [JsonPropertyName("sha256")]
    public string? Sha256;
}