using ClearBlazor;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace ClearBlazorTest
{
    public static class GridApi
    {
        public static List<ApiComponentInfo> ParameterApi = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "<a href=\"/Alignment\">Alignment</a>", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
        public static List<ApiComponentInfo> PropertyApi = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "int", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
        public static List<ApiComponentInfo> MethodApi = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "int", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
        public static List<ApiComponentInfo> EventApi = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Columns", "string", "*", "Defines columns by a comma delimited string of column widths. eg *,2*,auto,200"),
            new ApiComponentInfo("Rows", "string", "*", "Defines columns by a comma delimited string of column widths.  eg *,2*,auto,200"),
            new ApiComponentInfo("ColumnSpan", "int", "1", "Defines column spans. Indicates the number of columns this column spans."),
            new ApiComponentInfo("RowSpan", "int", "1", "Defines row spans. Indicates the number of rows this row spans.")
        };
    }
}