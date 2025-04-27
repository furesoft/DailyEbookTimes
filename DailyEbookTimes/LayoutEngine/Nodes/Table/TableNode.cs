namespace Moss.NET.Sdk.LayoutEngine.Nodes.Table;

public class TableNode : YogaNode
{
    public List<YogaNode> Columns = [];

    public TableNode(YogaConfig config, Layout parentLayout) : base(config, parentLayout)
    {
        FlexDirection = YogaFlexDirection.Column;
    }

    public TableRowNode AddRow()
    {
        TableRowNode row = new(Config, ParentLayout);
        Add(row);
        return row;
    }

    public void AddColumn()
    {
        Columns.Add(ParentLayout.CreateNode());
    }
}