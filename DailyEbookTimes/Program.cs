using System.Diagnostics;
using System.Globalization;
using CodeHollow.FeedReader;

namespace Moss.NET.Sdk;

class Program
{
    static async Task Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        string[] feeds = [
            "https://www.heise.de/rss/heise-Rubrik-IT.rdf",
            "https://rss.golem.de/rss.php?ms=softwareentwicklung&feed=RSS2.0",
            "https://css-tricks.com/feed/",
            "https://dev.to/feed/"
        ];

        var newspaper = new Newspaper(1, "furesoft");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
        httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");

        foreach (var url in feeds)
        {
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();
                var feed = FeedReader.ReadFromByteArray(data);
                newspaper.AddFeed(feed);
            }
        }

        var documentBytes = newspaper.Render();

        var samplePdf = "../../../Sample.pdf";
        File.WriteAllBytes(samplePdf, documentBytes);
        Process.Start(new ProcessStartInfo(Path.GetFullPath(samplePdf)) { UseShellExecute = true });
    }
}