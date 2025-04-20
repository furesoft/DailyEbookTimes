//do not copy to moss sdk/totletheyn

using System.Net;

public class RestTemplate
{
    public Dictionary<string, string> Headers = new();
    public byte[] GetBytes(string url)
    {
        var template = new WebClient();

        foreach (var header in Headers)
        {
            template.Headers.Add(header.Key, header.Value);
        }

        return template.DownloadData(url);
    }

    public string GetString(string url)
    {
        var template = new WebClient();
        foreach (var header in Headers)
        {
            template.Headers.Add(header.Key, header.Value);
        }

        return template.DownloadString(url);
    }
}