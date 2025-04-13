using Marius.Yoga;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

public class Layout
{
    private YogaNode root;
    private YogaConfig config;

    private Layout(YogaConfig config, PdfPageBuilder page)
    {
        this.config = config;

        root = new YogaNode(config)
        {
            Width = page.PageSize.Width,
            Height = page.PageSize.Height,
            Margin = 10,
            Padding = 10,
            Name = "root",
            StyleDirection = YogaDirection.LeftToRight
        };
    }

    public YogaNode GetRoot() => root;

    public static Layout Create(PdfPageBuilder page)
    {
        return new Layout(new(), page);
    }

    public static Layout Create(PdfPageBuilder page, YogaConfig config)
    {
        return new Layout(config, page);
    }

    public YogaNode CreateNode(string? name = null)
    {
        return new YogaNode(config)
        {
            Name = name
        };
    }

    public TextNode CreateTextNode(string text, string? name = null)
    {
        return new TextNode(config)
        {
            Name = name,
            Text = text
        };
    }

    public void Add(YogaNode node)
    {
        root.Add(node);
    }

    public void Apply(PdfPageBuilder page)
    {
        ReCalculate(root, page);
        root.CalculateLayout();

        DrawNode(root, page);
    }

    private static void DrawNode(YogaNode root, PdfPageBuilder page, double offsetX = 0, double offsetY = 0)
    {
        foreach (var child in root.Children)
        {
            var absoluteX = offsetX + child.LayoutX;
            var absoluteY = offsetY + child.LayoutY;

            child.Draw(page, absoluteX, absoluteY);

            DrawNode(child, page, absoluteX, absoluteY);
        }
    }

    private static void ReCalculate(YogaNode root, PdfPageBuilder page)
    {
        foreach (var child in root.Children)
        {
            child.ReCalculate(page);

            ReCalculate(child, page);
        }
    }

    public HorizontalLineNode CreateHorizontalLine(string? name = null)
    {
        return new HorizontalLineNode(config){
            Name = name
        };
    }

    /// <summary>
    /// Finds a node in the layout tree by a query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    /// <example>content.left.article</example>
    public YogaNode? FindNode(string query)
    {
        var parts = query.Split('.');
        var currentNode = root;

        foreach (var part in parts)
        {
            currentNode = currentNode.Children.FirstOrDefault(child => child.Name == part);
            if (currentNode == null)
            {
                return null; // Node nicht gefunden
            }
        }

        return currentNode;
    }
}