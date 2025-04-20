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
        var imgHtml = feed.Items[0].Description;
        var doc = XDocument.Parse(imgHtml);

        var img = node.ParentLayout.CreateImageNode(doc.Root!.Attribute("src")!.Value);
        img.Width = 300;
        img.Height = 150;

        node.Add(img);

        node.Margin = 10;

        var text = node.ParentLayout.CreateTextNode("© xkcd");
        text.FontFamily = "NoticiaText";
        text.FontSize = 6;
        text.AlignSelf = YogaAlign.FlexEnd;
        text.AutoSize = true;

        node.Add(text);

        node.FlexDirection = YogaFlexDirection.Column;
        node.Width = img.Width;
        node.Height = img.Height.Value!.Value + 10;
    }
}