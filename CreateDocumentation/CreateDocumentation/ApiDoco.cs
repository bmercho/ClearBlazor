using ClearBlazor.Common;

namespace CreateDocumentation
{
    public class ApiDoco
    {
        public void Execute(string srcPath)
        {
            var testComponentsFolder = Path.Combine(srcPath, Paths.TestComponentsFolder);
            var testEnumsFolder = Path.Combine(srcPath, Paths.TestEnumsFolder);
            var testInterfacesFolder = Path.Combine(srcPath, Paths.TestInterfacesFolder);
            var componentsFolder = Path.Combine(srcPath, Paths.ComponentsFolder);

            CreateEnumDoco(testEnumsFolder, componentsFolder);

            CreateInterfaceDoco(testInterfacesFolder, componentsFolder);

            CreateComponentDoco(testComponentsFolder, componentsFolder);

        }

        private void CreateEnumDoco(string testEnumsFolder, string componentsFolder)
        {

            var enumerationsFolder = Path.Combine(componentsFolder, "Enumerations");
            var directoryInfo = new DirectoryInfo(enumerationsFolder);
            foreach (var entry in directoryInfo.GetFiles("*.cs", SearchOption.TopDirectoryOnly))
            {
                OtherDocsInfo info = new();
                var lines = File.ReadAllLines(entry.FullName);
                int line = 0;
                info.Name = FindName(lines, ref line, "public enum", 2);
                if (info.Name == string.Empty)
                {
                    Console.WriteLine("Enumeration name not found");
                    return;
                }
                info.Description = GetAssociatedComment(lines, ref line);
                line++;
                while (true)
                {
                    ApiFieldInfo api = GetNextEnumApi(lines, ref line, info.Name);
                    if (api.Name == string.Empty)
                        break;
                    info.FieldApi.Add(api);
                    SkipBlankLines(lines, ref line);
                    if (lines[line].Trim().StartsWith('}'))
                        break;
                }

                string testEnumFolder = Path.Combine(testEnumsFolder, info.Name);

                WriteOtherDocsInfoFile(testEnumFolder, info);

                WriteApiPageFile(testEnumFolder, info.Name);
            }
        }

        private void CreateInterfaceDoco(string testInterfacesFolder, string componentsFolder)
        {
            var interfacesFolder = Path.Combine(componentsFolder, "Interfaces");
            var directoryInfo = new DirectoryInfo(interfacesFolder);
            foreach (var entry in directoryInfo.GetFiles("*.cs", SearchOption.TopDirectoryOnly))
            {
                OtherDocsInfo info = new();
                var lines = File.ReadAllLines(entry.FullName);
                int line = 0;
                info.Name = FindName(lines, ref line, "public interface", 2);
                if (info.Name == string.Empty)
                {
                    Console.WriteLine("Enumeration name not found");
                    return;
                }
                info.Description = GetAssociatedComment(lines, ref line);
                line++;
                while (true)
                {
                    ApiFieldInfo api = GetNextInterfaceApi(lines, ref line);
                    if (api.Name == string.Empty)
                        break;
                    info.FieldApi.Add(api);
                    SkipBlankLines(lines, ref line);
                    if (lines[line].Trim().StartsWith('}'))
                        break;
                }

                string testEnumFolder = Path.Combine(testInterfacesFolder, info.Name);

                WriteOtherDocsInfoFile(testEnumFolder, info);

                WriteApiPageFile(testEnumFolder, info.Name);
            }

        }

        private string FindName(string[] lines, ref int line, string textBeforeName, int tokenNumber)
        {
            for (int l = line; l < lines.Count(); l++)
            {
                if (lines[l].Trim().StartsWith(textBeforeName))
                {
                    var values = lines[l].Trim().Split(" ");
                    line = l;
                    return values[tokenNumber];
                }
            }
            return string.Empty;
        }

        private string GetAssociatedComment(string[] lines, ref int line)
        {
            line--;
            while (lines[line].Trim().StartsWith("///"))
                line--;
            if (lines[++line].Trim().StartsWith("///"))
                return FindNextComment(lines, ref line);
            return string.Empty;
        }

        private ApiFieldInfo GetNextEnumApi(string[] lines, ref int line, string enumName)
        {
            while (lines[line].Trim().StartsWith("///") ||
                   lines[line].Trim().StartsWith("{") ||
                   lines[line].Trim() == string.Empty)
                line++;

             var name = lines[line].Trim().Trim(',');
             var comment = GetAssociatedComment(lines, ref line);
             line++;
             return new ApiFieldInfo(name, enumName, comment);
        }

        private ApiFieldInfo GetNextInterfaceApi(string[] lines, ref int line)
        {
            while (lines[line].Trim().StartsWith("///") ||
                   lines[line].Trim().StartsWith("{") ||
                   lines[line].Trim() == string.Empty)
                line++;

            var values = lines[line].Trim().Split(" ");
            var name = values[2];
            var type = values[1];
            var comment = GetAssociatedComment(lines, ref line);
            line++;
            return new ApiFieldInfo(name, type, comment);
        }

        private void SkipBlankLines(string[] lines, ref int line)
        {
            while (lines[line].Trim() == string.Empty)
                line++;
        }

