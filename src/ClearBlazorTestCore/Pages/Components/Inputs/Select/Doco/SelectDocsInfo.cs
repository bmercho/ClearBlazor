/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record SelectDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Select<TItem>";
        public string Description {get; set; } = "Represents a selectable input control that can display a list of items and allows for single or multiple\rselections.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "SelectApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Select");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<TItem>", "ContainerInputBase<TItem>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("SelectData", "List<ListDataItem<TItem>>?", "null", "Holds a list of selectable data items of type TItem. It can be null and is used for data binding in\rcomponents.\r"),
            new ApiComponentInfo("Values", "List<TItem?>?", "null", "A list of nullable items of type TItem. It can be set to null and is used to hold multiple values.\r"),
            new ApiComponentInfo("ValuesChanged", "EventCallback<List<TItem?>>", "", "An event callback that triggers when the list of values changes. It allows for handling updates to the list\rof items.\r"),
            new ApiComponentInfo("MultiSelect", "bool", "false", "Indicates whether multiple selections are allowed. Defaults to false.\r"),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomLeft", "Defines the position of a popup, defaulting to the bottom left corner. It uses the PopupPosition enumeration\rfor various placement options.\r"),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopLeft", "\r"),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", "Indicates whether vertical flipping is permitted. Defaults to true.\r"),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", "Indicates whether horizontal flipping is permitted. Defaults to true.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
