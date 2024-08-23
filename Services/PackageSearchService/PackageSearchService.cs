﻿using HtmlAgilityPack;
using PipManager.Core.Wrappers.PackageSearchQueryWrapper;

namespace PipManager.Core.Services.PackageSearchService;

public class PackageSearchService(HttpClient httpClient) : IPackageSearchService
{
    private Dictionary<(string, int), QueryWrapper> QueryCaches { get; } = [];

    public async ValueTask<QueryWrapper> Query(string name, int page = 1)
    {
        if (QueryCaches.ContainsKey((name, page)))
        {
            return QueryCaches[(name, page)];
        }
        string htmlContent;
        try
        {
            htmlContent = await httpClient.GetStringAsync($"https://pypi.org/search/?q={name}&page={page}");
        }
        catch (Exception exception) when (exception is TaskCanceledException or HttpRequestException)
        {
            return new QueryWrapper
            {
                Status = QueryStatus.Timeout
            };
        }
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        var queryWrapper = new QueryWrapper
        {
            ResultCount = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='left-layout__main']//strong").InnerText
        };
        queryWrapper.Status = queryWrapper.ResultCount != "0" ? QueryStatus.Success : QueryStatus.NoResults;
        if (queryWrapper.Status == QueryStatus.NoResults)
        {
            return queryWrapper;
        }
        var pageNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'button-group')]");
        queryWrapper.MaxPageNumber = pageNode == null ? 1 : int.Parse(pageNode.ChildNodes[^4].InnerText);

        try
        {
            var resultList = htmlDocument.DocumentNode.SelectSingleNode("//ul[@aria-label='Search results']").ChildNodes.Where(result => result.InnerLength != 15).Select(result => result.ChildNodes[1]);
            queryWrapper.Results = [];
            foreach (var resultItem in resultList)
            {
                queryWrapper.Results.Add(new QueryListItemModel
                {
                    Name = resultItem.ChildNodes[1].ChildNodes[1].InnerText,
                    Version = resultItem.ChildNodes[1].ChildNodes[3].InnerText,
                    Description = resultItem.ChildNodes[3].InnerText,
                    Url = $"https://pypi.org{resultItem.Attributes["href"].Value}",
                    // ReSharper disable once StringLiteralTypo
                    UpdateTime = DateTime.ParseExact(resultItem.ChildNodes[1].ChildNodes[5].ChildNodes[0].Attributes["datetime"].Value, "yyyy-MM-ddTHH:mm:sszzz", null, System.Globalization.DateTimeStyles.RoundtripKind)
                });
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            QueryCaches.Add((name, page), queryWrapper);
            return queryWrapper;
        }
        QueryCaches.Add((name, page), queryWrapper);
        return queryWrapper;
    }
}