namespace ClearBlazor.Common
{
    public interface IOtherDocsInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ApiFieldInfo> FieldApi { get; set; }
    }
}
