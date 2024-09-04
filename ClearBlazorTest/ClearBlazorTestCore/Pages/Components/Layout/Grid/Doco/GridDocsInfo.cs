/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record GridDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Grid";
        public string Description {get; set; } = "Defines a flexible grid area that consists of columns and rows.\r";
        public (string, string) ApiLink  {get; set; } =  ("{docInfo.ApiLink.Item1}", "{docInfo.ApiLink.Item2}");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Grid");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IContent", "IContent"),
            ("IBackground", "IBackground"),
            ("IBoxShadow", "IBoxShadow"),
            (" IBorder", " IBorder"),
            (" IBackgroundGradient", " IBackgroundGradient"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. \rA column width consists of one to three values separated by colons. The seconds and third values are optional.\rThe first value can be one of:\r   '*'    - a weighted proportion of available space.\r   'auto' - the minimum width of the content\r   value  - a pixel value for the width\rThe second value is the minimum width in pixels\rThe third value is the maximum width in pixels\r\reg *,2*,auto,200  - Four columns, the 3rd column auto sizes to content, the 4th column is 200px wide, and the remaining space\ris shared between columns 1 and 2 but column 2 is twice as wide as column 1.\reg *:100:200,* - Two columns sharing available width equally except column 1 must be a minimum of 100px and a maximum of 200px. \rSo if the available width is 600px then column 1 will be 200px and column 2 400px.\rIf the available width is 150px then column 1 will be 100px and column 2 50px.\rIf the available width is 300px then column 1 will be 150px and column 2 150px.\r"),
            new ApiComponentInfo("Rows", "string", "*", "Defines rows by a comma delimited string of row heights which are similar to columns. \r"),
            new ApiComponentInfo("ColumnSpacing", "double", "0", "The spacing in pixels between each column\r"),
            new ApiComponentInfo("RowSpacing", "double", "0", "The spacing in pixels between each row\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