        private void CreateComponentDoco(string testComponentsFolder, string componentsFolder)
        {
            var directoryInfo = new DirectoryInfo(testComponentsFolder);
            foreach (var entry in GetValidFolders(testComponentsFolder))
            {
                string testComponentFolder = entry.FullName;
                string componentName = entry.Name;

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

                WriteApiPageFile(testComponentFolder, componentName);
            }
        }

        private List<DirectoryInfo> GetValidFolders(string testComponentsFolder)
        {
            List<DirectoryInfo> folders = new();

            var directoryInfo = new DirectoryInfo(testComponentsFolder);
            foreach (var entry in directoryInfo.GetDirectories("*", SearchOption.AllDirectories))
            {
                if (entry.Name == "Code" || entry.Name == "Doco" || entry.Name == "Examples" || entry.Name == "Logs")
                    continue;

                var dirs = entry.GetDirectories("*", SearchOption.TopDirectoryOnly).Select(d => d.Name).ToList();
                dirs.Remove("Code");
                dirs.Remove("Doco");
                dirs.Remove("Examples");

                if (dirs.Count > 0)
                    continue;

                folders.Add(entry);
            }
            return folders;
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

        private IComponentDocsInfo GetComponentDoco(string componentName, string fileName, bool hasExamples)
        {
            IComponentDocsInfo info = new ComponentDocsInfo();
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
                        typ.Replace("<", "&lt");
                        typ.Replace(">", "&gt");
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

        private void WriteDocsInfoFile(string testComponentFolder, IComponentDocsInfo docInfo)
        {
            var folder = Path.Combine(testComponentFolder, Paths.DocsFolder);
            var docFileName = folder + @$"\{docInfo.Name}DocsInfo.cs";
            Directory.CreateDirectory(folder);

            var lines = new List<string>();
            lines.Add("/// This file is auto-generated. Do not change manually");
            lines.Add("");
            lines.Add("using ClearBlazor.Common;");
            lines.Add("namespace ClearBlazorTest");
            lines.Add("{");
            lines.Add($"    public record {docInfo.Name}DocsInfo:IComponentDocsInfo");
            lines.Add("    {");
            lines.Add("        public string Name { get; set; } = " + $"\"{docInfo.Name}\";");
            lines.Add("        public string Description {get; set; } = " + $"\"{docInfo.Description}\";");
            lines.Add("        public (string, string) ApiLink  {get; set; } =  (\"{docInfo.ApiLink.Item1}\", \"{docInfo.ApiLink.Item2}\");");
            lines.Add("        public (string, string) ExamplesLink {get; set; } = " + $"(\"{docInfo.ExamplesLink.Item1}\", \"{docInfo.ExamplesLink.Item2}\");");
            lines.Add("        public (string, string) InheritsLink {get; set; } = " + $"(\"{docInfo.InheritsLink.Item1}\", \"{docInfo.InheritsLink.Item2}\");");

            lines.Add("        public List<(string, string)> ImplementsLinks {get; set; } = new()");
            lines.Add("        {");
            foreach (var link in docInfo.ImplementsLinks)
                lines.Add($"            (\"{link.Item1}\", \"{link.Item2}\"),");
            lines.Add("        };");

            lines.Add("        public List<ApiComponentInfo> ParameterApi {get; set; } = " + $"new List<ApiComponentInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.ParameterApi)
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description}\"),");
            lines.Add("        };");

            lines.Add("        public List<ApiComponentInfo> MethodApi {get; set; } = " + $" new List<ApiComponentInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.MethodApi)
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description}\"),");
            lines.Add("        };");

            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(docFileName, lines);
        }

        private void WriteOtherDocsInfoFile(string testFolder, IOtherDocsInfo docInfo)
        {
            var docFileName = testFolder + @$"\{docInfo.Name}DocsInfo.cs";
            Directory.CreateDirectory(testFolder);

            var lines = new List<string>();
            lines.Add("/// This file is auto-generated. Do not change manually");
            lines.Add("");
            lines.Add("using ClearBlazor.Common;");
            lines.Add("namespace ClearBlazorTest");
            lines.Add("{");
            lines.Add($"    public record {docInfo.Name}DocsInfo:IOtherDocsInfo");
            lines.Add("    {");
            lines.Add("        public string Name { get; set; } = " + $"\"{docInfo.Name}\";");
            lines.Add("        public string Description {get; set; } = " + $"\"{docInfo.Description}\";");
            lines.Add("        public List<ApiFieldInfo> FieldApi {get; set; } = " + $"new List<ApiFieldInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.FieldApi)
                lines.Add($"            new ApiFieldInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Description}\"),");
            lines.Add("        };");
            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(docFileName, lines);
        }

        private void WriteApiPageFile(string testFolder, string componentName)
        {
            Directory.CreateDirectory(testFolder);

            var docFileName = testFolder + @$"\{componentName}ApiPage.razor";

            var lines = new List<string>();
            lines.Add("/// This file is auto-generated. Do not change manually");
            lines.Add("");
            lines.Add($"@page \"/{componentName}Api\"");
            lines.Add("@using ClearBlazorTest");
            lines.Add($"<DocsApiPage DocsInfo=@(new {componentName}DocsInfo())/>");

            File.WriteAllLines(docFileName, lines);
        }
    }
}
