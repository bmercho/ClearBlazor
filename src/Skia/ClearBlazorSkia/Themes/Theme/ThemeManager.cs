using Microsoft.JSInterop;

namespace ClearBlazor
{
    public class ThemeManager
    {
        List<Theme> _themes = new List<Theme>();
        private static bool _isDarkMode = false;

        public static Theme CurrentTheme { get; set; } = null!;

        public static Palette CurrentPalette { get; set; } = null!;

        public static RootComponent RootComponent { get; set; } = null!;

        //private IJSObjectReference? _scrollbarModule = null;
        public static bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (IsDarkMode != value)
                {
                    IsDarkMode = _isDarkMode = value;
                    if (IsDarkMode)
                        CurrentPalette = CurrentTheme.PaletteDark;
                    else
                        CurrentPalette = CurrentTheme.PaletteLight;

                    Color.SetColors();
                    RootComponent.Refresh();
                }
            }
        }

        public ThemeManager(RootComponent rootComponent, bool useDarkMode)
        {
            RootComponent = rootComponent;
            IsDarkMode = useDarkMode;

            CurrentTheme = new Theme("DefaultTheme");// { PaletteDark = new PaletteDark(), PaletteLight = new PaletteLight() };

            if (IsDarkMode)
                CurrentPalette = CurrentTheme.PaletteDark;
            else
                CurrentPalette = CurrentTheme.PaletteLight;

            Color.SetColors();

            AddTheme(CurrentTheme);
        }

        public void AddTheme(Theme theme)
        {
            _themes.Add(theme);
        }

        public async Task UpdateTheme(IJSRuntime jsRuntime)
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
                                                       CurrentPalette.ScrollbarBackgroundColor.Value,
                                                       CurrentPalette.ScrollbarThumbColor.Value,
                                                       CurrentPalette.ScrollbarThumbColor.Darken(0.1).Value,
                                                       thumbBorder);
        }
    }
}
