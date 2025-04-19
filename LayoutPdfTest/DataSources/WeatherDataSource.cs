using Moss.NET.Sdk.LayoutEngine;

namespace Moss.NET.Sdk.DataSources;

public class WeatherDataSource : IDataSource
{
    public string Name => "weather";

    public void ApplyData(YogaNode node)
    {
        var items = node.FindNodes("forecastItem").ToArray();

        
    }
}