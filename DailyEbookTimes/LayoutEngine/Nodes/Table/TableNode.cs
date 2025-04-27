namespace Moss.NET.Sdk.LayoutEngine.Nodes.Table;

public class TableNode : YogaNode
{
    public List<YogaNode> Columns = [];

    public TableNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        FlexDirection = YogaFlexDirection.Column;
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
    public TableRowNode AddHeaderRow()
    {
        var row = AddRow("header");

        foreach (var column in Columns)
        {
            var cell = row.AddCell();
            cell.Add(column);
        }

        return row;
    }

    public void AddColumn(string? header = null)
    {
        Columns.Add(ParentLayout.CreateNode());

        if (header != null)
        {
            var headerCell = ParentLayout.CreateTextNode(header);
            headerCell.AutoSize = true;
            headerCell.JustifyContent = YogaJustify.Center;
            Columns.Last().Add(headerCell);
        }
    }
}