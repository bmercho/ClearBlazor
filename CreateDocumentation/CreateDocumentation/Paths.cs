using System.IO;
using System.Linq;

namespace CreateDocumentation
{
    public class Paths
    {
        public const string SolutionFolder = "ClearBlazor";
        public const string TestComponentsFolder = @"ClearBlazorTest\ClearBlazorTestCore\Pages\Components";
        public const string TestEnumsFolder = @"ClearBlazorTest\ClearBlazorTestCore\Pages\Enums";
        public const string TestInterfacesFolder = @"ClearBlazorTest\ClearBlazorTestCore\Pages\Interfaces";
        public const string ComponentsFolder = @"ClearBlazorTest\ClearBlazor\Components";
        public const string InterfacesFolder = @"ClearBlazorTest\ClearBlazor\Components\Interfaces";
        public const string EnumerationsFolder = "ClearBlazorTest\\ClearBlazor.Components.Enumerations";
        public const string TestCoreProjectFile = @"ClearBlazorTest\ClearBlazorTestCore\ClearBlazorTestCore.csproj";
        public const string DocsFolder = "Doco";
        public const string ExampleDiscriminator = "Example.razor"; // example components must contain this string

        public static string? SrcPath
        {
            get
            {
                var workingPath = Directory.GetCurrentDirectory();
                do
                {
                    workingPath = Path.GetDirectoryName(workingPath);
                }
                while (Path.GetFileName(workingPath) != SolutionFolder && !string.IsNullOrWhiteSpace(workingPath));

                return workingPath;
            }
        }
    }
}
