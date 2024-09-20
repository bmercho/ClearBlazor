/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record VirtualizeDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Virtualize<TItem>";
        public string Description {get; set; } = "Virtualizes a list of items( of type 'IItem') inside a ScrollViewer which is embedded in this component.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "VirtualizeApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Virtualize");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBorder", "IBorderApi"),
            ("IBackground", "IBackgroundApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("RenderFragment<TItem>?", "required", "null", "The child content of this control.\rThe item is passed to each child for customization of the row\r"),
            new ApiComponentInfo("Items", "IEnumerable<TItem>?", "null", " The items to be displayed in the list. If this is not null ItemsProvider is used.\r If ItemsProvider is also not null then Items takes precedence.\r"),
            new ApiComponentInfo("ItemsProvider", "ItemsProviderRequestDelegate<TItem>?", "null", "Defines the data provider used to get pages of data from where ever. eg database\rUsed if Items is null.\r"),
            new ApiComponentInfo("ItemHeight", "int?", "null", "The height to be used for each item.\rThis is optional in which case the height is obtained from the first item.\r"),
            new ApiComponentInfo("index,", "(int", "(0, Alignment.Start)", "Gets or sets the index of the Items to be displayed in the centre of the visible area \r(except if it near the start or end of list, where it wont be in the centre) \r"),
            new ApiComponentInfo("HorizontalContentAlignment", "<a href=AlignmentApi>Alignment</a>", "Alignment.Stretch", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=IBoxShadowApi>IBoxShadow</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=IBackgroundApi>IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Task GotoIndex(int index, Alignment verticalAlignment)", "async", "", ""),
            new ApiComponentInfo("Task HandleScrollEvent(ScrollState scrollState)", "async", "", ""),
        };
    }
}
