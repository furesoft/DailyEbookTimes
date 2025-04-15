using System.Diagnostics;
using CodeHollow.FeedReader;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk;

class Program
{
    static void Main(string[] args)
    {
        string[] feeds = [
            "https://www.heise.de/rss/heise-Rubrik-IT.rdf",
            "https://rss.nytimes.com/services/xml/rss/nyt/World.xml"];

        foreach (string url in feeds)
        {
            var feed = FeedReader.Read(url);
            Feeds.Add(feed);
        }

        var builder = new PdfDocumentBuilder();

        var page = builder.AddPage(PageSize.A4, false);
        builder.Bookmarks = new([
            new DocumentBookmarkNode("Cover",0, new ExplicitDestination(1, ExplicitDestinationType.FitPage, ExplicitDestinationCoordinates.Empty), [])
        ]);

        var layout = Layout.Create(page, builder);
        layout.GetRoot().Background = Colors.Creme;

        layout.AddFont("Jaini", "fonts/Jaini-Regular.ttf");
        layout.AddFont("NoticiaText", "fonts/NoticiaText-Regular.ttf");

        AddHeader(layout);
        AddContentArea(layout);
        AddFooter(layout, 0);

        var article5 = layout.FindNode<TextNode>("content middle truncated summary");
        article5.TruncateSize = 10;

        layout.EnableDebugLines();
        layout.Apply();

        var documentBytes = builder.Build();

        File.WriteAllBytes("newPdf.pdf", documentBytes);
        Process.Start(new ProcessStartInfo("newPdf.pdf") { UseShellExecute = true });
    }

    public static List<Feed> Feeds { get; set; } = [];

    private static YogaNode AddHeader(Layout layout)
    {
        var header = layout.CreateNode("header");

        header.Height = 120;
        header.MarginTop = 20;
        header.MarginLeft = 10;
        header.MarginRight = 10;
        header.FlexGrow = 0;

        var headerInfo = layout.CreateNode();
        headerInfo.FlexDirection = YogaFlexDirection.Row;
        headerInfo.AlignItems = YogaAlign.Center;
        headerInfo.JustifyContent = YogaJustify.SpaceBetween;
        headerInfo.Margin = 10;

        var headline = layout.CreateTextNode("Daily E-Book Times");
        headline.FontSize = (int)FontSize.Headline;
        headline.FontFamily = "Jaini";
        headline.AlignSelf = YogaAlign.Center;
        headline.AutoSize = true;
        headline.PositionType = YogaPositionType.Relative;
        headline.MarginBottom = 5;
        headline.TextDecoration = TextDecoration.Underline;

        var topLine = layout.CreateHorizontalLine();
        topLine.MarginLeft = 10;
        topLine.MarginRight = 10;

        var bottomLine = layout.CreateHorizontalLine();
        bottomLine.MarginLeft = 10;
        bottomLine.MarginRight = 10;

        var issueText = layout.CreateTextNode("Issue #{0}");
        issueText.FontFamily = "Jaini";
        issueText.FontSize = (int)FontSize.HeaderInfo;
        issueText.AlignSelf = YogaAlign.FlexStart;

        var dateText = layout.CreateTextNode(DateTime.Now.ToString("dddd, MMMM dd, yyyy"));
        dateText.FontFamily = "Jaini";
        dateText.FontSize = (int)FontSize.HeaderInfo;
        dateText.AlignSelf = YogaAlign.Center;
        dateText.AutoSize = true;

        var rssImg = layout.CreateImageNode("file://images/rss.png");
        rssImg.Width = 9;
        rssImg.Height = 9;
        rssImg.AlignSelf = YogaAlign.Center;

        header.Add(headline);
        header.Add(topLine);

        headerInfo.Add(issueText);
        headerInfo.Add(dateText);
        headerInfo.Add(rssImg);

        header.Add(headerInfo);
        header.Add(bottomLine);

        layout.Add(header);

        return header;
    }

