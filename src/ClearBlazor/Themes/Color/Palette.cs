namespace ClearBlazor;

#nullable enable
public abstract class Palette
{
    public virtual Color Black { get; set; } = new("#272c34");
    public virtual Color White { get; set; } = new(Colors.Shades.White);

    public virtual Color Primary { get; set; } = new(Colors.Indigo.Darken1);
    public virtual Color PrimaryContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Secondary { get; set; } = new(Colors.Pink.Accent2);
    public virtual Color SecondaryContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Tertiary { get; set; } = new("#1EC8A5");
    public virtual Color TertiaryContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Info { get; set; } = new(Colors.Blue.Default);
    public virtual Color InfoContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Success { get; set; } = new(Colors.Green.Accent4);
    public virtual Color SuccessContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Warning { get; set; } = new(Colors.Orange.Default);
    public virtual Color WarningContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Error { get; set; } = new(Colors.Red.Default);
    public virtual Color ErrorContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color Dark { get; set; } = new(Colors.Grey.Darken3);
    public virtual Color DarkContrastText { get; set; } = new(Colors.Shades.White);

    public virtual Color TextPrimary { get; set; } = new(Colors.Grey.Darken3);
    public virtual Color TextSecondary { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.54);

    public virtual Color TextDisabled { get; set; } = new Color("#b0b0b0");
    public virtual Color BackgroundDisabled { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.12);

    //public virtual Color ActionDefault { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.54);

    //public virtual Color TextActionDefault { get; set; } = new Color(Colors.Shades.Black);//.SetAlpha(0.54);

    //public virtual Color ActionDefaultBackground { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.06);
    //public virtual Color ActionDefaultBackground { get; set; } = new Color("#f0f0f0");

    //public virtual Color ActionDisabled { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.26);

    //public virtual Color ActionDisabledBackground { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.12);

    public virtual Color Background { get; set; } = new(Colors.Shades.White);

    public virtual Color BackgroundGrey { get; set; } = new(Colors.Grey.Lighten4);

    //public virtual Color Surface { get; set; } = new(Colors.Shades.White);

    //public virtual Color DrawerBackground { get; set; } = new(Colors.Shades.White);

    //public virtual Color DrawerText { get; set; } = new(Colors.Grey.Darken3);

    //public virtual Color DrawerIcon { get; set; } = new(Colors.Grey.Darken2);

    //public virtual Color AppbarBackground { get; set; } = new("#594AE2");

    //public virtual Color AppbarText { get; set; } = new(Colors.Shades.White);

    //public virtual Color LinesDefault { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.12);

    //public virtual Color LinesInputs { get; set; } = new(Colors.Grey.Lighten1);

    //public virtual Color TableLines { get; set; } = new Color(Colors.Grey.Lighten2).SetAlpha(1.0);

    //public virtual Color TableStriped { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.02);

    //public virtual Color TableHover { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.04);

    //public virtual Color Divider { get; set; } = new(Colors.Grey.Lighten2);

    //public virtual Color DividerLight { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.8);

    //public virtual Color ChipDefault { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.08);

    //public virtual Color ChipDefaultHover { get; set; } = new Color(Colors.Shades.Black).SetAlpha(0.12);

    //public string PrimaryDarken
    //{
    //    get => (_primaryDarken ??= Primary.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _primaryDarken = value;
    //}

    //public string PrimaryLighten
    //{
    //    get => (_primaryLighten ??= Primary.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _primaryLighten = value;
    //}

    //public string SecondaryDarken
    //{
    //    get => (_secondaryDarken ??= Secondary.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _secondaryDarken = value;
    //}

    //public string SecondaryLighten
    //{
    //    get => (_secondaryLighten ??= Secondary.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _secondaryLighten = value;
    //}

    //public string TertiaryDarken
    //{
    //    get => (_tertiaryDarken ??= Tertiary.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _tertiaryDarken = value;
    //}

    //public string TertiaryLighten
    //{
    //    get => (_tertiaryLighten ??= Tertiary.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _tertiaryLighten = value;
    //}

    //public string InfoDarken
    //{
    //    get => (_infoDarken ??= Info.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _infoDarken = value;
    //}

    //public string InfoLighten
    //{
    //    get => (_infoLighten ??= Info.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _infoLighten = value;
    //}

    //public string SuccessDarken
    //{
    //    get => (_successDarken ??= Success.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _successDarken = value;
    //}

    //public string SuccessLighten
    //{
    //    get => (_successLighten ??= Success.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _successLighten = value;
    //}

    //public string WarningDarken
    //{
    //    get => (_warningDarken ??= Warning.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _warningDarken = value;
    //}

    //public string WarningLighten
    //{
    //    get => (_warningLighten ??= Warning.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _warningLighten = value;
    //}

    //public string ErrorDarken
    //{
    //    get => (_errorDarken ??= Error.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _errorDarken = value;
    //}

    //public string ErrorLighten
    //{
    //    get => (_errorLighten ??= Error.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _errorLighten = value;
    //}

    //public string DarkDarken
    //{
    //    get => (_darkDarken ??= Dark.ColorRgbDarken()).ToString(MudColorOutputFormats.RGB);
    //    set => _darkDarken = value;
    //}

    //public string DarkLighten
    //{
    //    get => (_darkLighten ??= Dark.ColorRgbLighten()).ToString(MudColorOutputFormats.RGB);
    //    set => _darkLighten = value;
    //}

    public virtual double HoverOpacity { get; set; } = 0.06;

//    public Color GrayDefault { get; set; } = new(Colors.Grey.Default);

    public virtual Color GrayLight { get; set; } = new(Colors.Grey.Lighten1);

    public virtual Color GrayLightContrastText { get; set; } = new(Colors.Grey.Darken4);

    public virtual Color GrayLighter { get; set; } = new(Colors.Grey.Lighten2);

    public virtual Color GrayLighterContrastText { get; set; } = new(Colors.Grey.Darken3);
    //    public Color GrayDark { get; set; } = new(Colors.Grey.Darken1);

    //    public Color GrayDarker { get; set; } = new(Colors.Grey.Darken2);

    //    public Color OverlayDark { get; set; } = new Color("#212121").SetAlpha(0.5);

    //    public Color OverlayLight { get; set; } = new Color(Colors.Shades.White).SetAlpha(0.5);

    public virtual Color AvatarBackgroundColor { get; set; } = new(Colors.Grey.Lighten1);

    public virtual Color ToolTipBackgroundColor { get; set; } = new(Colors.Grey.Darken3);
    public virtual Color ToolTipTextColor { get; set; } = new Color(Colors.Green.Lighten2);

    public virtual Color ScrollbarBackgroundColor { get; set; } = new(Colors.Grey.Lighten4);
    public virtual Color ScrollbarBackgroundBoxShadowColor { get; set; } = new(Colors.Grey.Lighten1);
    public virtual Color ScrollbarThumbColor { get; set; } = new(Colors.Indigo.Darken1);
    public virtual Color ScrollbarOverlayThumbColor { get; set; } = new Color(Colors.Indigo.Darken1).SetAlpha(0.5);
    public virtual Color ListBackgroundColor { get; set; } = new(Colors.Grey.Lighten2);
    public virtual Color ListSelectedColor { get; set; } = new(Colors.Blue.Lighten5);

    public virtual Color ListColor { get; set; } = new(Colors.Grey.Darken1);

}
