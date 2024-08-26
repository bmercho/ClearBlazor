namespace ClearBlazorTest
{
    public interface IDocsInterfaceInfo
    {
        public string Name { get; }
        public string Description { get; }
        public List<ApiFieldInfo> FieldApi { get; }
    }
}