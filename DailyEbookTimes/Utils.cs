using HtmlAgilityPack;

namespace Moss.NET.Sdk;

public class Utils
{
    public static string StripHtml(string value)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(value);

        if (htmlDoc == null)
            return value;

        return htmlDoc.DocumentNode.InnerText;
    }
}