using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;

namespace ConvertMaterialIcons
{
    public static class ConvertIcons
    {
        private static List<TextWriter?> _iconFiles = new List<TextWriter?>();
        private static List<string> _families = new List<string>
        {
            "materialicons",
            "materialiconsoutlined",
            "materialiconsround",
            "materialiconssharp",
            "materialiconstwotone"
        };

        public static async Task Convert(string destination)
        {
            var iconsPath = Path.Combine(destination, "Icons");
            if (Directory.Exists(iconsPath)) 
                Directory.Delete(iconsPath, true);

            var materialPath = Path.Combine(iconsPath, "Material");
            Directory.CreateDirectory(materialPath);

            _iconFiles.Add(CreateIconFile(materialPath, "Filled"));
            _iconFiles.Add(CreateIconFile(materialPath, "Outlined"));
            _iconFiles.Add(CreateIconFile(materialPath, "Rounded"));
            _iconFiles.Add(CreateIconFile(materialPath, "Sharp"));
            _iconFiles.Add(CreateIconFile(materialPath, "TwoTone"));


            var metaData = await GetIconsMetaData();
            if (metaData == null)
                return;

            foreach(var icon in metaData.icons)
            {
                int i = 0;
                foreach (var family in _families)
                {
                    var doc = await GetIconData(family, (string)icon.name, (int)icon.version);
                    string name = icon.name;
                    name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Replace("_", "");
                    if (Char.IsDigit(name[0]))
                        name = "_" + name;

                    if (doc == null)
                        continue;
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(doc);
                    var svg = xmlDoc.InnerXml;
                    svg = svg.Replace(" xmlns=\"http://www.w3.org/2000/svg\"", "");
                    svg = svg.Replace(" />", "/>");
                    svg = svg.Replace("\"", "\\\"");
                    var index = svg.IndexOf(">");
                    svg = svg.Substring(index+1);
                    svg = svg.Substring(0, svg.Length-6);

                    WriteMaterialIcon(name,svg, _iconFiles[i]); 

                    i++;
                }

            }

            for(int i=0;i<_iconFiles.Count;i++)
                CloseIconFile(_iconFiles[i]);
        }

        private static async Task<dynamic?> GetIconsMetaData()
        {
            string page = "https://fonts.google.com/metadata/icons";

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();

                if (result != null)
                {
                    result = result.Substring(5);
                    return JsonConvert.DeserializeObject<dynamic>(result);
                }
            }

            return null;

        }

        private static async Task<string?> GetIconData(string family, string iconName, int version)
        {
            string page = $"https://fonts.gstatic.com/s/i/{family}/{iconName}/v{version}/24px.svg";

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();

                if (result != null)
                {
                    return result;
                }
            }

            return null;

        }

        private static TextWriter? CreateIconFile(string path, string type)
        {
            TextWriter file =  File.CreateText(Path.Combine(path, $"{type}.cs"));

            file.WriteLine("namespace ClearBlazor");
            file.WriteLine("{");
            file.WriteLine("    public partial class Icons");
            file.WriteLine("    {");
            file.WriteLine("        public partial class Material");
            file.WriteLine("        {");
            file.WriteLine("            public partial class {type}");
            file.WriteLine("            {");

            return file;
        }

        private static void CloseIconFile(TextWriter? iconFile)
        {
            if (iconFile == null)
                return;

            iconFile.WriteLine("            }");
            iconFile.WriteLine("        }");
            iconFile.WriteLine("    }");
            iconFile.WriteLine("}");

            iconFile.Close();
        }

        private static void WriteMaterialIcon(string name, string svg, TextWriter? iconFile)
        {
            iconFile?.WriteLine($"                public const string {name} = \"{svg}\";");
        }
    }
}
