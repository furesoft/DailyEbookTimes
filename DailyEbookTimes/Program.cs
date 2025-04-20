using System.Diagnostics;
using System.Globalization;
using CodeHollow.FeedReader;
using Moss.NET.Sdk.DataSources;
using Moss.NET.Sdk.LayoutEngine;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk;

class Program
{
    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        string[] feeds = [
            "https://www.heise.de/rss/heise-Rubrik-IT.rdf",
            "https://rss.nytimes.com/services/xml/rss/nyt/World.xml"];

        foreach (string url in feeds)
        {
            var feed = FeedReader.Read(url);
            Feeds.Add(feed);
        }

        var builder = new PdfDocumentBuilder();

        builder.DocumentInformation.Producer = "Totletheyn";
        builder.DocumentInformation.Title = "Issue 1";
        builder.DocumentInformation.CreationDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

        Layout.Builder = builder;
        Layout.AddFont("Default", "Assets/fonts/NoticiaText-Regular.ttf");
        Layout.AddFont("Jaini", "Assets/fonts/Jaini-Regular.ttf");
        Layout.AddFont("NoticiaText", "Assets/fonts/NoticiaText-Regular.ttf");

        Layout.PathResolver.Base = "Assets/";
        LayoutLoader.AddDataSource<WeatherDataSource>();
        LayoutLoader.AddDataSource<XkcdDataSource>();
        LayoutLoader.AddDataSource<NasaDataSource>();
        LayoutLoader.AddDataSource<JokeDataSource>();
        LayoutLoader.AddDataSource<ComicDataSource>();
        LayoutLoader.AddDataSource<Meta>();

        var coverLayout = LayoutLoader.Load("layouts/cover.xml");
        //coverLayout.EnableDebugLines();
        coverLayout.Apply();

        var contentLayout = LayoutLoader.Load("layouts/content.xml");
        contentLayout.Name = "Page 1";
        contentLayout.Apply();

        AddBookmarks(coverLayout, contentLayout);
        var documentBytes = builder.Build();

        File.WriteAllBytes("newPdf.pdf", documentBytes);
        Process.Start(new ProcessStartInfo("newPdf.pdf") { UseShellExecute = true });
    }

    private static void AddBookmarks(params Layout[] layouts)
    {
        var nodes = new List<DocumentBookmarkNode>();

        foreach (var layout in layouts)
        {
            nodes.Add(new DocumentBookmarkNode(layout.Name, 0,
                new ExplicitDestination(layout.Page!.PageNumber, ExplicitDestinationType.FitPage,
                    ExplicitDestinationCoordinates.Empty),
                [])
            );
        }

        Layout.Builder.Bookmarks = new(nodes);
    }

    public static List<Feed> Feeds { get; set; } = [];
}