namespace ClearBlazor
{
    public interface IDropZone
    {
        public bool IsDroppable { get; set; }

        public string DropZoneName { get; set; }
    }
}
