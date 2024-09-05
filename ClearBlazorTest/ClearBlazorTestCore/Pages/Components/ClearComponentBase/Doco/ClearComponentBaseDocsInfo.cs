/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ClearComponentBaseDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ClearComponentBase";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ClearComponentBaseApi");
        public (string, string) ExamplesLink {get; set; } = ("", "");
        public (string, string) InheritsLink {get; set; } = ("ComponentBase", "ComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBaseProperties", " IBasePropertiesApi"),
            (" IDisposable", " IDisposableApi"),
            (" IHandleEvent", " IHandleEventApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Tag", "object?", "null", ""),
            new ApiComponentInfo("Name", "string?", "null", ""),
            new ApiComponentInfo("Class", "string", "null", ""),
            new ApiComponentInfo("Style", "string", "null", ""),
            new ApiComponentInfo("Width", "double", "double.NaN", ""),
            new ApiComponentInfo("Height", "double", "double.NaN", ""),
            new ApiComponentInfo("MinWidth", "double", "0", ""),
            new ApiComponentInfo("MinHeight", "double", "0", ""),
            new ApiComponentInfo("MaxWidth", "double", "double.PositiveInfinity", ""),
            new ApiComponentInfo("MaxHeight", "double", "double.PositiveInfinity", ""),
            new ApiComponentInfo("Margin", "string", "String.Empty", ""),
            new ApiComponentInfo("Padding", "string", "String.Empty", ""),
            new ApiComponentInfo("HorizontalAlignment", "<a href=AlignmentApi>Alignment?</a>", "null", ""),
            new ApiComponentInfo("VerticalAlignment", "<a href=AlignmentApi>Alignment?</a>", "null", ""),
            new ApiComponentInfo("Row", "int", "0", ""),
            new ApiComponentInfo("Column", "int", "0", ""),
            new ApiComponentInfo("RowSpan", "int", "1", ""),
            new ApiComponentInfo("ColumnSpan", "int", "1", ""),
            new ApiComponentInfo("Dock", "<a href=DockApi>Dock?</a>", "null", ""),
            new ApiComponentInfo("LastChildFill", "bool?", "null!", ""),
            new ApiComponentInfo("OnClicked", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnDoubleClicked", "EventCallback<MouseEventArgs>", "", ""),
            new ApiComponentInfo("OnMouseMoved", "EventCallback<MouseEventArgs>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Refresh()", "void", "", ""),
            new ApiComponentInfo("GetBoxShadowCss(int? boxShadow)", "string", "", ""),
        };
    }
}
