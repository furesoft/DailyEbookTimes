using Moss.NET.Sdk.LayoutEngine;

namespace Moss.NET.Sdk;

public class ContentLayout
{
    private readonly Dictionary<string, YogaNode> _sections = [];

    public ContentLayout(string name)
    {
        Layout = LayoutLoader.Load("layouts/content.xml", name);
        var sublayout = LayoutLoader.LoadFragment($"layouts/fragments/{name}.xml");
        var contentContainer = Layout.FindNode<YogaNode>("content")!;
        contentContainer.Parent.ReplaceChild(contentContainer, sublayout);

        foreach (var section in Layout.FindDescendantNodes("section"))
        {
            _sections.Add(section.ID, section);
        }
    }

    public Layout Layout { get; }

    public YogaNode this[string name] => _sections[name];
}