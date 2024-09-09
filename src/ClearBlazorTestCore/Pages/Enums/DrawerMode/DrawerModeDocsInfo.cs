/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DrawerModeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "DrawerMode";
        public string Description {get; set; } = "Used by Drawer to indicate its mode.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Permanent", "DrawerMode", "When the drawer is open it stays open until the Open parameter is set to false.\r"),
            new ApiFieldInfo("Temporary", "DrawerMode", "When the drawer is open it automatically closes when the overlay is clicked, provided the overlay is showing.\r"),
            new ApiFieldInfo("Responsive", "DrawerMode", "The drawer closes if the Browser size is reduced to a DeviceSize of less than medium and reopens when the DeviceSize is \rgreater than or equal to medium.\r"),
        };
    }
}
