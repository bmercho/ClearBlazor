namespace ClearBlazor
{
    public class LightColorScheme : IColorScheme
    {
        public Color Primary => new Color("#365E9DFF");
        public Color Secondary => new Color("#515F79FF");
        public Color Tertiary => new Color("#804A87FF");
        public Color Error => new Color("#BA1A1AFF");
        public Color Info => new Color("#3241F8FF");
        public Color Success => new Color("#246D00FF");
        public Color Warning => new Color("#904D00FF");
        public Color OnPrimary => new Color("#FFFFFFFF");
        public Color OnSecondary => new Color("#FFFFFFFF");
        public Color OnTertiary => new Color("#FFFFFFFF");
        public Color OnError => new Color("#FFFFFFFF");
        public Color OnInfo => new Color("#FFFFFFFF");
        public Color OnSuccess => new Color("#FFFFFFFF");
        public Color OnWarning => new Color("#FFFFFFFF");
        public Color PrimaryContainer => new Color("#769CDFFF");
        public Color SecondaryContainer => new Color("#D2E0FFFF");
        public Color TertiaryContainer => new Color("#C386C8FF");
        public Color ErrorContainer => new Color("#FFDAD6FF");
        public Color InfoContainer => new Color("#E0E0FFFF");
        public Color SuccessContainer => new Color("#7DFF46FF");
        public Color WarningContainer => new Color("#FFDCC2FF");
        public Color OnPrimaryContainer => new Color("#003168FF");
        public Color OnSecondaryContainer => new Color("#54617BFF");
        public Color OnTertiaryContainer => new Color("#4F1D58FF");
        public Color OnErrorContainer => new Color("#93000AFF");
        public Color OnInfoContainer => new Color("#0518E3FF");
        public Color OnSuccessContainer => new Color("#93000AFF");
        public Color OnWarningContainer => new Color("#6D3900FF");
        public Color PrimaryFixed => new Color("#D6E3FFFF");
        public Color PrimaryFixedDim => new Color("#AAC7FFFF");
        public Color SecondaryFixed => new Color("#D6E3FFFF");
        public Color SecondaryFixedDim => new Color("#B9C7E5FF");
        public Color TertiaryFixed => new Color("#FFD6FEFF");
        public Color TertiaryFixedDim => new Color("#F2B0F6FF");
        public Color OnPrimaryFixed => new Color("#001B3EFF");
        public Color OnPrimaryFixedVariant => new Color("#194683FF");
        public Color OnSecondaryFixed => new Color("#0D1C32FF");
        public Color OnSecondaryFixedVariant => new Color("#3A4760FF");
        public Color OnTertiaryFixed => new Color("#35013FFF");
        public Color OnTertiaryFixedVariant => new Color("#66326EFF");
        public Color SurfaceDim => new Color("#DAD9DFFF");
        public Color Surface => new Color("#F9F9FFFF");
        public Color SurfaceBright => new Color("#F9F9FFFF");
        public Color InverseSurface => new Color("#2F3035FF");
        public Color OnInverseSurface => new Color("#F1F0F6FF");
        public Color InversePrimary => new Color("#AAC7FFFF");
        public Color SurfaceContainerLowest => new Color("#FFFFFFFF");
        public Color SurfaceContainerLow => new Color("#F3F3F9FF");
        public Color SurfaceContainer => new Color("#EEEDF3FF");
        public Color SurfaceContainerHigh => new Color("#E8E7EDFF");
        public Color SurfaceContainerHighest => new Color("#E2E2E8FF");
        public Color OnSurface => new Color("#1A1C20FF");
        public Color OnSurfaceVariant => new Color("#434750FF");
        public Color Outline => new Color("#737781FF");
        public Color OutlineVariant => new Color("#C3C6D2FF");
        public Color Scrim => new Color("#000000FF");
        public Color Shadow => new Color("#000000FF");


        //To be deleted
        public Color BackgroundDisabled { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.12);
        public Color GrayLight { get; set; } = new(Colors.Grey.Lighten1);
        public Color Dark { get; set; } = new(Colors.Grey.Darken3);
        public Color TextDisabled { get; set; } = new Color("#b0b0b0");

        public Color ToolTipTextColor { get; set; } = new Color(Colors.Green.Lighten2);

        public Color ListBackgroundColor { get; set; } = new(Colors.Grey.Lighten2);
        public Color ListSelectedColor { get; set; } = new(Colors.Blue.Lighten5);

        public Color GrayLighter { get; set; } = new(Colors.Grey.Lighten2);
        public  Color Background { get; set; } = new(Colors.Shades.White);
        public Color BackgroundGrey { get; set; } = new(Colors.Grey.Lighten4);

    }
}
