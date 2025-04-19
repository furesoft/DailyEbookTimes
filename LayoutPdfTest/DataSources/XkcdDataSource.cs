using System.Xml;
using System.Xml.Linq;
using CodeHollow.FeedReader;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.DataSources;

public class XkcdDataSource : IDataSource
{
    const string URL = "https://xkcd.com/rss.xml";
    public string Name => "xkcd";

    private Feed feed;
    public XkcdDataSource()
    {
        feed = FeedReader.Read(URL);
    }

    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (node is ImageNode img)
        {
            var imgHtml = feed.Items[0].Description;
            var doc = XDocument.Parse(imgHtml);
            img.Src = new Uri(doc.Root!.Attribute("src")!.Value);
        }
    }
}