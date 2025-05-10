using PolyType;

namespace Moss.NET.Sdk.DataSources.Sodoku.Model;

[GenerateShape]
public partial class SudokuResponse
{
    [PropertyShape(Name = "newboard")]
    public NewBoard NewBoard { get; set; }
}