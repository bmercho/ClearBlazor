/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ListViewDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ListView<TItem>";
        public string Description {get; set; } = "Displays a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ListViewApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ListView");
        public (string, string) InheritsLink {get; set; } = ("ListViewBase<TItem>", "ListViewBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RowTemplate", "RenderFragment<TItem>?", "null", "The template for rendering each row.\rThe item is passed to each child for customization of the row\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
