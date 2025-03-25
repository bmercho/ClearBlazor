using ClearBlazor;
using ClearBlazor.Common;

namespace CreateDocumentation
{
    public class ApiDoco
    {
        private List<string> _enumerations = new();


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
                info.Description = GetAssociatedComment(lines, line);
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

                _enumerations.Add(info.Name);

                WriteOtherDocsInfoFile(testEnumFolder, info.Name, info);

                WriteApiPageFile(testEnumFolder, info.Name, true, false);
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
                info.Description = GetAssociatedComment(lines, line);
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
                if (testEnumFolder.Contains("<"))
                    testEnumFolder = testEnumFolder.Substring(0, testEnumFolder.IndexOf("<"));

                var name = info.Name;
                if (name.Contains("<"))
                    name = name.Substring(0, name.IndexOf("<"));

                WriteOtherDocsInfoFile(testEnumFolder, name, info);

                WriteApiPageFile(testEnumFolder, name, false, true);
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
                    return values[tokenNumber].Trim().Split(":")[0];
                }
            }
            return string.Empty;
        }

        private string GetAssociatedComment(string[] lines, int line)
        {
            line--;
            SkipBlankLinesInReverse(lines, ref line);
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
             var comment = GetAssociatedComment(lines, line);
             line++;
             return new ApiFieldInfo(name, enumName, comment);
        }

        private ApiFieldInfo GetNextInterfaceApi(string[] lines, ref int line)
        {
            while (lines[line].Trim().StartsWith("///") ||
                   lines[line].Trim().StartsWith("{") ||
                   lines[line].Trim() == string.Empty)
                line++;

            if (lines[line].Trim().StartsWith("}"))
                return new ApiFieldInfo(string.Empty, string.Empty, string.Empty);

            var values = lines[line].Trim().Split(" ");
            var name = values[2];
            var type = values[1];
            var comment = GetAssociatedComment(lines, line);
            line++;
            return new ApiFieldInfo(name, type, comment);
        }

        private void SkipBlankLines(string[] lines, ref int line)
        {
            while (lines[line].Trim() == string.Empty)
                line++;
        }

        private void SkipBlankLinesInReverse(string[] lines, ref int line)
        {
            while (lines[line].Trim() == string.Empty)
                line--;
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
                if (docInfo != null)
                {
                    WriteDocsInfoFile(componentName, testComponentFolder, docInfo);

                    WriteApiPageFile(Path.Combine(testComponentFolder, Paths.DocsFolder), componentName, false, false);
                }
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

        private IComponentDocsInfo? GetComponentDoco(string componentName, string fileName, bool hasExamples)
        {
            IComponentDocsInfo info = new ComponentDocsInfo();
            var lines = File.ReadAllLines(fileName);
            int line = 0;
            info.Name = FindName(lines, ref line, "public class", 2);
            if (info.Name == string.Empty)
            {
                info.Name = FindName(lines, ref line, "public abstract class", 3);
                if (info.Name == string.Empty)
                {
                    info.Name = FindName(lines, ref line, "public partial class", 3);
                    if (info.Name == string.Empty)
                    {
                        Console.WriteLine("Component class name not found");
                        return null;
                    }
                }
            }
            info.Description = GetAssociatedComment(lines, line);
            info.ApiLink = ("API", $"{componentName}Api");
            if (hasExamples)
                info.ExamplesLink = ("Examples", $"{componentName}");

            (string inherits, List<string> implements) = ExtractInheritsAndImplements(componentName, lines, ref line);
            if (inherits != null)
                info.InheritsLink = (inherits, $"{inherits}Api");
            if (implements.Count() > 0)
                foreach (var link in implements)
                    info.ImplementsLinks.Add((link, $"{link}Api"));

            line++;
            while (true)
            {
                bool found = GetNextComponentApi(info, lines, ref line);
                if (!found)
                    break;
            }

            return info;
        }

        private (string inherits, List<string> implements) ExtractInheritsAndImplements(string componentName, string[] lines, ref int line)
        {
            string inherits = string.Empty;
            List<string> implements = new();
            while (line < lines.Count())
            {
                int index = lines[line].IndexOf($"class {componentName}");
                if (index > 0)
                {
                    var values = lines[line].Split(':');

                    if (values.Length > 1)
                        values = values[1].Trim().Split(',');
                        if (!values[0].StartsWith("I") || !Char.IsUpper(values[0][1]))
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

        private bool GetNextComponentApi(IComponentDocsInfo info, string[] lines, ref int line)
        { 
            while (line < lines.Count())
            {
                var trimmedLine = lines[line].Trim();   
                if (trimmedLine.StartsWith("public"))
                {
                    if (!lines[line].Contains("=") &&
                        lines[line].Trim().EndsWith(")"))
                    {
                        var values = lines[line].Trim().Split(" ");
                        if (values[1].Split('(').First() == info.Name || values[1] == "override" || values[1] == "virtual")
                        {
                            line++;
                            continue;
                        }
                        int startIndex = 1;
                        if (values[1] == "static")
                            startIndex++;

                        var index1 = lines[line].Trim().IndexOf(values[startIndex+1]);
                        var index2 = lines[line].Trim().IndexOf(')', index1);
                        var methodSignature = lines[line].Trim().Substring(index1, index2 - index1+1);

                        var typ = values[startIndex];
                        typ.Replace("<", "&lt");
                        typ.Replace(">", "&gt");

                        if (_enumerations.Contains(typ.Trim('?')))
                        {
                            typ = $"<a href={typ.Trim('?')}Api>{typ}</a>";
                        }

                        var comment = GetAssociatedComment(lines, line);
                        line++;
                        info.MethodApi.Add(new ApiComponentInfo(methodSignature, typ, "", comment));
                    }
                }
                else if (trimmedLine.StartsWith("[Parameter]"))
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

                    if (_enumerations.Contains(typ.Trim('?')))
                    {
                        typ = $"<a href={ typ.Trim('?')}Api>{typ}</a>";
                    }
                    line--;
                    var comment = GetAssociatedComment(lines, line);
                    line++;
                    if (values.Length > 2)
                        info.ParameterApi.Add(new ApiComponentInfo(values[2], typ, def, comment));
                    return true;
                }
                line++;
            }

            return false;
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

        private void WriteDocsInfoFile(string componentName, string testComponentFolder, IComponentDocsInfo docInfo)
        {
            var folder = Path.Combine(testComponentFolder, Paths.DocsFolder);
            var docFileName = folder + @$"\{componentName}DocsInfo.cs";
            Directory.CreateDirectory(folder);

            var lines = new List<string>();
            lines.Add("/// This file is auto-generated. Do not change manually");
            lines.Add("");
            lines.Add("using ClearBlazor.Common;");
            lines.Add("namespace ClearBlazorTest");
            lines.Add("{");
            lines.Add($"    public record {componentName}DocsInfo:IComponentDocsInfo");
            lines.Add("    {");
            lines.Add("        public string Name { get; set; } = " + $"\"{docInfo.Name}\";");
            lines.Add("        public string Description {get; set; } = " + $"\"{docInfo.Description.Replace("\"", "\\\"")}\";");
            lines.Add("        public (string, string) ApiLink  {get; set; } = " + $"(\"{docInfo.ApiLink.Item1}\", \"{docInfo.ApiLink.Item2}\");");
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
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description.Replace("\"", "\\\"")}\"),");
            lines.Add("        };");

            lines.Add("        public List<ApiComponentInfo> MethodApi {get; set; } = " + $" new List<ApiComponentInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.MethodApi)
                lines.Add($"            new ApiComponentInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Default}\", \"{info.Description.Replace("\"", "\\\"")}\"),");
            lines.Add("        };");

            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(docFileName, lines);
        }

        private void WriteOtherDocsInfoFile(string testFolder, string name, IOtherDocsInfo docInfo)
        {
            var docFileName = testFolder + @$"\{name}DocsInfo.cs";
            Directory.CreateDirectory(testFolder);

            var lines = new List<string>();
            lines.Add("/// This file is auto-generated. Do not change manually");
            lines.Add("");
            lines.Add("using ClearBlazor.Common;");
            lines.Add("namespace ClearBlazorTest");
            lines.Add("{");
            lines.Add($"    public record {name}DocsInfo:IOtherDocsInfo");
            lines.Add("    {");
            lines.Add("        public string Name { get; set; } = " + $"\"{docInfo.Name}\";");
            lines.Add("        public string Description {get; set; } = " + $"\"{docInfo.Description.Replace("\"", "\\\"")}\";");
            lines.Add("        public List<ApiFieldInfo> FieldApi {get; set; } = " + $"new List<ApiFieldInfo>");
            lines.Add("        {");
            foreach (var info in docInfo.FieldApi)
                lines.Add($"            new ApiFieldInfo(\"{info.Name}\", \"{info.Type}\", \"{info.Description.Replace("\"", "\\\"")}\"),");
            lines.Add("        };");
            lines.Add("    }");
            lines.Add("}");

            File.WriteAllLines(docFileName, lines);
        }

        private void WriteApiPageFile(string testFolder, string componentName, bool isEnum, bool isInterface)
        {
            Directory.CreateDirectory(testFolder);

            var docFileName = testFolder + @$"\{componentName}ApiPage.razor";

            var lines = new List<string>();
            lines.Add("@* This file is auto-generated. Do not change manually *@");
            lines.Add("");
            lines.Add($"@page \"/{componentName}Api\"");
            lines.Add("@using ClearBlazorTest");
            if (isEnum)
                lines.Add($"<DocsEnumPage DocsInfo=@(new {componentName}DocsInfo())/>");
            else if (isInterface)
                lines.Add($"<DocsInterfacePage DocsInfo=@(new {componentName}DocsInfo())/>");
            else
                lines.Add($"<DocsApiPage DocsInfo=@(new {componentName}DocsInfo())/>");

            File.WriteAllLines(docFileName, lines);
        }
    }
}
