namespace ClearBlazor
{
    public partial class Divider:ClearComponentBase
    {

        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; ";
            css += $"border-color: {ThemeManager.CurrentColorScheme.OutlineVariant.Value}; ";
            css += $"border-width: 1px 0 0 0; border-style: solid;";
            return css;
        }
    }
}