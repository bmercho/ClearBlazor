/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record NonVirtualizedTreeDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "NonVirtualizedTree<TItem>";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "NonVirtualizedTreeApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "NonVirtualizedTree");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            ("", "Api"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RenderFragment<(TItem", "required", "null", "The template for rendering each row.\rThe item is passed to each child for customization of the row\r"),
            new ApiComponentInfo("Items", "IEnumerable<TItem>?", "null", " The items to be displayed in the list. If this is not null DataProvider is used.\r If DataProvider is also not null then Items takes precedence.\r"),
            new ApiComponentInfo("DataProvider", "DataProviderRequestDelegate<TItem>?", "null", "Defines the data provider used to get pages of data from where ever. eg database\rUsed if Items is null.\r"),
            new ApiComponentInfo("index,", "(int", "(0, Alignment.Start)", "Gets or sets the index of the Items to be initially shown in visible area.\rIt can be shown in the centre, start or end of the visible are.\r"),
            new ApiComponentInfo("HorizontalContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", "The horizontal content alignment within the control.\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task OnRowClicked(MouseEventArgs e, TItem item)", "async", "", ""),
        };
    }
}
