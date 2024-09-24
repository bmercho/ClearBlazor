/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SelectionModeDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "SelectionMode";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("None", "SelectionMode", "Not selectable\r"),
            new ApiFieldInfo("Single", "SelectionMode", "Can only select a single item\r"),
            new ApiFieldInfo("SimpleMulti", "SelectionMode", "Can select multiple rows by clicking rows.\rClick to select and then click to unselect\r"),
            new ApiFieldInfo("Multi", "SelectionMode", "Can select multiple rows clicking rows using the standard\rwindows selection method, using click, ctl click and ctl shift click.\r"),
        };
    }
}
