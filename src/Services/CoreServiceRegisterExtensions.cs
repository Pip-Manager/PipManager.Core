using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace PipManager.Core.Services;

public static class CoreServiceRegisterExtensions
{
    public static void AddHttpClient(this IServiceCollection services, string appVersion)
    {
        services.AddTransient(_ =>
        {
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.All }) { DefaultRequestVersion = HttpVersion.Version20 };
            client.DefaultRequestHeaders.Add("User-Agent", $"PipManager/{appVersion}");
            client.Timeout = TimeSpan.FromSeconds(6);
            return client;
        });
    }
}