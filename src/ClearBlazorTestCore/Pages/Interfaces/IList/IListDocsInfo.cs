/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IListDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IList<TItem>";
        public string Description {get; set; } = "Defines a interface for the list type components, NonVirtualizedList, VirtualizedList or InfiniteScrollerList\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("firstIndex,", "GetSelections(int", ""),
        };
    }
}
