using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace ClearBlazorSkia.Tests
{
    public partial class TestHost : ComponentBase
    {
        private Type? _testType { get; set; } = null;
        private int _testIndex = 1;
        private TestInfo? _currentTest = null;
        private bool _auto = true;
        private string TestName { get; set; }

        private Dictionary<string, object> _params = new Dictionary<string, object>();
        private SortedList<int, TestInfo> _tests = new SortedList<int, TestInfo>();

        protected override void OnInitialized()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (assemblies == null)
                return;

            Assembly? assembly = assemblies.FirstOrDefault(a => a.FullName.StartsWith("ClearBlazorSkia.Tests"));

            if (assembly == null)
                return;

            var types = assembly.GetTypes().Where(t => t.Name.StartsWith("Test") &&
                                                          !t.Name.Contains("Host") &&
                                                          !t.Name.Contains("TestBase") &&
                                                          !t.Name.Contains("TestInfo")).ToList();

            foreach (var type in types)
            {
                var numString = type.Name.Substring(type.Name.Substring(0, type.Name.LastIndexOf("_")).LastIndexOf("_") + 1);

                var groupNumber = int.Parse(numString.Split("_")[0]);
                var groupTestNumber = int.Parse(numString.Split("_")[1]);

                int testNumber = groupNumber * 1000 + groupTestNumber;
                _tests.Add(testNumber, new TestInfo()
                {
                    GroupNumber = groupNumber,
                    GroupTestNumber = groupTestNumber,
                    TestName = type.Name,
                    TestType = type
                });
            }

            int group = 1;
            int num = 1;

            int testNum = group * 1000 + num;
            _testIndex = _tests.IndexOfKey(testNum);
            _currentTest = GetTest(_testIndex);
            if (_currentTest == null)
                _testType = null;
            else
                _testType = _currentTest.TestType;

            _params.Add("TestComplete", EventCallback.Factory.Create<bool>(this, TestComplete));

            base.OnInitialized();
        }

        public TestInfo? GetTest(int testIndex)
        {
            if (_tests.Values.Count <= testIndex)
            {
                TestName = "";
                return null;
            }
            TestInfo info = _tests.Values.ElementAt(testIndex);
            TestName = info.TestName;
            return info;
        }

        private async Task TestComplete(bool passed)
        {
            if (_currentTest != null)
            {
                _currentTest.TestState = passed;
                Console.WriteLine($"{_currentTest.TestName}: {(passed ? "Passed" : "Failed")}");
            }
            await Task.Delay(250);
            _testIndex++;
            if (!_auto)
                return;
            _currentTest = GetTest(_testIndex);
            if (_currentTest == null)
                return;
            else
                _testType = _currentTest.TestType;
            StateHasChanged();
        }

        private void OnClick()
        {
            _currentTest = GetTest(_testIndex);
            if (_currentTest == null)
                return;
            else
                _testType = _currentTest.TestType;
            StateHasChanged();
        }
    }

    public class TestInfo
    {
        public int GroupNumber { get; set; }
        public int GroupTestNumber { get; set; }
        public string TestName { get; set; } = string.Empty;
        public Type? TestType { get; set; } = null;
        public bool? TestState { get; set; } = null;
    }
}
