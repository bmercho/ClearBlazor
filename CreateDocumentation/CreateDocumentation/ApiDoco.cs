using ClearBlazorTest;
using System.Collections.Generic;
using System.Xml.Linq;

namespace CreateDocumentation
{
    public class ApiDoco
    {
        public bool Execute(string srcPath)
        {
            var testComponentsFolder = Path.Combine(srcPath, Paths.TestComponentsFolder);
            var componentsFolder = Path.Combine(srcPath, Paths.ComponentsFolder);
            var directoryInfo = new DirectoryInfo(testComponentsFolder);

            //foreach(var entry in Directory.EnumerateFiles(testComponentsFolder, "*.razor", SearchOption.AllDirectories)
            //.Where(s => s.Contains("ExamplesPage) || s.Contains("ApiPage"));

            foreach (var entry in directoryInfo.GetFiles("*ApiPage.razor", SearchOption.AllDirectories))
            {
                string testComponentFolder = entry.DirectoryName == null ? string.Empty : entry.DirectoryName;
                string componentName = entry.Name.Substring(0, entry.Name.LastIndexOf("ApiPage.razor"));

                string? fileName = GetComponentFileName(componentsFolder, componentName);

                if (fileName == null)
                {
                    Console.WriteLine($"ApiDoco: Component '{componentName}' was not found (or more than one found)");
                    continue;
                }

                var examplesFilename = testComponentFolder + @$"\{componentName}ExamplesPage.razor";

                bool hasExamples = File.Exists(examplesFilename);

                var docInfo = GetComponentDoco(componentName, fileName, hasExamples);

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

        private DocoInfo GetComponentDoco(string componentName, string fileName, bool hasExamples)
        {
            DocoInfo info = new DocoInfo();
            info.Name = componentName;
            info.ApiLink = ("API", $"{componentName}Api");
            if (hasExamples)
                info.ExamplesLink = ("Examples", $"{componentName}");
            var lines = File.ReadAllLines(fileName);
            int line = 0;

            (string inherits, List<string> implements) = ExtractInheritsAndImplements(componentName, lines, line);
            if (inherits != null)
                info.InheritsLink = (inherits, $"{inherits}Api");
            if (implements.Count() > 0)
                foreach (var link in implements)
                    info.ImplementsLinks.Add((link, link));
            line = 0;


            while (line < lines.Count())
            {
                var comment = FindNextComment(lines, ref line);
                if (comment != string.Empty)
                {
                    if (lines[line].Contains($"class {componentName}"))
                        info.Description = comment;
                    else if (lines[line].Trim().StartsWith("[Parameter]"))
                    {
                        line++;
                        var values = lines[line].Trim().Split(" ");
                        if (values[1] == "virtual" || values[1] == "override")
                        {
                            var l = values.ToList();
                            l.Remove(values[1]);
                            values = l.ToArray();   
                        }
                        var index = lines[line].IndexOf('=');
                        string def = string.Empty;
                        if (!lines[line].Contains("EventCallback"))
                            if (index == -1)
                                def = "null";
                            else
                                def = lines[line].Substring(index + 1).Trim().Trim(';').Trim('"');
                        var typ = values[1];
                        typ.Replace("<", @"\<");
                        typ.Replace(">", @"\>");
                        info.ParameterApi.Add(new ApiComponentInfo(values[2], typ, def, comment));
                    }
                    else if (lines[line].Trim().StartsWith("public") &&
                             !lines[line].Contains("=") &&
                             lines[line].Trim().EndsWith(")"))
                    {
                        var values = lines[line].Trim().Split(" ");
                        info.MethodApi.Add(new ApiComponentInfo(values[2], values[1], "", comment));
                    }

                }
            }

            return info;
        }

        private (string inherits, List<string> implements) ExtractInheritsAndImplements(string componentName, string[] lines, int line)
        {
            string inherits = string.Empty;
            List<string> implements = new();
            while (line < lines.Count())
            {
                int index = lines[line].IndexOf($"class {componentName}:");
                if (index > 0)
                {

                    var values = lines[line].Substring(index + $"class {componentName}:".Length).Split(',');

                    if (values.Length > 0)
                        if (!values[0].StartsWith("I"))
                        {
                            inherits = values[0];
                            values = values.Skip(1).Select(s => s).ToArray();
                        }
                    implements = values.ToList();

                    return (inherits, implements);
                }
                line++;
            }
            return (inherits, implements);
        }

        private string FindNextComment(string[] lines, ref int line)
        {
            string comment = string.Empty;
            while (line < lines.Count() && !lines[line].Trim().StartsWith("///"))
            {
                line++;
                if (line == lines.Length)
                    return string.Empty;
            }
            while (line < lines.Count() && !lines[line].Contains("</summary>"))
            { 
                if (!lines[line].Contains("<summary>"))
                {
                    comment += lines[line].Substring(lines[line].IndexOf("/// ") + 4) + "\\r";
                }

                line++;
            }
            line++;
            return comment;
        }

        private void WriteDocsInfoFile(string testComponentFolder, DocoInfo docInfo)
        {
            var folder = Path.Combine(testComponentFolder, Paths.DocsFolder);
            var docFileName = folder + @$"\{docInfo.Name}DocsInfo.cs";
            Directory.CreateDirectory(folder);

            var lines = new List<string>();
            lines.Add("namespace ClearBlazorTest");
            lines.Add("{");
            lines.Add($"    public record {docInfo.Name}DocsInfo:IDocsInfo");
            lines.Add("    {");
            lines.Add($"        public string Name => \"{docInfo.Name}\";");
            lines.Add($"        public string Description => \"{docInfo.Description}\";");
            lines.Add($"        public (string, string) ApiLink => (\"{docInfo.ApiLink.Item1}\", \"{docInfo.ApiLink.Item2}\");");
            lines.Add($"        public (string, string) ExamplesLink => (\"{docInfo.ExamplesLink.Item1}\", \"{docInfo.ExamplesLink.Item2}\");");
            lines.Add($"        public (string, string) InheritsLink => (\"{docInfo.InheritsLink.Item1}\", \"{docInfo.InheritsLink.Item2}\");");

            lines.Add($"        public List<(string, string)> ImplementsLinks => new()");
            lines.Add("        {");
            foreach (var link in docInfo.ImplementsLinks)
                lines.Add($"        (\"{link.Item1}\", \"{link.Item2}\"),");
            lines.Add("        };");

            lines.Add("        public List<ApiComponentInfo> ParameterApi => new List<ApiComponentInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.ParameterApi)
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description}\"),");
            lines.Add("        };");

            lines.Add("        public List<ApiComponentInfo> PropertyApi => new List<ApiComponentInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.PropertyApi)
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description}\"),");
            lines.Add("        };");

            lines.Add("        public List<ApiComponentInfo> MethodApi => new List<ApiComponentInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.MethodApi)
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description}\"),");
            lines.Add("        };");

            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(docFileName, lines);
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

        public List<ApiComponentInfo> PropertyApi { get; set; } = new();
        public List<ApiComponentInfo> MethodApi { get; set; } = new();
        public List<ApiComponentInfo> EventApi { get; set; } = new();
    }
}
