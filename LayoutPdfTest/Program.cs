using System.Diagnostics;
using Marius.Yoga;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

class Program
{
    static void Main(string[] args)
    {
        var builder = new PdfDocumentBuilder();

        var page = builder.AddPage(PageSize.A4, false);
        var config = new YogaConfig();

        var root = new YogaNode(config)
        {
            Width = page.PageSize.Width,
            Height = page.PageSize.Height,
            Margin = 10,
            Padding = 10,
        };

        var header = new YogaNode(config)
        {
            Height = 100,
            MarginTop = 15,
            MarginLeft = 10,
            MarginRight = 10,
            FlexGrow = 0,
            Data = "header"
        };

        var headerText = new YogaNode(config);
        header.Add(headerText);

        var footer = new YogaNode(config)
        {
            Height = 35,
            MarginBottom = 5,
            MarginLeft = 10,
            MarginRight = 10,
            FlexDirection = YogaFlexDirection.Row,
            FlexGrow = 0,
            Data = "footer"
        };

        var contentArea = new YogaNode(config)
        {
            FlexDirection = YogaFlexDirection.Row,
            FlexGrow = 1,
            MarginLeft = 10,
            MarginRight = 10,
            Data = "content"
        };

        var leftColumn = new YogaNode(config)
        {
            FlexGrow = 1,
            MarginLeft = 10,
            MarginRight = 10,
            Data = "left"
        };

        var middleColumn = new YogaNode(config)
        {
            FlexGrow = 2,
            MarginLeft = 10,
            MarginRight = 10,
            Data = "middle"
        };

        var rightColumn = new YogaNode(config)
        {
            FlexGrow = 1,
            MarginLeft = 10,
            MarginRight = 10,
            Data = "right"
        };

        contentArea.Add(leftColumn);
        contentArea.Add(middleColumn);
        contentArea.Add(rightColumn);

        root.StyleDirection = YogaDirection.LeftToRight;

        root.Add(footer);
        root.Add(contentArea);
        root.Add(header);

        root.CalculateLayout();

        font = builder.AddStandard14Font(Standard14Font.Helvetica);

        DrawBox(root, page);

        var documentBytes = builder.Build();

        File.WriteAllBytes("newPdf.pdf", documentBytes);
        Process.Start(new ProcessStartInfo("newPdf.pdf") { UseShellExecute = true });
    }

    private static readonly (byte r,byte g,byte b)[] Colors =
    {
        (255, 0, 0),    // Rot
        (0, 255, 0),    // Grün
        (0, 0, 255),    // Blau
        (255, 255, 0),  // Gelb
        (255, 0, 255),  // Magenta
        (0, 255, 255)   // Cyan
    };

    private static PdfDocumentBuilder.AddedFont font;
    private static void DrawBox(YogaNode root, PdfPageBuilder page, int colorIndex = 0)
    {
        foreach (var child in root.Children)
        {
            var color = Colors[colorIndex % Colors.Length];
            page.SetStrokeColor(color.r, color.g, color.b);
            var boxPos = new PdfPoint(child.LayoutX, child.LayoutY);
            page.DrawRectangle(boxPos, child.LayoutWidth, child.LayoutHeight, 1);

            if (child.Data is string text)
            {
                page.AddText(text, 20, new PdfPoint(child.LayoutX + 5, child.LayoutY + 5), font);
            }

            DrawBox(child, page, colorIndex + 1);
        }

        PdfPoint ConvertToTopLeftOrigin(PdfPoint point)
        {
            return new PdfPoint(point.X, page.PageSize.Height - point.Y);
        }
    }
}