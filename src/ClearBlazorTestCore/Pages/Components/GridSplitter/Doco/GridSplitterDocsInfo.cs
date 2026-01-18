/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record GridSplitterDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "GridSplitter";
        public string Description {get; set; } = "Represents a GridSplitter component.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "GridSplitterApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "GridSplitter");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Color", "Color?", "null", "Color of spinner.\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "Size of spinner.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
