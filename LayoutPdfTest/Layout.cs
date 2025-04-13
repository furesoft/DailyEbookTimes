using Marius.Yoga;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

public class Layout
{
    private YogaNode root;
    private YogaConfig config;
    private readonly PdfPageBuilder _page;
    private readonly PdfDocumentBuilder _builder;
    static Dictionary<string, PdfDocumentBuilder.AddedFont> _fonts = new();

    private Layout(YogaConfig config, PdfPageBuilder page, PdfDocumentBuilder builder)
    {
        this.config = config;
        _page = page;
        _builder = builder;

        root = new YogaNode(config)
        {
            Width = page.PageSize.Width,
            Height = page.PageSize.Height,
            Margin = 10,
            Padding = 10,
            Name = "root",
            StyleDirection = YogaDirection.LeftToRight
        };

        //todo: add standard font from moss
    }

    public YogaNode GetRoot() => root;

    public static Layout Create(PdfPageBuilder page, PdfDocumentBuilder builder)
    {
        return new Layout(new(), page, builder);
    }

    public static Layout Create(PdfPageBuilder page, PdfDocumentBuilder builder, YogaConfig config)
    {
        return new Layout(config, page, builder);
    }

    public YogaNode CreateNode(string? name = null)
    {
        return new YogaNode(config)
        {
            Name = name,
            ParentLayout = this
        };
    }

    public TextNode CreateTextNode(string text, string? name = null)
    {
        return new TextNode(config)
        {
            Name = name,
            Text = text,
            ParentLayout = this
        };
    }

    public void Add(YogaNode node)
    {
        root.Add(node);
    }

    public void Apply()
    {
        ReCalculate(root, _page);
        root.CalculateLayout();

        DrawNode(root, _page);
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

    public void AddFont(string name, string path)
    {
       var font = _builder.AddTrueTypeFont(File.ReadAllBytes(path));
       _fonts[name] = font;
    }

    public PdfDocumentBuilder.AddedFont GetFont(string fontFamily)
    {
        if (_fonts.TryGetValue(fontFamily, out var font))
        {
            return font;
        }

        //todo: use standard font instead of exception
        throw new ArgumentException($"Font '{fontFamily}' not found.");
    }
}