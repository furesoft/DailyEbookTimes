using System.Text.Json.Serialization;
using PolyType;

namespace Moss.NET.Sdk.DataSources.Weather.Model;

[GenerateShape]
public class WeatherApiResponse
{
    [PropertyShape(Name = "latitude")]
    public double Latitude { get; set; }

    [PropertyShape(Name = "longitude")]
    public double Longitude { get; set; }

    [PropertyShape(Name = "generationtime_ms")]
    public double Generationtime_ms { get; set; }

    [PropertyShape(Name = "utc_offset_seconds")]
    public int Utc_offset_seconds { get; set; }

    [PropertyShape(Name = "timezone")]
    public string Timezone { get; set; }

    [PropertyShape(Name = "timezone_abbreviation")]
    public string Timezone_abbreviation { get; set; }

    [PropertyShape(Name = "elevation")]
    public double Elevation { get; set; }

    [PropertyShape(Name = "current_units")]
    public CurrentUnits Current_units { get; set; }

    [PropertyShape(Name = "current")]
    public Current Current { get; set; }

    [PropertyShape(Name = "hourly_units")]
    public HourlyUnits Hourly_units { get; set; }

    [PropertyShape(Name = "hourly")]
    public Hourly Hourly { get; set; }

    [PropertyShape(Name = "daily_units")]
    public DailyUnits Daily_units { get; set; }

    [PropertyShape(Name = "daily")]
    public Daily Daily { get; set; }
}

