/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SpinnerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Spinner";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "SpinnerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Spinner");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
