﻿using System.IO;
using System.Linq;

namespace CreateDocumentation
{
    public class Paths
    {
        public const string SolutionFolder = "ClearBlazor";
        public const string TestComponentsFolder = @"ClearBlazorTest\ClearBlazorTestCore\Pages\Components";
        public const string ComponentsFolder = @"ClearBlazorTest\ClearBlazor\Components";
        public const string InterfacesFolder = @"ClearBlazorTest\ClearBlazor\Components\Interfaces";
        public const string EnumerationsFolder = "ClearBlazorTest\\ClearBlazor.Components.Enumerations";
        public const string DocsFolder = "Doco";

        private const string DocsDirectory = "ClearBlazorTest";
        private const string TestDirectory = "MudBlazor.UnitTests";
        private const string SnippetsFile = "Snippets.generated.cs";
        private const string DocStringsFile = "DocStrings.generated.cs";
        private const string ComponentTestsFile = "ExampleDocsTests.generated.cs";
        private const string ApiPageTestsFile = "ApiDocsTests.generated.cs";
        private const string NewFilesToBuild = "NewFilesToBuild.txt";

        public const string ExampleDiscriminator = "Example"; // example components must contain this string

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


        public string DocsDirPath
        {
            get
            {
                return Directory.EnumerateDirectories(SrcPath, DocsDirectory).FirstOrDefault();
            }
        }

        public string TestDirPath
        {
            get
            {
                return Path.Join(Directory.EnumerateDirectories(SrcPath, TestDirectory).FirstOrDefault(), "Generated");
            }
        }

        public string DocsStringSnippetsDirPath
        {
            get
            {
                return Path.Join(DocsDirPath, "Models");
            }
        }

        public string DocStringsFilePath
        {
            get
            {
                return Path.Join(DocsStringSnippetsDirPath, DocStringsFile);
            }
        }

        public string SnippetsFilePath
        {
            get
            {
                return Path.Join(DocsStringSnippetsDirPath, SnippetsFile);
            }
        }

        public string ComponentTestsFilePath
        {
            get
            {
                return Path.Join(TestDirPath, ComponentTestsFile);
            }
        }

        public string ApiPageTestsFilePath
        {
            get
            {
                return Path.Join(TestDirPath, ApiPageTestsFile);
            }
        }

        public string NewFilesToBuildPath
        {
            get
            {
                return Path.Join(DocsDirPath, NewFilesToBuild);
            }
        }
    }
}
