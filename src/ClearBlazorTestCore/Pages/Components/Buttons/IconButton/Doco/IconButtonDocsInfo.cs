/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IconButtonDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "IconButton";
        public string Description {get; set; } = "A button that shows an icon and possibly text.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "IconButtonApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "IconButton");
        public (string, string) InheritsLink {get; set; } = ("Button", "ButtonApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("LeadingIcon", "bool", "true", "Used when the IconButton is inside an AppBar\rIndicates if this icon is a leading icon. (otherwise its a trailing icon)\rLeading and trailing icons get slightly different colors.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
