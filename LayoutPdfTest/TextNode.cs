using Marius.Yoga;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

public class TextNode(YogaConfig config) : YogaNode(config)
{
    public double FontSize { get; set; } = 10;
    public string Text { get; set; }
    public string FontFamily { get; set; } = "Default";

    public Color? Color { get; set; }

    public override void ReCalculate(PdfPageBuilder page)
    {
        var measuredText = page.MeasureText(Text, FontSize, PdfPoint.Origin, ParentLayout.GetFont(FontFamily));
        var leftMost = measuredText.Min(g => g.GlyphRectangle.Left);
        var rightMost = measuredText.Max(g => g.GlyphRectangle.Right);
        var textWidth = rightMost - leftMost;

        var textHeight = measuredText.Max(glyph => glyph.GlyphRectangle.Top)
                         - measuredText.Min(glyph => glyph.GlyphRectangle.Bottom);

        if (Width.Value < textWidth)
            Width = textWidth;

        if (Height.Value < textHeight)
            Height = textHeight;
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        base.Draw(page, absoluteX, absoluteY);

        var font = ParentLayout.GetFont(FontFamily);

        var measuredText = page.MeasureText(Text, FontSize, PdfPoint.Origin, font);
        var maxAscent = measuredText.Max(g => g.GlyphRectangle.Top);
        var yPosition = page.PageSize.Height - absoluteY - maxAscent;

        if (Color != null)
        {
            page.SetTextAndFillColor(Color.r, Color.g, Color.b);
        }

        page.AddText(Text, FontSize, new PdfPoint(absoluteX, yPosition), font);
        page.ResetColor();
    }
}