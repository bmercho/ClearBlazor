/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ScrollViewerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ScrollViewer";
        public string Description {get; set; } = "A ScrollViewer provides a scrollable area that can contain child elements. \r";
        public (string, string) ApiLink  {get; set; } = ("API", "ScrollViewerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ScrollViewer");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("HorizontalScrollMode", "<a href=ScrollModeApi>ScrollMode</a>", "ScrollMode.Disabled", "The horizontal scroll mode.\r"),
            new ApiComponentInfo("VerticalScrollMode", "<a href=ScrollModeApi>ScrollMode</a>", "ScrollMode.Auto", "The horizontal scroll mode.\r"),
            new ApiComponentInfo("VerticalOverscrollBehaviour", "<a href=OverscrollBehaviourApi>OverscrollBehaviour</a>", "OverscrollBehaviour.Auto", "Defines what happens when the boundary of a scrolling area is reached in the vertical direction. \r"),
            new ApiComponentInfo("HorizontalOverscrollBehaviour", "<a href=OverscrollBehaviourApi>OverscrollBehaviour</a>", "OverscrollBehaviour.Auto", "Defines what happens when the boundary of a scrolling area is reached in the horizontal direction. \r"),
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("IDisposable Subscribe(IObserver<bool> observer)", "static", "", ""),
            new ApiComponentInfo("observers, IObserver<bool> observer)", "Unsubscriber(List<IObserver<bool>>", "", ""),
            new ApiComponentInfo("Dispose()", "void", "", ""),
        };
    }
}
