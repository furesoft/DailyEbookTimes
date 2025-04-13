using System.Diagnostics;
using Marius.Yoga;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

class Program
{
    static void Main(string[] args)
    {
        var builder = new PdfDocumentBuilder();
        var headlineFont = builder.AddTrueTypeFont(File.ReadAllBytes("fonts/Jaini-Regular.ttf")); //builder.AddStandard14Font(Standard14Font.Helvetica);
        var textFont = builder.AddTrueTypeFont(File.ReadAllBytes("fonts/NoticiaText-Regular.ttf"));

        var page = builder.AddPage(PageSize.A4, false);
        var layout = Layout.Create(page);

        var header = layout.CreateNode("header");

        header.Height = 120;
        header.MarginTop = 15;
        header.MarginLeft = 10;
        header.MarginRight = 10;
        header.FlexGrow = 0;
        header.AlignItems = YogaAlign.Center;

        var headerText = layout.CreateTextNode("Daily E-Book Times");
        headerText.FontSize = 75;
        headerText.Font = headlineFont;
        headerText.AlignSelf = YogaAlign.Center;
        headerText.Width = 300;
        headerText.Height = 55;
        headerText.PositionType = YogaPositionType.Relative;

        var topLine = layout.CreateHorizontalLine();
        topLine.LineThickness = 2;
        topLine.Margin = 10;

        var bottomLine = layout.CreateHorizontalLine();
        bottomLine.LineThickness = 2;
        bottomLine.Margin = 10;

        var issueText = layout.CreateTextNode("Issue #1");
        issueText.Font = textFont;
        issueText.FontSize = 15;
        issueText.MarginLeft = 10;
        issueText.MarginBottom = 10;
        issueText.AlignSelf = YogaAlign.FlexStart;

        header.Add(headerText);
        header.Add(topLine);
        header.Add(issueText);
        header.Add(bottomLine);

        var footer = layout.CreateNode("footer");
        footer.Height = 10;
        footer.MarginBottom = 5;
        footer.MarginLeft = 10;
        footer.MarginRight = 10;
        footer.FlexDirection = YogaFlexDirection.Row;
        footer.FlexGrow = 0;

        var footerLine = layout.CreateHorizontalLine();
        footerLine.LineThickness = 1;
        footerLine.Margin = 10;

        footer.Add(footerLine);

        var contentArea = layout.CreateNode("content");
        contentArea.FlexDirection = YogaFlexDirection.Row;
        contentArea.FlexGrow = 1;
        contentArea.Margin = 10;

        var leftColumn = layout.CreateNode("left");
        leftColumn.FlexGrow = 1;
        leftColumn.MarginLeft = 10;
        leftColumn.MarginRight = 10;
        leftColumn.BorderColor = new Color(0, 0, 255);

        var middleColumn = layout.CreateNode("middle");
        middleColumn.FlexGrow = 2;
        middleColumn.MarginLeft = 10;
        middleColumn.MarginRight = 10;
        middleColumn.BorderColor = new Color(0, 0, 255);

        var rightColumn = layout.CreateNode("right");
        rightColumn.FlexGrow = 1;
        rightColumn.MarginLeft = 10;
        rightColumn.MarginRight = 10;
        rightColumn.BorderColor = new Color(0, 0, 255);

        contentArea.Add(leftColumn);
        contentArea.Add(middleColumn);
        contentArea.Add(rightColumn);

        layout.Add(header);
        layout.Add(contentArea);
        layout.Add(footer);

        layout.Apply(page);

        var documentBytes = builder.Build();

        File.WriteAllBytes("newPdf.pdf", documentBytes);
        Process.Start(new ProcessStartInfo("newPdf.pdf") { UseShellExecute = true });
    }
}