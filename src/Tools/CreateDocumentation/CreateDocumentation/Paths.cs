using System.IO;
using System.Linq;

namespace CreateDocumentation
{
    public class Paths
    {
        public const string SolutionFolder = "ClearBlazor";
        public const string TestComponentsFolder = @"src\ClearBlazorTestCore\Pages\Components";
        public const string TestEnumsFolder = @"src\ClearBlazorTestCore\Pages\Enums";
        public const string TestInterfacesFolder = @"src\ClearBlazorTestCore\Pages\Interfaces";
        public const string ComponentsFolder = @"src\ClearBlazor\Components";
        public const string InterfacesFolder = @"src\ClearBlazor\Components\Interfaces";
        public const string EnumerationsFolder = "src\\ClearBlazor.Components.Enumerations";
        public const string TestCoreProjectFile = @"src\ClearBlazorTestCore\ClearBlazorTestCore.csproj";
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
