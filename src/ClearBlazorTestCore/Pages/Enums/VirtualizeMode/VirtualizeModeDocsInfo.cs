/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record VirtualizeModeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "VirtualizeMode";
        public string Description {get; set; } = "Indicates how a list of items is Virtualized.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("None", "VirtualizeMode", "No virtualization.\r"),
            new ApiFieldInfo("Virtualize", "VirtualizeMode", "Virtualizes a list of items when the height of each item is the same and the total number of items is known.\r"),
            new ApiFieldInfo("InfiniteScroll", "VirtualizeMode", "Virtualizes a list of items when the height of each item is not the same or the total number of \ritems is not known.\r"),
            new ApiFieldInfo("InfiniteScrollReverse", "VirtualizeMode", "Virtualizes a list of items when the height of each item is not the same or the total number of \ritems is not known. In addition it places the first items from the list data at the bottom \rof the scroll viewer and the thumb starts at the bottom. The scroll viewer can be scrolled up to\rget to the last items in the list data. Allows controls like 'WhatsApp' to be implemented. \r"),
            new ApiFieldInfo("Pagination", "VirtualizeMode", "Allows for paging through a list of items. \r"),
        };
    }
}
