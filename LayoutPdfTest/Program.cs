using System.Diagnostics;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk;

class Program
{
    static void Main(string[] args)
    {
        var builder = new PdfDocumentBuilder();

        var page = builder.AddPage(PageSize.A4, false);
        var layout = Layout.Create(page, builder);
        layout.GetRoot().Background = Colors.Creme;

        layout.AddFont("Jaini", "fonts/Jaini-Regular.ttf");
        layout.AddFont("NoticiaText", "fonts/NoticiaText-Regular.ttf");

        AddHeader(layout);
        AddContentArea(layout);
        AddFooter(layout);

        var article5 = layout.FindNode<TextNode>("content middle truncated summary");
        article5.TruncateSize = 10;

        layout.EnableDebugLines();
        layout.Apply();

        var documentBytes = builder.Build();

        File.WriteAllBytes("newPdf.pdf", documentBytes);
        Process.Start(new ProcessStartInfo("newPdf.pdf") { UseShellExecute = true });
    }

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
        headline.Width = 300;
        headline.Height = 55;
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
        dateText.Width = 100;
        dateText.Height = 12;

        var rssImg = layout.CreateImageNode("images/rss.png");
        rssImg.Width = 10;
        rssImg.Height = 10;
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

    private static YogaNode AddFooter(Layout layout)
    {
        var footer = layout.CreateNode("footer");
        footer.Height = 10;
        footer.MarginBottom = 5;
        footer.MarginTop = 5;
        footer.MarginLeft = 10;
        footer.MarginRight = 10;
        footer.FlexDirection = YogaFlexDirection.Row;
        footer.Display = YogaDisplay.Flex;
        footer.JustifyContent = YogaJustify.Center;

        var footerText = layout.CreateTextNode("Generated with Totletheyn on Moss");
        footerText.FontSize = (int)FontSize.Footer;
        footerText.FlexGrow = 0;
        footerText.FontFamily = "NoticiaText";
        footerText.AlignSelf = YogaAlign.Center;
        footerText.Width = 10;
        footerText.Height = 10;
        footerText.TextDecoration = TextDecoration.Strikethrough;

        footer.Add(footerText);

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
        leftColumn.MarginLeft = 10;
        leftColumn.MarginRight = 10;
        leftColumn.BorderColor = Colors.Gray;
        leftColumn.Background  = Colors.White;
        leftColumn.BoxShadow = new BoxShadow(Colors.Gray, 2);

        var firstLeftArticle = CreateArticle(layout, "Test 1", "My Super duper test content. So great stuff here. My Super duper test content. So great stuff here");
        leftColumn.Add(firstLeftArticle);

        var secondLeftArticle = CreateArticle(layout, "Test 2", "My Super duper test content. So great stuff here");
        leftColumn.Add(secondLeftArticle);

        var thirdLeftArticle = CreateArticle(layout, "Test 3", "My Super duper test content. So great stuff here. My Super duper test content. So great stuff here");
        leftColumn.Add(thirdLeftArticle);

        var middleColumn = layout.CreateNode("middle");
        middleColumn.FlexGrow = 2;
        middleColumn.MarginLeft = 10;
        middleColumn.MarginRight = 10;
        middleColumn.BorderColor = Colors.Gray;
        middleColumn.Background  = Colors.White;
        middleColumn.BoxShadow = new BoxShadow(Colors.Gray, 2);

        firstLeftArticle = CreateArticle(layout, "Test 4", "My Super duper test content. So great stuff here.My Super duper test content. So great stuff here.");
        middleColumn.Add(firstLeftArticle);

        secondLeftArticle = CreateArticle(layout, "Test 5", "My Super duper test content. So great stuff here. My Super duper test content. So great stuff here.", "truncated");
        middleColumn.Add(secondLeftArticle);

        thirdLeftArticle = CreateArticle(layout, "Test 6", "My Super duper test content. So great stuff here. My Super duper test content. So great stuff here");
        middleColumn.Add(thirdLeftArticle);

        var rightColumn = layout.CreateNode("right");
        rightColumn.FlexGrow = 1;
        rightColumn.MarginLeft = 10;
        rightColumn.MarginRight = 10;
        rightColumn.BorderColor = Colors.Gray;
        rightColumn.Background  = Colors.White;
        rightColumn.BoxShadow = new BoxShadow(Colors.Gray, 2);

        firstLeftArticle = CreateArticle(layout, "Test 7", "My Super duper test content. So great stuff here");
        rightColumn.Add(firstLeftArticle);

        secondLeftArticle = CreateArticle(layout, "Test 8", "My Super duper test content. So great stuff here");
        rightColumn.Add(secondLeftArticle);

        thirdLeftArticle = CreateArticle(layout, "Test 9", "My Super duper test content. So great stuff here. My Super duper test content. So great stuff here");
        rightColumn.Add(thirdLeftArticle);

        contentArea.Add(leftColumn);
        contentArea.Add(middleColumn);
        contentArea.Add(rightColumn);

        layout.Add(contentArea);

        return contentArea;
    }

    private static YogaNode CreateArticle(Layout layout, string title, string summary, string? name = "article")
    {
        var article = layout.CreateNode(name);

        article.FlexGrow = 1;
        article.Margin = 10;
        article.FlexDirection = YogaFlexDirection.Column;
        article.Height = 50;
        article.Margin = 5;
        article.Padding = 5;

        var articleTitle = layout.CreateTextNode(title, "title");
        articleTitle.FontSize = (int)FontSize.ArticleHeading;
        articleTitle.FontFamily = "NoticiaText";
        articleTitle.Height = 20;
        articleTitle.Color = Colors.Red;

        var articleSummary = layout.CreateTextNode(summary, "summary");
        articleSummary.FontSize = (int)FontSize.ArticleP;
        articleSummary.FontFamily = "NoticiaText";
        articleSummary.Height = 100;

        article.Add(articleTitle);
        article.Add(articleSummary);

        return article;
    }
}