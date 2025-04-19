namespace Moss.NET.Sdk.LayoutEngine;

public interface IDataSource
{
    string Name { get; }
    void ApplyData(YogaNode node);
}