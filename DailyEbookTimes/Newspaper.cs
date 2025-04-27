using CodeHollow.FeedReader;
using Moss.NET.Sdk.DataSources;
using Moss.NET.Sdk.DataSources.Crypto;
using Moss.NET.Sdk.LayoutEngine;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk;

public class Newspaper
{
    private List<Feed> Feeds { get; } = [];

    private readonly PdfDocumentBuilder _builder = new();
    private readonly int Issue;

    private readonly List<Layout> _layouts = [];

    public Newspaper(int issue, string? author)
    {
        Issue = issue;

        _builder.DocumentInformation.Producer = "Totletheyn";
        _builder.DocumentInformation.Title = "Issue #" + issue;
        _builder.DocumentInformation.CreationDate = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
        _builder.DocumentInformation.Author = author;

        Layout.Builder = _builder;

        Layout.PathResolver.Base = "Assets/";
        Layout.AddFont("Default", "fonts/NoticiaText-Regular.ttf");
        Layout.AddFont("Jaini", "fonts/Jaini-Regular.ttf");
        Layout.AddFont("NoticiaText", "fonts/NoticiaText-Regular.ttf");

        LayoutLoader.AddDataSource<WeatherDataSource>();
        LayoutLoader.AddDataSource<XkcdDataSource>();
        LayoutLoader.AddDataSource<NasaDataSource>();
        LayoutLoader.AddDataSource<JokeDataSource>();
        LayoutLoader.AddDataSource<ComicDataSource>();
        LayoutLoader.AddDataSource<TiobeDataSource>();
        LayoutLoader.AddDataSource<CryptoDataSource>();

        var coverLayout = LayoutLoader.Load("layouts/cover.xml");
        _layouts.Add(coverLayout);
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

    public byte[] Render()
    {
        foreach (var layout in _layouts)
        {
            layout.Apply();
        }

        AddBookmarks(_layouts.ToArray());

        return _builder.Build();
    }

    public void AddFeed(Feed feed)
    {
        Feeds.Add(feed);

        var layout = LayoutLoader.Load("layouts/content.xml", feed.Title);
        _layouts.Add(layout);
    }
}