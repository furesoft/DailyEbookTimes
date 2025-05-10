using System.Xml.Linq;
using Moss.NET.Sdk.DataSources.Sodoku.Model;
using Moss.NET.Sdk.LayoutEngine;
using Moss.NET.Sdk.LayoutEngine.Nodes;
using PolyType.Examples.JsonSerializer;
using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.DataSources.Sodoku;

public class SodokuDataSource : IDataSource
{
    public string Name => "sodoku";

    private Grid? _boards;
    private float _scale = 1.0f;
    public void ApplyData(YogaNode node, PdfPageBuilder page, XElement element)
    {
        if (element.Attribute("scale") != null)
        {
            _scale = float.Parse(element.Attribute("scale")?.Value ?? "1.0");
        }

        var thickBorderWidth = 2;
        if (node is not ContainerNode container)
        {
            throw new ArgumentException("node is not a ContainerNode");
        }

        if (_boards == null)
        {
            var url = "https://sudoku-api.vercel.app/api/dosuku?query={newboard(limit:1){grids{value,solution,difficulty}}}";
            var template = new RestTemplate();
            var data = template.GetString(url);
            var response = JsonSerializerTS.Deserialize<SudokuResponse>(data);
            _boards = response!.NewBoard.Grids[0];
        }

        container.Title = $"Sudoku {_boards!.Difficulty}";
        container.Copyright = "sudoku-api.vercel.app";

        int[][] selectedGrid;
        var sodokuAttributeValue = element.Attribute("sodoku")?.Value;
        if (sodokuAttributeValue == "solution")
        {
            selectedGrid = _boards!.Solution;
            container.Title = "Sudoku - Solution";
        }
        else
        {
            selectedGrid = _boards!.Value;
        }

        container.Content.AlignSelf = YogaAlign.Center;
        container.Content.Margin = 5;
        container.Content.Background = Colors.Black;

        for (var rowIndex = 0; rowIndex < selectedGrid.Length; rowIndex++)
        {
            var box = selectedGrid[rowIndex];
            var rowNode = container.Content.ParentLayout.CreateNode($"row-{rowIndex}");
            rowNode.FlexDirection = YogaFlexDirection.Row;

            if(rowIndex != 0 && rowIndex % 3 == 0)
            {
                rowNode.MarginTop = thickBorderWidth;
            }

            for (int cellIndex = 0; cellIndex < box.Length; cellIndex++)
            {
                var cell = container.ParentLayout.CreateNode($"cell-{cellIndex}");
                cell.Width = 25 * _scale;
                cell.Height = 25 * _scale;
                cell.BorderColor = Colors.Black;
                cell.BorderWidth = 1;
                cell.BorderStyle = BorderStyle.Solid;
                cell.AlignItems = YogaAlign.Center;
                cell.JustifyContent = YogaJustify.Center;
                cell.Background = Colors.White;

                if(cellIndex != 0 && cellIndex % 3 == 0)
                {
                    cell.MarginLeft = thickBorderWidth;
                }

                if (box[cellIndex] != 0)
                {
                    var textNode = container.ParentLayout.CreateTextNode(box[cellIndex].ToString());
                    textNode.FontSize = 13 * _scale;
                    textNode.AlignSelf = YogaAlign.Center;
                    textNode.AutoSize = true;

                    cell.Add(textNode);
                }

                rowNode.Add(cell);
            }

            container.Content.Add(rowNode);
        }
    }
}