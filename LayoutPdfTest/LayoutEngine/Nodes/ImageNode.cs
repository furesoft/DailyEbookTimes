using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ImageNode : YogaNode
{
    public string ImagePath { get; set; }

    public ImageNode(YogaConfig config) : base(config)
    {
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        if (!string.IsNullOrEmpty(ImagePath))
        {
            var imageBytes = File.ReadAllBytes(ImagePath);

            var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
            var rect = new PdfRectangle(boxPos, new PdfPoint(boxPos.X + LayoutWidth, boxPos.Y + LayoutHeight));
            var img = page.AddPng(new MemoryStream(imageBytes), rect);

            page.AddImage(img, rect);
        }

        base.Draw(page, absoluteX, absoluteY);
    }
}