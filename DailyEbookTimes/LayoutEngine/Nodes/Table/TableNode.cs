using UglyToad.PdfPig.Writer;

namespace Moss.NET.Sdk.LayoutEngine.Nodes.Table;

public class TableNode : YogaNode
{
    private TableRowNode headerRow;

    public TableNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        FlexDirection = YogaFlexDirection.Column;
        headerRow = AddRow("header");
    }

    public TableRowNode AddRow(string? name = null)
    {
        TableRowNode row = new(Config, ParentLayout)
        {
            MarginBottom = 5,
            Name = name
        };

        Add(row);
        return row;
    }

    /// <summary>
    /// Display a header row with the column names. Call this after adding all columns.
    /// </summary>
    public TableRowNode GetHeaderRow()
    {
        return headerRow;
    }

    public YogaNode AddColumn(string? header = null, YogaAlign? align = null)
    {
        var cell = headerRow.AddCell();

        if (header != null)
        {
            var headerTextNode = ParentLayout.CreateTextNode(header);
            headerTextNode.AutoSize = true;
            headerTextNode.AlignSelf = align ?? YogaAlign.Center;
            cell.Add(headerTextNode);
        }

        return cell;
    }

    public override void ReCalculate(PdfPageBuilder page)
    {
        base.ReCalculate(page);

        if (headerRow.Display == YogaDisplay.None)
        {
            foreach (var cell in headerRow)
            {
                cell.GetChild(0).Display = YogaDisplay.None;
            }
        }
    }
}