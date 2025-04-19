namespace Moss.NET.Sdk.LayoutEngine.Nodes;

public abstract class Component
{
    public abstract string Tag { get; }
    public abstract string GetLayout();

    internal virtual void AfterLoad(YogaNode node)
    {
    }
}