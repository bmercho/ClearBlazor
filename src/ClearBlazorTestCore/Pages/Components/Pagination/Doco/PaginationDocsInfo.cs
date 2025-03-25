/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record PaginationDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Pagination";
        public string Description {get; set; } = "A pagination control component.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "PaginationApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Pagination");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of the component.\r"),
            new ApiComponentInfo("Color", "Color?", "null", "The color of the component.\r"),
            new ApiComponentInfo("NumPages", "int", "0", "The number of pages.\r"),
            new ApiComponentInfo("NumPagesShown", "int", "0", "The number of pages shown.\r"),
            new ApiComponentInfo("ShowFirstAndLastButtons", "bool", "false", "Whether to show the first and last buttons.\r"),
            new ApiComponentInfo("SelectedPage", "int", "1", "The selected page.\r"),
            new ApiComponentInfo("SelectedPageChanged", "EventCallback<int>", "", "The event that is raised when the selected page changes.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
