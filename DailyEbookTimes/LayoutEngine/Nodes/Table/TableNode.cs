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

    public YogaNode AddColumn(string? header = null, YogaAlign? align = null, bool isBold = true)
    {
        var cell = headerRow.AddCell();

        if (header != null)
        {
            var headerTextNode = ParentLayout.CreateTextNode(header);
            headerTextNode.AutoSize = true;
            headerTextNode.FontSize = 13;
            headerTextNode.AlignSelf = align ?? YogaAlign.Center;
            headerTextNode.IsBold = isBold;

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

    public void AlternateColor(Color first, Color second)
    {
        var startIndex = 1;
        if (headerRow.Display == YogaDisplay.None)
        {
            startIndex = 0;
        }

        for (int i = startIndex; i < Count; i++)
        {
            if (this[i] is TableRowNode tableRow)
            {
                tableRow.Background = i % 2 == 0 ? first : second;
            }
        }
    }

    public TextNode GetColumn(int index)
    {
        if (headerRow[index] is TableCellNode cell)
        {
            return (cell.GetChild(0) as TextNode)!;
        }

        throw new ArgumentOutOfRangeException(nameof(index), "Column index out of range");
    }
}