using System.Net;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public class ImageNode : YogaNode
{
    public required Uri ImagePath { get; set; }

    public ImageNode(YogaConfig config) : base(config)
    {
    }

    public override void Draw(PdfPageBuilder page, double absoluteX, double absoluteY)
    {
        base.Draw(page, absoluteX, absoluteY);

        byte[] imageBytes = null;
        if (ImagePath is { IsAbsoluteUri: true, Scheme: "http" or "https" })
        {
            //Todo: convert to moss rest template
            var template = new WebClient();
            imageBytes = template.DownloadData(ImagePath);
        }

        if (ImagePath.IsFile)
        {
            imageBytes = File.ReadAllBytes(ImagePath.ToString().Replace("file://", ""));
        }

        var boxPos = new PdfPoint(absoluteX, page.PageSize.Height - absoluteY - LayoutHeight);
        var rect = new PdfRectangle(boxPos, new PdfPoint(boxPos.X + LayoutWidth, boxPos.Y + LayoutHeight));
        var img = page.AddPng(new MemoryStream(imageBytes), rect);

        page.AddImage(img, rect);
    }
}