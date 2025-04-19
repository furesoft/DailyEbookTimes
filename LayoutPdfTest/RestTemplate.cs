//do not copy to moss sdk/totletheyn

using System.Net;

public class RestTemplate
{
    public byte[] GetBytes(string url)
    {
        var template = new WebClient();
        return template.DownloadData(url);
    }
}