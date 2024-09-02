using ClearBlazorTest;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace CreateDocumentation
{
    public class ApiDoco
    {
        public bool Execute(string srcPath)
        {
            var testComponentsFolder = Path.Combine(srcPath, Paths.TestComponentsFolder);
            var componentsFolder = Path.Combine(srcPath, Paths.ComponentsFolder);
            var directoryInfo = new DirectoryInfo(testComponentsFolder);

            foreach (var entry in directoryInfo.GetFiles("*ExamplesPage.razor", SearchOption.AllDirectories))
            {
                string testComponentFolder = entry.DirectoryName == null ? string.Empty : entry.DirectoryName;
                string componentName = entry.Name.Substring(0, entry.Name.LastIndexOf("ExamplesPage.razor"));

                string? fileName = GetComponentFileName(componentsFolder, componentName);

                if (fileName == null)
                {
                    Console.WriteLine($"ApiDoco: Component '{componentName}' was not found (or more than one found)");
                    continue;
                }

                var docInfo = GetComponentDoco(componentName, fileName);

                WriteDocsInfoFile(testComponentFolder, docInfo);

            }
            return true;
        }

        private string? GetComponentFileName(string componentsFolder, string componentName)
        {
            var info = new DirectoryInfo(componentsFolder);

            var files = info.GetFiles($"{componentName}.cs", SearchOption.AllDirectories);

            if (files.Count() == 1)
                return files[0].FullName;
            else
            {
                files = info.GetFiles($"{componentName}.razor.cs", SearchOption.AllDirectories);

                if (files.Count() == 1)
                    return files[0].FullName;
                else
                    return null;
            }
        }

        private DocoInfo GetComponentDoco(string componentName, string fileName)
        {
            DocoInfo info = new DocoInfo();
            info.Name = componentName;

            return info;
        }
        private void WriteDocsInfoFile(string testComponentFolder, DocoInfo docInfo)
        {
            var folder = Path.Combine(testComponentFolder, Paths.DocsFolder);
            var docFileName = folder + @$"\{docInfo.Name}DocsInfo";
            Directory.CreateDirectory(folder);
            string jsonString = JsonSerializer.Serialize(docInfo);
            File.WriteAllText(docFileName, jsonString); 
        }
    }


    public record DocoInfo
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public (string, string) ApiLink { get; set; }

        public (string, string) ExamplesLink { get; set; }

        public (string, string) InheritsLink { get; set; }

        public List<(string, string)> ImplementsLinks { get; set; } = new();

        public List<ApiComponentInfo> ParameterApi { get; set; } = new();

        public List<ApiComponentInfo> PropertyAp { get; set; } = new();
        public List<ApiComponentInfo> MethodApi { get; set; } = new();
        public List<ApiComponentInfo> EventApi { get; set; } = new();
    }
}
