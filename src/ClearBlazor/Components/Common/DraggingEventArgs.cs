using Microsoft.AspNetCore.Components.Web;
namespace ClearBlazor
{
    public class DraggingEventArgs
    {
        public DraggingEventArgs(PointerEventArgs pointerEventArgs)
        {
            PointerEventArgs = pointerEventArgs;
        }
        public PointerEventArgs PointerEventArgs { get; set; }

        public string DragId { get; set; } = string.Empty;

        public DragType DragType { get; set; } = DragType.Move;

        public string? Cursor { get; set; } = null;  

        public ClearComponentBase? DragZone { get; set; } = null;

        public ClearComponentBase? DropZone { get; set; } = null;

        public bool AllowDrop { get; set; } = false; 
    }
}