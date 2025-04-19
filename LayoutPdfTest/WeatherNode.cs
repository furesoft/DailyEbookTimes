using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;

namespace Moss.NET.Sdk;

public class WeatherComponent : Component
{
    public override string Tag => "weather";

    public override string GetLayout()
    {
        return """
               <forecastStrip flexdirection="row" justifycontent="spaceevenly" alignitems="center">
                   <fragment src="fragments/forecast_item.xml" width="100" height="145" alignself="center" background="darkgray" boxshadow="gray 1" />
                   <fragment src="fragments/forecast_item.xml" width="100" height="145" alignself="center" background="lightblue" boxshadow="gray 1" />
                   <fragment src="fragments/forecast_item.xml" width="100" height="145" alignself="center" background="green" boxshadow="gray 1" />
               </forecastStrip>
               """;
    }

    internal override void AfterLoad(YogaNode node)
    {
        var icon = node.FindNodes("forecastItem");

        
    }
}