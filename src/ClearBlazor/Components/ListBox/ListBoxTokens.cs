namespace ClearBlazor
{
    public static class ListBoxTokens
    {
        public static Color ContainerColor => ThemeManager.CurrentColorScheme.Surface;
        public static Color RowContainerColor => ThemeManager.CurrentColorScheme.SurfaceContainerHighest;
        public static Color SelectedRowContainerColor => ThemeManager.CurrentColorScheme.SecondaryContainer;
        public static Color RowColor => ThemeManager.CurrentColorScheme.OnSurface;
        public static Color SelectedRowColor => ThemeManager.CurrentColorScheme.OnSecondaryContainer;
        public static string RowCornerRadius => "20";
    }
}
