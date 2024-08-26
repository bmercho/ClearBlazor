namespace ClearBlazorTest
{
    public interface IDocsEnumInfo
    {
        public string Name { get; }
        public string Description { get; }
        public List<ApiFieldInfo> FieldApi { get; }
    }
}