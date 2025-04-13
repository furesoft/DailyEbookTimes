using Marius.Yoga;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace LayoutPdfTest;

public class HorizontalLineNode : YogaNode
{
    public HorizontalLineNode(YogaConfig config) : base(config)
    {
        Width = YogaValue.Percent(100);
        PositionType = YogaPositionType.Relative;
        AlignSelf = YogaAlign.FlexStart;
    }

    public double LineThickness { get; set; } = 1.0;
    public Color LineColor { get; set; } = new Color(0, 0, 0);

    public override void ReCalculate(PdfPageBuilder page)
    {
        if (!MarginLeft.Value.HasValue && Margin.Value.HasValue)
        {
            MarginLeft = Margin;
        }
        if (!MarginRight.Value.HasValue && Margin.Value.HasValue)
        {
            MarginRight = Margin;
        }

        if (!MarginLeft.Value.HasValue)
        {
            MarginLeft = 0;
        }
        if (!MarginRight.Value.HasValue)
        {
            MarginRight = 0;
        }

        Height = LineThickness;
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        base.Draw(page, absoluteX, absoluteY);

        var startX = absoluteX;
        var endX = absoluteX + LayoutWidth - MarginRight.Value - MarginLeft.Value;
        var yPosition = page.PageSize.Height - absoluteY - (LineThickness / 2);

        page.SetStrokeColor(LineColor.r, LineColor.g, LineColor.b);
        page.DrawLine(new PdfPoint(startX, yPosition), new PdfPoint(endX.Value, yPosition), LineThickness);
    }
}