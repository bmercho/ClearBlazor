namespace ClearBlazorTest
{
    public record ClearComponentBaseDocsInfo : IDocsInfo
    {
        public string DocsName => "ClearComponentBase";

        public string DocsDescription => "Defines the base component of all Clear components.";

        public (string,string) ApiLink => ("","");

        public (string, string) ExamplesLink => ("", "");

        public (string, string) InheritsLink => ("", "");

        public List<(string, string)> ImplementsLinks => new();

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