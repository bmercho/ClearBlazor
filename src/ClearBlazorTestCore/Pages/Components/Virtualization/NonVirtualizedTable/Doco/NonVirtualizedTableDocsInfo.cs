/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record NonVirtualizedTableDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "NonVirtualizedTable<TItem>";
        public string Description {get; set; } = "TableView is a templated table component supporting virtualization and allowing multiple selections.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "NonVirtualizedTableApi");
        public (string, string) ExamplesLink {get; set; } = ("", "");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBackground", " IBackgroundApi"),
            ("IBoxShadow", "IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control. Contains the columns for that table.\r"),
            new ApiComponentInfo("Items", "List<TItem>", "new List<TItem>()", " The items to be displayed in the list. If this is not null DataProvider is used.\r If DataProvider is also not null then Items takes precedence.\r"),
            new ApiComponentInfo("DataProvider", "DataProviderRequestDelegate<TItem>?", "null", "Defines the data provider used to get pages of data from where ever. eg database\rUsed if Items is null.\r"),
            new ApiComponentInfo("VirtualizeMode", "<a href=VirtualizeModeApi>VirtualizeMode</a>", "VirtualizeMode.None", "If true it ignores ItemHeight and internally uses the InfiniteScroller component\r"),
            new ApiComponentInfo("ColumnDefs", "string", "", ""),
            new ApiComponentInfo("RowSpacing", "int", "5", ""),
            new ApiComponentInfo("ColumnSpacing", "int", "5", ""),
            new ApiComponentInfo("HorizontalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", ""),
            new ApiComponentInfo("VerticalGridLines", "<a href=GridLinesApi>GridLines</a>", "GridLines.None", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColor", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
