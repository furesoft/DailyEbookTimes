using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk;

public interface INewspaperModule
{
    void Add(PdfDocumentBuilder builder);
}