/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record UniformGridDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "UniformGrid";
        public string Description {get; set; } = "The UniformGrid control is a layout control which arranges items in a evenly-spaced set of rows or columns\rto fill the total available display space. Each cell in the grid, by default, will be the same size.\rIf no value for Rows and Columns are provided, the UniformGrid will create a square layout\rbased on the total number of visible items.\rIf a fixed size is provided for Rows and Columns then additional children \rthat can't fit in the number of cells provided won't be displayed.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "UniformGridApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "UniformGrid");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBackground", "IBackgroundApi"),
            ("IBorder", "IBorderApi"),
            ("IBoxShadow", "IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("NumRows", "int?", "null", "The number of rows to display. If not provided the number of rows will be automatically determined. \r"),
            new ApiComponentInfo("NumColumns", "int?", "null", "The number of columns to display. If not provided the number of columns will be automatically determined. \r"),
            new ApiComponentInfo("RowSpacing", "int?", "null", "The spacing between rows.\r"),
            new ApiComponentInfo("ColumnSpacing", "int?", "null", "The spacing between columns.\r"),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=IBorderApi>IBorder</a>\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=IBackgroundApi>IBackground</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
