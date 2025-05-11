using CodeHollow.FeedReader;
using HtmlAgilityPack;
using Moss.NET.Sdk.DataSources;
using Moss.NET.Sdk.DataSources.Crypto;
using Moss.NET.Sdk.DataSources.Sodoku;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk;

public class Newspaper
{
    private List<Feed> Feeds { get; } = [];

    private readonly PdfDocumentBuilder _builder = new();
    private readonly int _issue;

    private readonly List<Layout> _layouts = [];
	
	public string Title => _builder.DocumentInformation.Title!;

    public Newspaper(int issue, string? author)
    {
        _issue = issue;

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
        LayoutLoader.AddDataSource<SodokuDataSource>();

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
        AddNewsToCover();

        foreach (var layout in _layouts)
        {
            layout.Apply();
        }

        AddBookmarks(_layouts.ToArray());

        return _builder.Build();
    }

    private void AddNewsToCover()
    {
        var coverLayout = _layouts[0];
        var articles = coverLayout.FindDescendantNodes("article").ToArray();

        var articleIndex = 0;
        var totalArticles = articles.Length;
        var feedQueue = new Queue<Feed>(Feeds);

        while (articleIndex < totalArticles && feedQueue.Count > 0)
        {
            var feed = feedQueue.Dequeue();

            foreach (var item in feed.Items)
            {
                if (articleIndex >= totalArticles)
                    break;

                var titleNode = articles[articleIndex].FindNode<TextNode>("title");
                if(titleNode is not null)
                    titleNode.Text = item.Title;

                var summaryNode = articles[articleIndex].FindNode("summary")!;
                if (summaryNode is not null)
                {
                    var doc = new HtmlDocument();
                    doc.LoadHtml(item.Content);

                    summaryNode.FindNode<ImageNode>("img")!.Src = doc.DocumentNode.SelectSingleNode("//img")?.GetAttributeValue("src", null);
                    summaryNode.FindNode<TextNode>("text")!.Text = Utils.StripHtml(item.Description);
                }
                articleIndex++;
            }

            if (feed.Items.Any())
                feedQueue.Enqueue(feed);
        }
    }

    public void AddFeed(Feed feed)
    {
        Feeds.Add(feed);

        var layout = LayoutLoader.Load("layouts/content.xml", feed.Title);

        _layouts.Add(layout);
    }

    public ContentLayout AddContent(string name)
    {
        var contentLayout = new ContentLayout(name);
        _layouts.Add(contentLayout.Layout);

        return contentLayout;
    }
}