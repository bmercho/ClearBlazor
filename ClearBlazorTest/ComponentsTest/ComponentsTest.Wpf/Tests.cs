using ComponentsTest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComponentsTest.Wpf
{
    public class Tests
    {
        public static List<Type> _wpfTypes;
        public static List<Type> _blazorTypes;


        static Tests()
        {
            var t = typeof(MainView); // Needed to load assembly

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            _wpfTypes = assemblies.FirstOrDefault(a => a.FullName.StartsWith("ComponentsTest.Wpf")).GetTypes()
                                                           .Where(t => t.FullName.StartsWith("ComponentsTest.Wpf.Pages.Test")).ToList();
            _blazorTypes = assemblies.FirstOrDefault(a => a.FullName.StartsWith("ComponentsTest.Shared")).GetTypes()
                                                           .Where(t => t.FullName.StartsWith("ComponentsTest.Shared.Tests.Test") && t.Name.StartsWith("Test")).ToList();
        }

        public Type? GetWpfType(int testNum)
        {
            return _wpfTypes.FirstOrDefault(t => t.Name == $"Test{testNum:D3}");

        }
        public Type? GetBlazorType(int testNum)
        {
            return _blazorTypes.FirstOrDefault(t => t.Name == $"Test{testNum:D3}");
        }
    }
}