    private static YogaNode AddFooter(Layout layout, int? pageNum = null)
    {
        var footer = layout.CreateNode("footer");
        footer.Height = 10;
        footer.MarginBottom = 5;
        footer.MarginTop = 5;
        footer.MarginLeft = 10;
        footer.MarginRight = 10;
        footer.FlexDirection = YogaFlexDirection.Row;
        footer.Display = YogaDisplay.Flex;
        footer.JustifyContent = YogaJustify.SpaceBetween;

        var footerText = layout.CreateTextNode("Generated with Totletheyn on Moss");
        footerText.FontSize = (int)FontSize.Footer;
        footerText.FlexGrow = 0;
        footerText.FontFamily = "NoticiaText";
        footerText.AlignSelf = YogaAlign.Center;
        footerText.AutoSize = true;
        footerText.TextDecoration = TextDecoration.Strikethrough;

        footer.Add(footerText);

        if (pageNum.HasValue)
        {
            var pageText = layout.CreateTextNode("Page " + pageNum, "pageNum");
            pageText.FontSize = (int)FontSize.Footer;
            pageText.FlexGrow = 0;
            pageText.FontFamily = "NoticiaText";
            pageText.AlignSelf = YogaAlign.FlexEnd;
            pageText.AlignItems = YogaAlign.Center;
            pageText.AutoSize = true;

            footer.Add(pageText);
        }

        layout.Add(footer);

        return footer;
    }

    private static YogaNode AddContentArea(Layout layout)
    {
        var contentArea = layout.CreateNode("content");
        contentArea.FlexDirection = YogaFlexDirection.Row;
        contentArea.FlexGrow = 1;
        contentArea.Margin = 10;
        contentArea.MarginBottom = 1;

        var leftColumn = layout.CreateNode("left");
        leftColumn.FlexGrow = 1;
        leftColumn.MarginRight = 10;
        leftColumn.BorderColor = Colors.Gray;
        leftColumn.Background  = Colors.White;
        leftColumn.BoxShadow = new BoxShadow(Colors.Gray, 2);

        var firstLeftArticle = CreateArticle(layout, Feeds[0].Items[0]);
        leftColumn.Add(firstLeftArticle);

        var secondLeftArticle = CreateArticle(layout, Feeds[0].Items[1]);
        leftColumn.Add(secondLeftArticle);

        var thirdLeftArticle = CreateArticle(layout, Feeds[0].Items[2]);
        leftColumn.Add(thirdLeftArticle);

        var middleColumn = layout.CreateNode("middle");
        middleColumn.FlexGrow = 2;
        middleColumn.MarginLeft = 5;
        middleColumn.MarginRight = 5;
        middleColumn.BorderColor = Colors.Gray;
        middleColumn.Background  = Colors.White;
        middleColumn.BoxShadow = new BoxShadow(Colors.Gray, 2);

        firstLeftArticle = CreateArticle(layout, Feeds[0].Items[3]);
        middleColumn.Add(firstLeftArticle);

        secondLeftArticle = CreateArticle(layout, Feeds[1].Items[0], "truncated");
        middleColumn.Add(secondLeftArticle);

        thirdLeftArticle = CreateArticle(layout, Feeds[1].Items[1]);
        middleColumn.Add(thirdLeftArticle);

        var rightColumn = layout.CreateNode("right");
        rightColumn.FlexGrow = 1;
        rightColumn.MarginLeft = 10;
        rightColumn.BorderColor = Colors.Gray;
        rightColumn.Background  = Colors.White;
        rightColumn.BoxShadow = new BoxShadow(Colors.Gray, 2);

        firstLeftArticle = CreateArticle(layout, Feeds[1].Items[2]);
        rightColumn.Add(firstLeftArticle);

        secondLeftArticle = CreateArticle(layout, Feeds[1].Items[3]);
        rightColumn.Add(secondLeftArticle);

        thirdLeftArticle = CreateArticle(layout, Feeds[1].Items[4]);
        rightColumn.Add(thirdLeftArticle);

        contentArea.Add(leftColumn);
        contentArea.Add(middleColumn);
        contentArea.Add(rightColumn);

        layout.Add(contentArea);

        return contentArea;
    }

    private static YogaNode CreateArticle(Layout layout, FeedItem item, string? name = "article")
    {
        var article = layout.CreateNode(name);

        article.FlexGrow = 1;
        article.FlexDirection = YogaFlexDirection.Column;
        article.Height = 50;
        article.Padding = 5;

        var articleTitle = layout.CreateTextNode(item.Title, "title");
        articleTitle.FontSize = (int)FontSize.ArticleHeading;
        articleTitle.FontFamily = "NoticiaText";
        articleTitle.Height = 20;
        articleTitle.Color = Colors.Red;

        var articleSummary = layout.CreateTextNode(item.Description.Replace("\n", "\\n"), "summary");
        articleSummary.FontSize = (int)FontSize.ArticleContent;
        articleSummary.FontFamily = "NoticiaText";
        articleSummary.Height = 100;

        article.Add(articleTitle);
        article.Add(articleSummary);

        return article;
    }
}