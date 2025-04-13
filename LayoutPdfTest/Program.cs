using System.Diagnostics;
using Marius.Yoga;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

public enum FontSize
{
    Headline = 75,
    HeaderInfo = 15,
    ArticleHeading = 18,
    ArticleP = 14,
    Footer = 10
}

class Program
{
    static void Main(string[] args)
    {
        var builder = new PdfDocumentBuilder();

        var page = builder.AddPage(PageSize.A4, false);
        var layout = Layout.Create(page, builder);

        layout.AddFont("Jaini", "fonts/Jaini-Regular.ttf");
        layout.AddFont("NoticiaText", "fonts/NoticiaText-Regular.ttf");

        var header = AddHeader(layout);
        var footer = AddFooter(layout);
        var contentArea = AddContentArea(layout);

        layout.Add(header);
        layout.Add(contentArea);
        layout.Add(footer);

       // layout.EnableDebugLines();
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
        header.Background = new Color(255, 255, 0);

        var headerInfo = layout.CreateNode();
        headerInfo.FlexDirection = YogaFlexDirection.Row;
        headerInfo.Width = YogaValue.Percent(95);
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

        var topLine = layout.CreateHorizontalLine();
        topLine.LineThickness = 1;
        topLine.MarginLeft = 10;
        topLine.MarginRight = 10;

        var bottomLine = layout.CreateHorizontalLine();
        bottomLine.LineThickness = 1;
        bottomLine.MarginLeft = 10;
        bottomLine.MarginRight = 10;

        var issueText = layout.CreateTextNode("Issue #{0}");
        issueText.FontFamily = "NoticiaText";
        issueText.FontSize = (int)FontSize.HeaderInfo;
        issueText.AlignSelf = YogaAlign.FlexStart;

        var dateText = layout.CreateTextNode(DateTime.Now.ToString("dddd, MMMM dd, yyyy"));
        dateText.FontFamily = "NoticiaText";
        dateText.FontSize = (int)FontSize.HeaderInfo;
        dateText.AlignSelf = YogaAlign.Center;
        dateText.Width = 100;
        dateText.Height = 12;

        var rssImg = layout.CreateImageNode("images/rss.png");
        rssImg.Width = 10;
        rssImg.Height = 10;
        rssImg.AlignSelf = YogaAlign.FlexEnd;

        header.Add(headline);
        header.Add(topLine);

        headerInfo.Add(issueText);
        headerInfo.Add(dateText);
        headerInfo.Add(rssImg);

        header.Add(headerInfo);
        header.Add(bottomLine);

        return header;
    }

    private static YogaNode AddFooter(Layout layout)
    {
        var footer = layout.CreateNode("footer");
        footer.Height = 10;
        footer.MarginBottom = 5;
        footer.MarginLeft = 10;
        footer.MarginRight = 10;
        footer.FlexDirection = YogaFlexDirection.Row;

        var footerLeftLine = layout.CreateHorizontalLine();
        footerLeftLine.LineThickness = 1;
        footerLeftLine.Margin = 10;
        footerLeftLine.LineColor = Colors.Gray;
        footerLeftLine.FlexGrow = 1;
        footerLeftLine.AlignSelf = YogaAlign.FlexStart;

        var footerText = layout.CreateTextNode("Generated with Totletheyn on Moss");
        footerText.FontSize = (int)FontSize.Footer;
        footerText.FlexGrow = 0;
        footerText.FontFamily = "NoticiaText";
        footerText.AlignSelf = YogaAlign.Center;
        footerText.Width = YogaValue.Auto;
        footerText.Height = 10;

        var footerRightLine = layout.CreateHorizontalLine();
        footerRightLine.LineThickness = 1;
        footerRightLine.Margin = 10;
        footerRightLine.LineColor = Colors.Gray;
        footerRightLine.FlexGrow = 1;
        footerRightLine.AlignSelf = YogaAlign.FlexEnd;

        footer.Add(footerLeftLine);
        footer.Add(footerText);
        footer.Add(footerRightLine);

        return footer;
    }

    private static YogaNode AddContentArea(Layout layout)
    {
        var contentArea = layout.CreateNode("content");
        contentArea.FlexDirection = YogaFlexDirection.Row;
        contentArea.FlexGrow = 1;
        contentArea.Margin = 10;

        var leftColumn = layout.CreateNode("left");
        leftColumn.FlexGrow = 1;
        leftColumn.MarginLeft = 10;
        leftColumn.MarginRight = 10;
        leftColumn.BorderColor = Colors.Gray;

        var firstLeftArticle = CreateArticle(layout, "Test", "My Super duper test content. So great stuff here");
        leftColumn.Add(firstLeftArticle);

        var middleColumn = layout.CreateNode("middle");
        middleColumn.FlexGrow = 2;
        middleColumn.MarginLeft = 10;
        middleColumn.MarginRight = 10;
        middleColumn.BorderColor = Colors.Gray;

        var rightColumn = layout.CreateNode("right");
        rightColumn.FlexGrow = 1;
        rightColumn.MarginLeft = 10;
        rightColumn.MarginRight = 10;
        rightColumn.BorderColor = Colors.Gray;

        contentArea.Add(leftColumn);
        contentArea.Add(middleColumn);
        contentArea.Add(rightColumn);
        return contentArea;
    }

    private static YogaNode CreateArticle(Layout layout, string title, string summary)
    {
        var article = layout.CreateNode("article");

        article.FlexGrow = 1;
        article.Margin = 10;
        article.FlexDirection = YogaFlexDirection.Column;
        article.Height = 50;
        article.Margin = 5;
        article.Padding = 5;

        var articleTitle = layout.CreateTextNode(title);
        articleTitle.FontSize = (int)FontSize.ArticleHeading;
        articleTitle.FontFamily = "NoticiaText";
        articleTitle.Height = 20;
        articleTitle.Color = Colors.Red;

        var articleSummary = layout.CreateTextNode(summary);
        articleSummary.FontSize = (int)FontSize.ArticleP;
        articleSummary.FontFamily = "NoticiaText";
        articleSummary.Height = 100;

        article.Add(articleTitle);
        article.Add(articleSummary);

        return article;
    }
}