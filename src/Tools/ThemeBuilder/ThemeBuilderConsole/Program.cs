using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Scheme;
using ThemeBuilder;


internal class Program
{
    private static void Main(string[] args)
    {
        uint sourceColor = 0xff769cdf;
        var htc = Hct.FromInt(sourceColor);
        var scheme = new ContentScheme(htc, false, 0);
        CreateColorSchemes.CreateColorSchemeFiles(scheme, "LightColorScheme.cs", @"c:\work\Data");
        scheme = new ContentScheme(htc, true, 0);
        CreateColorSchemes.CreateColorSchemeFiles(scheme, "DarkColorScheme.cs", @"c:\work\Data");
    }
}