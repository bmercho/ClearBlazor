namespace ClearBlazorTest
{
    public record GridDocsInfo:IDocsInfo
    {
        public string Name => "Grid";

        public string Description => "Defines a flexible grid area that consists of columns and rows.";

        public (string,string) ApiLink => ("API", "GridApi");

        public (string, string) ExamplesLink => ("Examples","Grid");

        public (string, string) InheritsLink => ("ClearComponentBase", "ClearComponentBaseApi");

        public List<(string, string)> ImplementsLinks => new() { ("IBackground", "IBackground"), ("IBorder", "IBorder") };

        public List<ApiComponentInfo> ParameterApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "<a href=\"/Alignment\">Alignment</a>", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
        public List<ApiComponentInfo> PropertyApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "int", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
        public List<ApiComponentInfo> MethodApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "int", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
        public List<ApiComponentInfo> EventApi => new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "int", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
    }
}