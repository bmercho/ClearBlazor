/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record PaginationDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Pagination";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "PaginationApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Pagination");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Color", "Color?", "null", ""),
            new ApiComponentInfo("NumPages", "int", "0", ""),
            new ApiComponentInfo("NumPagesShown", "int", "0", ""),
            new ApiComponentInfo("ShowFirstAndLastButtons", "bool", "false", ""),
            new ApiComponentInfo("SelectedPage", "int", "1", ""),
            new ApiComponentInfo("SelectedPageChanged", "EventCallback<int>", "", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
