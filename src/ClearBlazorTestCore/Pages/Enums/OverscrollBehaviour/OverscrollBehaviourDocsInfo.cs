/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record OverscrollBehaviourDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "OverscrollBehaviour";
        public string Description {get; set; } = "Defines what happens when the boundary of a scrolling area is reached.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Auto", "OverscrollBehaviour", "The default scroll overflow behavior occurs as normal.\rie a parent scrolling area may scroll\r"),
            new ApiFieldInfo("Contain", "OverscrollBehaviour", "Default scroll overflow behavior (e.g., \"bounce\" effects) \ris observed inside the element where this value is set. \rHowever, no scroll chaining occurs on neighboring scrolling areas. \rThe underlying elements will not scroll. \rThe contain value disables native browser navigation, \rincluding the vertical pull-to-refresh gesture and horizontal swipe navigation.\r"),
            new ApiFieldInfo("None", "OverscrollBehaviour", "No scroll chaining occurs to neighboring scrolling areas, \rand default scroll overflow behavior is prevented.\r"),
        };
    }
}
