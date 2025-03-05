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
                BackgroundColor = AppBarTokens.ContainerColor;
        }

        protected override void UpdateChildParams(ClearComponentBase child, int level)
        {
            if (level == 1 && child is TextBlock)
            {
                var text = (TextBlock)child;
                text.ColorOverride = AppBarTokens.TextColor;
            }
            if (level == 1 && child is IconButton)
            {
                var icon = (IconButton)child;
                if (icon.LeadingIcon)
                    icon.ColorOverride = AppBarTokens.LeadingIconColor;
                else
                    icon.ColorOverride = AppBarTokens.TrailingIconColor;
            }
        }
    }
}