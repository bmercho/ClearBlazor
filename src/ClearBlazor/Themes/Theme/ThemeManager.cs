using Microsoft.JSInterop;

namespace ClearBlazor
{
    public class ThemeManager
    {
        List<Theme> _themes = new List<Theme>();
        private static bool _isDarkMode = false;

        public static Theme CurrentTheme { get; set; } = null!;

        public static IColorScheme CurrentColorScheme { get; set; } = null!;

        public static RootComponent? RootComponent { get; set; }

        public static bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (IsDarkMode != value)
                {

                    IsDarkMode = _isDarkMode = value;
                    SetColorScheme();
                    RootComponent?.ThemeChanged();
                }
            }
        }

        public ThemeManager(RootComponent? rootComponent, bool useDarkMode)
        {
            RootComponent = rootComponent;
            IsDarkMode = useDarkMode;

            CurrentTheme = new Theme("DefaultTheme");

            SetColorScheme();

            AddTheme(CurrentTheme);
        }

        private static void SetColorScheme()
        {
            if (IsDarkMode)
                CurrentColorScheme = CurrentTheme.DarkColorScheme;
            else
                CurrentColorScheme = CurrentTheme.LightColorScheme;
            Color.SetColors();
        }

        public bool AddTheme(Theme theme)
        {
            if (_themes.Select(t => t.ThemeName).Contains(theme.ThemeName))
                return false;
            _themes.Add(theme);
            return true;
        }

        public Theme? GetTheme(string themeName)
        {
            return _themes.FirstOrDefault(t => t.ThemeName == themeName);
        }

        internal async Task UpdateTheme(IJSRuntime jsRuntime)
        {
                string width = "16px";
                string height = "16px";
                string borderRadius = "0px";
                string thumbBorder = "0px";

                switch (CurrentTheme.ScrollBarStyle)
                {
                    case ScrollBarStyle.NormalWidthSquare:
                        width = "16px";
                        height = "16px";
                        borderRadius = "0px";
                        break;
                    case ScrollBarStyle.ThinWidthSquare:
                        width = "10px";
                        height = "10px";
                        borderRadius = "0px";
                        break;
                    case ScrollBarStyle.NormalWidthRound:
                        width = "16px";
                        height = "16px";
                        borderRadius = "8px";
                        break;
                    case ScrollBarStyle.ThinWidthRound:
                        width = "10px";
                        height = "10px";
                        borderRadius = "5px";
                        break;
                }

            await jsRuntime.InvokeVoidAsync("window.scrollbar.setScrollBarProperties", width, height, borderRadius,
                                                       ThemeManager.CurrentColorScheme.SurfaceContainer.Value,
                                                       ThemeManager.CurrentColorScheme.Outline.Value,
                                                       ThemeManager.CurrentColorScheme.SurfaceContainerHighest.Value,
                                                       thumbBorder);
        }
    }
}
