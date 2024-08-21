namespace ClearBlazor;

public class PaletteDark : Palette
{
    public override Color Black { get; set; } = new("#27272f");

    public override Color Primary { get; set; } = new("#776be7");

    /// <inheritdoc />
    public override Color Info { get; set; } = new("#3299ff");

    public override Color Success { get; set; } = new("#0bba83");

    public override Color Warning { get; set; } = new("#ffa800");

    public override Color Error { get; set; } = new("#f64e62");

    public override Color Dark { get; set; } = new("#27272f");

    public override Color TextPrimary { get; set; } = new("rgba(255,255,255, 0.70)");

    public override Color TextSecondary { get; set; } = new("rgba(255,255,255, 0.50)");

    public override Color TextDisabled { get; set; } = new("rgba(255,255,255, 0.2)");
    public override Color BackgroundDisabled { get; set; } = new("rgba(255,255,255, 0.2)");

    //public override Color ActionDefault { get; set; } = new("#adadb1");

    //public override Color ActionDefaultBackground { get; set; } = new Color("#1e1e2d");

    //public override Color ActionDisabled { get; set; } = new("rgba(255,255,255, 0.26)");

    //public override Color ActionDisabledBackground { get; set; } = new("rgba(255,255,255, 0.12)");

    public override Color Background { get; set; } = new("#32333d");

    public override Color BackgroundGrey { get; set; } = new("#27272f");

    //public override Color Surface { get; set; } = new("#373740");

    //public override Color DrawerBackground { get; set; } = new("#27272f");

    //public override Color DrawerText { get; set; } = new("rgba(255,255,255, 0.50)");

    //public override Color DrawerIcon { get; set; } = new("rgba(255,255,255, 0.50)");

    //public override Color AppbarBackground { get; set; } = new("#27272f");

    //public override Color AppbarText { get; set; } = new("rgba(255,255,255, 0.70)");

    //public override Color LinesDefault { get; set; } = new("rgba(255,255,255, 0.12)");

    //public override Color LinesInputs { get; set; } = new("rgba(255,255,255, 0.3)");

    //public override Color TableLines { get; set; } = new("rgba(255,255,255, 0.12)");

    //public override Color TableStriped { get; set; } = new("rgba(255,255,255, 0.2)");

    //public override Color Divider { get; set; } = new("rgba(255,255,255, 0.12)");

    //public override Color DividerLight { get; set; } = new("rgba(255,255,255, 0.06)");

    //public override Color ChipDefault { get; set; } = new("rgba(255,255,255, 0.16)");

    //public override Color ChipDefaultHover { get; set; } = new("rgba(255,255,255, 0.24)");

    public override Color ToolTipBackgroundColour { get; set; } = new ("#808080");
    public override Color ToolTipTextColour { get; set; } = new ("#0000ff");

    public override Color ListBackgroundColour { get; set; } = new(Colors.Grey.Lighten2);
}
