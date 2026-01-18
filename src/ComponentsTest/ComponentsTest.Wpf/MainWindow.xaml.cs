using ComponentsTest.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ComponentsTest.Wpf
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        const int FirstBorderTest = 0;
        const int FirstGridTest = 20;
        const int FirstStackPanelTest = 40;
        const int FirstWrapPanelTest = 200;
        const int FirstDockPanelTest = 220;
        const int FirstUniformGridTest = 240;
        const int FirstTabsTest = 60;
        const int FirstScrollViewerTest = 80;
        const int FirstTextBlockTest = 100;
        const int FirstImageTest = 120;
        const int FirstMaterialIconTest = 140;
        const int FirstButtonTest = 160;
        const int FirstTextBoxTest = 180;

        private int _firstTestNum;
        private int _lastTestNum;
        private int _testNum;
        private string _testNumText;
        private string _testName;
        private string _testInfo;
        private string _blazorTestInfo;
        private string _wpfTestInfo;

        private List<TestInfo> _testInfos = new List<TestInfo>
        {
            //new TestInfo {TestNumber = 0,Info="Info0"},
            //new TestInfo {TestNumber = 1,Info="Info1"},
            //new TestInfo {TestNumber = 5,Info="Info5"},
        };
        public string TestNumText
        {
            get => _testNumText;
            set
            {
                if (_testNumText != value)
                {
                    _testNumText = value;
                    OnPropertyChanged(nameof(TestNumText));
                }
            }
        }

        public string TestName
        {
            get => _testName;
            set
            {
                if (_testName != value)
                {
                    _testName = value;
                    OnPropertyChanged(nameof(TestName));
                }
            }
        }

        public string TestInfo
        {
            get => _testInfo;
            set
            {
                if (_testInfo != value)
                {
                    _testInfo = value;
                    OnPropertyChanged(nameof(TestInfo));
                }
            }
        }

        public string BlazorTestInfo
        {
            get => _blazorTestInfo;
            set
            {
                if (_blazorTestInfo != value)
                {
                    _blazorTestInfo = value;
                    OnPropertyChanged(nameof(BlazorTestInfo));
                }
            }
        }

        public string WpfTestInfo
        {
            get => _wpfTestInfo;
            set
            {
                if (_wpfTestInfo != value)
                {
                    _wpfTestInfo = value;
                    OnPropertyChanged(nameof(WpfTestInfo));
                }
            }
        }

        public bool NextTestAvailable => _testNum < _lastTestNum;

        public bool PrevTestAvailable => _testNum > _firstTestNum;

        private Tests Tests = new Tests();

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindow()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            Resources.Add("services", serviceCollection.BuildServiceProvider());
            DataContext = this;
            InitializeComponent();
           // UniformGridClick(null, null);
        }

        private bool ShowTest(int testNum)
        {
            var testInfo = GetTestInfo(testNum);

            if (testInfo == null)
            {
                TestInfo = string.Empty;
            }
            else
                TestInfo = testInfo.Info;

            Type? wpfType = Tests.GetWpfType(testNum);

            Type? blazorType = Tests.GetBlazorType(testNum);
            if (blazorType != null && wpfType != null)
            {
                TestNumText = GetTestNumText();
                OnPropertyChanged(nameof(NextTestAvailable));
                OnPropertyChanged(nameof(PrevTestAvailable));
                MainView.SetTest(blazorType);
                ContentControl.Content = Activator.CreateInstance(wpfType);
                return true;
            }
            return false;
        }

        private TestInfo? GetTestInfo(int testNum)
        {
            return _testInfos.FirstOrDefault(t => t.TestNumber == testNum);
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            if (ShowTest(_testNum + 1))
            {
                _testNum++;
                ShowTest(_testNum);
            }
        }
        private void PrevClick(object sender, RoutedEventArgs e)
        {
            if (_testNum > 0)
            {
                if (ShowTest(_testNum - 1))
                {
                    _testNum--;
                    ShowTest(_testNum);
                }
            }
        }

        public void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private void BorderClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("Border tests:", FirstBorderTest);
        }

        private void GridClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("Grid tests:", FirstGridTest);
        }

        private void StackPanelClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("StackPanel tests:", FirstStackPanelTest);
        }

        private void WrapPanelClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("WrapPanel tests:", FirstWrapPanelTest);
        }

        private void DockPanelClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("DockPanel tests:", FirstDockPanelTest);
        }

        private void UniformGridClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("UniformGrid tests:", FirstUniformGridTest);
        }

        private void TabsClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("StackPanel tests:", FirstTabsTest);
        }

        private void ScrollViewerClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("ScrollViewer tests:", FirstScrollViewerTest);
        }

        private void TextBlockClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("TextBlock tests:", FirstTextBlockTest);
        }

        private void ImageClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("Image tests:", FirstImageTest);
        }

        private void MaterialIconClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("MaterialIcon tests:", FirstMaterialIconTest);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("Button tests:", FirstButtonTest);
        }

        private void TextBoxClick(object sender, RoutedEventArgs e)
        {
            SetTestGroup("TextBox tests:", FirstTextBoxTest);
        }

        private void SetTestGroup(string testName, int firstTestNumber)
        {
            _firstTestNum = firstTestNumber;
            _lastTestNum = GetLastTestNum();
            _testNum = _firstTestNum;
            TestName = testName;
            TestNumText = GetTestNumText();
            ShowTest(_testNum);
            OnPropertyChanged(nameof(NextTestAvailable));
            OnPropertyChanged(nameof(PrevTestAvailable));
        }

        private string GetTestNumText()
        {
            int current = _testNum - _firstTestNum + 1;
            int last = _lastTestNum - _firstTestNum + 1;
            return $"({current} of {last})";
        }

        private int GetLastTestNum()
        {
            for (int i = _firstTestNum; i < _firstTestNum + 99; i++)
            {
                Type? wpfType = Tests.GetWpfType(i);

                Type? blazorType = Tests.GetBlazorType(i);
                if (blazorType == null || wpfType == null)
                    return i - 1;
            }
            return 0;
        }
    }

    public class TestInfo
    {
        public int TestNumber { get; set; }
        public string Info { get; set; } = string.Empty;
    }
}
