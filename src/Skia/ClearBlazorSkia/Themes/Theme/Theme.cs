namespace ClearBlazor
{
    public class Theme
    {
        public string ThemeName { get; set; }

        /// <summary>
        /// Gets or sets the palette for the light theme.
        /// </summary>
        public Palette PaletteLight { get; set; }

        /// <summary>
        /// Gets or sets the palette for the dark theme.
        /// </summary>
        public Palette PaletteDark { get; set; }

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

        ///// <summary>
        ///// Gets or sets the shadow settings.
        ///// </summary>
        //public Shadow Shadows { get; set; }

        ///// <summary>
        ///// Gets or sets the typography settings.
        ///// </summary>
        //public Typography Typography { get; set; }

        ///// <summary>
        ///// Gets or sets the layout properties.
        ///// </summary>
        //public LayoutProperties LayoutProperties { get; set; }

        ///// <summary>
        ///// Gets or sets the z-index values.
        ///// </summary>
        //public ZIndex ZIndex { get; set; }

        ///// <summary>
        ///// Gets or sets the pseudo CSS styles.
        ///// </summary>
        //public PseudoCss PseudoCss { get; set; }

        public Theme(string themeName)
        {
            PaletteLight = new PaletteLight();
            PaletteDark = new PaletteDark();
            Typography = new Typography();
            ThemeName = themeName;

        //Shadows = new Shadow();
        //Typography = new Typography();
        //LayoutProperties = new LayoutProperties();
        //ZIndex = new ZIndex();
        //PseudoCss = new PseudoCss();
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
