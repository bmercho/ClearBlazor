/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ClearComponentBaseDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ClearComponentBase";
        public string Description {get; set; } = "An abstract base class for all components.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ClearComponentBaseApi");
        public (string, string) ExamplesLink {get; set; } = ("", "");
        public (string, string) InheritsLink {get; set; } = ("ComponentBase", "ComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IDisposable", " IDisposableApi"),
            (" IHandleEvent", " IHandleEventApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Name", "string?", "null", "Name of component\r"),
            new ApiComponentInfo("Class", "string", "null", "Optional classes to be applied to component\r"),
            new ApiComponentInfo("Style", "string", "null", "Optional styles to be applied to the component\r"),
            new ApiComponentInfo("Width", "double", "double.NaN", "The width of the component. Takes precedence over other layout requirements.\r"),
            new ApiComponentInfo("Height", "double", "double.NaN", "The height of the component. Takes precedence over other layout requirements.\r"),
            new ApiComponentInfo("MinWidth", "double", "0", "The minimum width of the component. Takes precedence over other layout requirements.\r"),
            new ApiComponentInfo("MinHeight", "double", "0", "The minimum height of the component. Takes precedence over other layout requirements.\r"),
            new ApiComponentInfo("MaxWidth", "double", "double.PositiveInfinity", "The maximum width of the component. Takes precedence over other layout requirements, apart from width.\r"),
            new ApiComponentInfo("MaxHeight", "double", "double.PositiveInfinity", "The maximum height of the component. Takes precedence over other layout requirements, apart from height.\r"),
            new ApiComponentInfo("Margin", "string", "String.Empty", "The margin of the component.\rCan be in the format of:\r    4 - all margins are 4px\r    4,8 - top and bottom margins are 4px radius, left and right margins have 8px radius\r    20,10,30,40 - top has 20px margin, right has 10px margin, bottom has 30px margin and left has 40px margin\r"),
            new ApiComponentInfo("Padding", "string", "String.Empty", "The padding of the component.\rCan be in the format of:\r    4 - all paddings are 4px\r    4,8 - top and bottom paddings are 4px radius, left and right paddings have 8px radius\r    20,10,30,40 - top has 20px padding, right has 10px padding, bottom has 30px padding and left has 40px padding\r"),
            new ApiComponentInfo("HorizontalAlignment", "<a href=AlignmentApi>Alignment?</a>", "null", "The horizontal alignment of the component in its available space.\r"),
            new ApiComponentInfo("VerticalAlignment", "<a href=AlignmentApi>Alignment?</a>", "null", "The vertical alignment of the component in its available space.\r"),
            new ApiComponentInfo("Tag", "int?", "null", "Tag is user definable value.\r"),
            new ApiComponentInfo("Row", "int", "0", "Applies to children of a grid. Indicates the start row of the grid that the child will occupy. \rThe first row is 0.\r"),
            new ApiComponentInfo("Column", "int", "0", "Applies to children of a grid. Indicates the start column of the grid that the child will occupy. \rThe first column is 0.\r"),
            new ApiComponentInfo("RowSpan", "int", "1", "Applies to children of a grid. Indicates how many rows of the grid that the child will occupy (starting at Row). \r"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Applies to children of a <a href=\"GridPage\">Grid</a>. Indicates how many columns of the grid that the child will occupy (starting at Column). \r"),
            new ApiComponentInfo("Dock", "<a href=DockApi>Dock?</a>", "null", "Applies to children of a <a href=\"GridPage\">DockPanel</a>. \rIndicates how the component will dock in its parent.\r"),
            new ApiComponentInfo("OnClicked", "EventCallback<MouseEventArgs>", "", "Event raised when the component is clicked \r"),
            new ApiComponentInfo("OnDoubleClicked", "EventCallback<MouseEventArgs>", "", "Event raised when the component is double clicked \r"),
            new ApiComponentInfo("OnMouseMoved", "EventCallback<MouseEventArgs>", "", "Event raised when the mouse is moved over the component \r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
