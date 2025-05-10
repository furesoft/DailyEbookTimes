using PolyType;

namespace Moss.NET.Sdk.DataSources.Sodoku.Model;

[GenerateShape]
public partial class Grid
{
    [PropertyShape(Name = "value")]
    public int[][] Value { get; set; }

    [PropertyShape(Name = "solution")]
    public int[][] Solution { get; set; }

    [PropertyShape(Name = "difficulty")]
    public string Difficulty { get; set; }
}