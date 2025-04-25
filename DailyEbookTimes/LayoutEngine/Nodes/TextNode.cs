using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public enum TextWrapping
{
    None,
    Wrap

}
public class TextNode(YogaConfig config, Layout parentLayout) : YogaNode(config, parentLayout)
{
    public double FontSize { get; set; } = 10;
    public string? Text { get; set; }
    public string FontFamily { get; set; } = "Default";

    public Color? Color { get; set; } = Colors.Black;
    public TextWrapping TextWrapping { get; set; } = TextWrapping.None;

    public TextDecoration TextDecoration { get; set; }

    public int TruncateSize { get; set; }

    public bool AutoSize
    {
        get => Width is { Value: 1 } && Height is { Value: 1 };
        set
        {
            if (value)
            {
                Width = new YogaValue() { Unit = YogaUnit.Auto, Value = 1 };
                Height = new YogaValue() { Unit = YogaUnit.Auto, Value = 1 };
            }
            else
            {
                Width = YogaValue.Unset;
                Height = YogaValue.Unset;
            }
        }
    }

    public override void ReCalculate(PdfPageBuilder page)
    {
        if (string.IsNullOrEmpty(Text))
        {
            return;
        }

        var text = GetActualString();

        var measuredText = page.MeasureText(text, FontSize, PdfPoint.Origin, ParentLayout.GetFont(FontFamily));
        var leftMost = measuredText.Min(g => g.GlyphRectangle.Left);
        var rightMost = measuredText.Max(g => g.GlyphRectangle.Right);
        var textWidth = rightMost - leftMost;

        var textHeight = measuredText.Max(glyph => glyph.GlyphRectangle.Top) - measuredText.Min(glyph => glyph.GlyphRectangle.Bottom);

        if (Width is { Unit: YogaUnit.Auto, Value: 1 })
            Width = textWidth;

        if (Height is { Unit: YogaUnit.Auto, Value: 1 })
            Height = textHeight;
    }

    private string GetActualString()
    {
        if (TruncateSize > 0 && Text.Length > TruncateSize)
        {
            Text = Text[..TruncateSize] + " ...";
        }

        return Text;
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (Display == YogaDisplay.None || string.IsNullOrEmpty(Text))
            return;

        base.Draw(page, absoluteX, absoluteY);

        var text = GetActualString();
        var font = ParentLayout.GetFont(FontFamily);
        var maxWidth = LayoutWidth > 0 ? LayoutWidth : double.MaxValue;

        List<string> lines = new();

        if (TextWrapping == TextWrapping.Wrap)
        {
            var hardLines = text.Split('\n');
            foreach (var hardLine in hardLines)
            {
                var start = 0;
                while (start < hardLine.Length)
                {
                    var length = 1;
                    var line = "";
                    while (start + length <= hardLine.Length)
                    {
                        var testLine = hardLine.Substring(start, length);
                        var measured = page.MeasureText(testLine, FontSize, PdfPoint.Origin, font);
                        var width = measured.Max(g => g.GlyphRectangle.Right) -
                                    measured.Min(g => g.GlyphRectangle.Left);
                        if (width > maxWidth)
                            break;
                        line = testLine;
                        length++;
                    }

                    if (string.IsNullOrEmpty(line))
                        line = hardLine[start].ToString();
                    lines.Add(line);
                    start += line.Length;
                }
            }
        }
        else
        {
            lines.AddRange(text.Split('\n'));
        }

        var measuredLine = page.MeasureText(lines[0], FontSize, PdfPoint.Origin, font);
        var lineHeight = measuredLine.Max(g => g.GlyphRectangle.Top) - measuredLine.Min(g => g.GlyphRectangle.Bottom);
        var y = page.PageSize.Height - absoluteY - measuredLine.Max(g => g.GlyphRectangle.Top);

        if (Color != null)
        {
            page.SetStrokeColor(Color.r, Color.g, Color.b);
            page.SetTextAndFillColor(Color.r, Color.g, Color.b);
        }

        for (var i = 0; i < lines.Count; i++)
        {
            page.AddText(lines[i], FontSize, new PdfPoint(absoluteX, y - i * lineHeight), font);
        }

        page.ResetColor();
    }

    internal override void SetAttribute(string name, string value)
    {
        switch (name)
        {
            case "text":
                Text = value;
                break;
            case "fontsize":
                FontSize = int.Parse(value);
                break;
            case "fontfamily":
                FontFamily = value;
                break;
            case "color":
                Color = Colors.Parse(value);
                break;
            case "autosize":
                AutoSize = value == "true";
                break;
            case "textdecoration":
                TextDecoration = Enum.Parse<TextDecoration>(value, true);
                break;
            case "textwrapping":
                TextWrapping = Enum.Parse<TextWrapping>(value, true);
                break;
        }
    }
}