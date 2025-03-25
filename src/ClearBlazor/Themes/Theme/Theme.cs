namespace ClearBlazor
{
    public class Theme
    {
        public string ThemeName { get; set; }

        /// <summary>
        /// Gets or sets the light color scheme.
        /// </summary>
        public IColorScheme LightColorScheme { get; set; }

        /// <summary>
        /// Gets or sets the dark color scheme.
        /// </summary>
        public IColorScheme DarkColorScheme { get; set; }

        public Typography Typography { get; set; }

        public int DefaultCornerRadius { get; set; } = 8;

        public virtual int ToolTipDelay { get; set; } = 30; // Milliseconds

        public virtual ScrollBarStyle ScrollBarStyle { get; set; } = ScrollBarStyle.ThinWidthRound;

        // Used by new ScrollViewer
        public virtual int ScrollbarWidth { get; set; } = 10;
        public virtual int ScrollbarBackgroundBoxShadowWidth { get; set; } = 5;
        public virtual int ScrollbarCornerRadius { get; set; } = 5;
        public virtual int ScrollbarThumbCornerRadius { get; set; } = 5;

        //
        public virtual int TextEntryDebounceInterval { get; set; } = 100;


        public Theme(string themeName)
        {
            LightColorScheme = new LightColorScheme();
            DarkColorScheme = new DarkColorScheme();
            Typography = new Typography();
            ThemeName = themeName;
        }

        public (int width, int height, int borderRadius, int thumbBorderWidth) GetScrollBarProperties()
        {
            int width = 16;
            int height = 16;
            int borderRadius = 0;
            int thumbBorder = 0;

            switch (ScrollBarStyle)
            {
                case ScrollBarStyle.NormalWidthSquare:
                    width = 16;
                    height = 16;
                    borderRadius = 0;
                    break;
                case ScrollBarStyle.ThinWidthSquare:
                    width = 10;
                    height = 10;
                    borderRadius = 0;
                    break;
                case ScrollBarStyle.NormalWidthRound:
                    width = 16;
                    height = 16;
                    borderRadius = 8;
                    break;
                case ScrollBarStyle.ThinWidthRound:
                    width = 10;
                    height = 10;
                    borderRadius = 5;
                    break;
            }

            return (width, height, borderRadius, thumbBorder);
        }

    }
}
