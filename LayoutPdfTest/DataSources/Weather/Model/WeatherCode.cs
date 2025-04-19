namespace Moss.NET.Sdk.DataSources.Weather.Model;

public class WeatherCode
{
    private static readonly Dictionary<int, string> WeatherCodeToImageMap = new()
    {
        {0, "file://images/icons/forecast/sunny.png"},
        {1, "file://images/icons/forecast/mostly_sunny.png"},
        {2, "file://images/icons/forecast/partly_cloudy.png"},
        {3, "file://images/icons/forecast/cloudy.png"},
        {45, "file://images/icons/forecast/fog.png"},
        {48, "file://images/icons/forecast/fog.png"},
        {51, "file://images/icons/forecast/drizzle.png"},
        {53, "file://images/icons/forecast/drizzle.png"},
        {55, "file://images/icons/forecast/drizzle.png"},
        {56, "file://images/icons/forecast/freezing_drizzle.png"},
        {57, "file://images/icons/forecast/freezing_drizzle.png"},
        {61, "file://images/icons/forecast/rain.png"},
        {63, "file://images/icons/forecast/rain.png"},
        {65, "file://images/icons/forecast/rain.png"},
        {66, "file://images/icons/forecast/freezing_rain.png"},
        {67, "file://images/icons/forecast/freezing_rain.png"},
        {71, "file://images/icons/forecast/snow.png"},
        {73, "file://images/icons/forecast/snow.png"},
        {75, "file://images/icons/forecast/snow.png"},
        {77, "file://images/icons/forecast/snow.png"},
        {80, "file://images/icons/forecast/showers.png"},
        {81, "file://images/icons/forecast/showers.png"},
        {82, "file://images/icons/forecast/showers.png"},
        {85, "file://images/icons/forecast/snow_showers.png"},
        {86, "file://images/icons/forecast/snow_showers.png"},
        {95, "file://images/icons/forecast/thunderstorm.png"},
        {96, "file://images/icons/forecast/thunderstorm.png"},
        {99, "file://images/icons/forecast/thunderstorm.png"}
    };

    public static string GetImagePath(int weatherCode)
    {
        return WeatherCodeToImageMap.GetValueOrDefault(weatherCode, "file://images/icons/forecast/sunny.png");
    }
}