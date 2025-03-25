namespace ClearBlazor
{
    /// <summary>
    /// The AppBar displays information and actions relating to the AppBar's content.
    /// </summary>
    public class AppBar:DockPanel
    {
        bool? _backgroundIsNull = null;
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_backgroundIsNull == null)
                _backgroundIsNull = BackgroundColor == null;

            if (_backgroundIsNull == true)
                BackgroundColor = ThemeManager.CurrentColorScheme.SurfaceContainerLow;
        }

        protected override void UpdateChildParams(ClearComponentBase child, int level)
        {
            if (level == 1 && child is TextBlock)
            {
                var text = (TextBlock)child;
                text.ColorOverride = ThemeManager.CurrentColorScheme.OnSurface;
            }
            if (level == 1 && child is IconButton)
            {
                var icon = (IconButton)child;
                if (icon.LeadingIcon)
                    icon.ColorOverride = ThemeManager.CurrentColorScheme.OnSurface;
                else
                    icon.ColorOverride = ThemeManager.CurrentColorScheme.OnSurfaceVariant;
            }
        }
    }
}