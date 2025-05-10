using PolyType;

namespace Moss.NET.Sdk.DataSources.Sodoku.Model;

[GenerateShape]
public partial class NewBoard
{
    [PropertyShape(Name = "grids")]
    public List<Grid> Grids { get; set; }
}