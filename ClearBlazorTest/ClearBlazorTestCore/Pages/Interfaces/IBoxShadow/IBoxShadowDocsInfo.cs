/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IBoxShadowDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "IBoxShadow";
        public string Description {get; set; } = "Defines a box shadow surrounding a component.\rNote that it is drawn outside of the component so there needs to space around the component to show the box shadow.\rSet a margin if there is no room to show it.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("BoxShadow", "int?", "The level of box shadow:\r    0    - no box shadow\r    1-5  - greater shadow as number increases\r"),
        };
    }
}
