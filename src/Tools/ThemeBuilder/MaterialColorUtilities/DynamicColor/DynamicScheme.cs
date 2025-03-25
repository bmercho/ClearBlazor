using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.DynamicColor;

public class DynamicScheme
{
    public DynamicScheme(
        Hct sourceColorHct,
        DynamicSchemeVariant variant,
        bool isDark,
        double contrastLevel,
        TonalPalette primaryPalette,
        TonalPalette secondaryPalette,
        TonalPalette tertiaryPalette,
        TonalPalette neutralPalette,
        TonalPalette neutralVariantPalette)
        : this(sourceColorHct, variant, isDark, contrastLevel, primaryPalette, secondaryPalette, tertiaryPalette,
            neutralPalette, neutralVariantPalette, null, null, null, null)
    {
    }

    public DynamicScheme(
        Hct sourceColorHct,
        DynamicSchemeVariant variant,
        bool isDark,
        double contrastLevel,
        TonalPalette primaryPalette,
        TonalPalette secondaryPalette,
        TonalPalette tertiaryPalette,
        TonalPalette neutralPalette,
        TonalPalette neutralVariantPalette,
        TonalPalette? errorPalette,
        TonalPalette? warningPalette,
        TonalPalette? infoPalette,
        TonalPalette? successPalette)
    {
        SourceColorArgb = sourceColorHct.ToInt();
        SourceColorHct = sourceColorHct;
        Variant = variant;
        IsDark = isDark;
        ContrastLevel = contrastLevel;

        PrimaryPalette = primaryPalette;
        SecondaryPalette = secondaryPalette;
        TertiaryPalette = tertiaryPalette;
        NeutralPalette = neutralPalette;
        NeutralVariantPalette = neutralVariantPalette;
        ErrorPalette = errorPalette ?? TonalPalette.FromHueAndChroma(25.0, 84.0);
        InfoPalette = infoPalette ?? TonalPalette.FromHueAndChroma(282, 80);
        WarningPalette = warningPalette ?? TonalPalette.FromHueAndChroma(59.0, 63.0);
        ;
        SuccessPalette = successPalette ?? TonalPalette.FromHueAndChroma(139, 89.0);
        ;
    }

    public uint SourceColorArgb { get; }
    public Hct SourceColorHct { get; }
    public DynamicSchemeVariant Variant { get; }
    public bool IsDark { get; }
    public double ContrastLevel { get; }

    public TonalPalette PrimaryPalette { get; }
    public TonalPalette SecondaryPalette { get; }
    public TonalPalette TertiaryPalette { get; }
    public TonalPalette NeutralPalette { get; }
    public TonalPalette NeutralVariantPalette { get; }
    public TonalPalette ErrorPalette { get; }
    public TonalPalette SuccessPalette { get; }

    public TonalPalette InfoPalette { get; }
    public TonalPalette WarningPalette { get; }

    // Properties for key colors
    public uint PrimaryPaletteKeyColor => GetArgb(MaterialDynamicColors.PrimaryPaletteKeyColor());
    public uint SecondaryPaletteKeyColor => GetArgb(MaterialDynamicColors.SecondaryPaletteKeyColor());
    public uint TertiaryPaletteKeyColor => GetArgb(MaterialDynamicColors.TertiaryPaletteKeyColor());
    public uint NeutralPaletteKeyColor => GetArgb(MaterialDynamicColors.NeutralPaletteKeyColor());
    public uint NeutralVariantPaletteKeyColor => GetArgb(MaterialDynamicColors.NeutralVariantPaletteKeyColor());

    // Properties for colors
    public uint Background => GetArgb(MaterialDynamicColors.Background());
    public uint OnBackground => GetArgb(MaterialDynamicColors.OnBackground());
    public uint Surface => GetArgb(MaterialDynamicColors.Surface());
    public uint SurfaceDim => GetArgb(MaterialDynamicColors.SurfaceDim());
    public uint SurfaceBright => GetArgb(MaterialDynamicColors.SurfaceBright());
    public uint SurfaceContainerLowest => GetArgb(MaterialDynamicColors.SurfaceContainerLowest());
    public uint SurfaceContainerLow => GetArgb(MaterialDynamicColors.SurfaceContainerLow());
    public uint SurfaceContainer => GetArgb(MaterialDynamicColors.SurfaceContainer());
    public uint SurfaceContainerHigh => GetArgb(MaterialDynamicColors.SurfaceContainerHigh());
    public uint SurfaceContainerHighest => GetArgb(MaterialDynamicColors.SurfaceContainerHighest());
    public uint OnSurface => GetArgb(MaterialDynamicColors.OnSurface());
    public uint SurfaceVariant => GetArgb(MaterialDynamicColors.SurfaceVariant());
    public uint OnSurfaceVariant => GetArgb(MaterialDynamicColors.OnSurfaceVariant());
    public uint InverseSurface => GetArgb(MaterialDynamicColors.InverseSurface());
    public uint InverseOnSurface => GetArgb(MaterialDynamicColors.InverseOnSurface());
    public uint Outline => GetArgb(MaterialDynamicColors.Outline());
    public uint OutlineVariant => GetArgb(MaterialDynamicColors.OutlineVariant());
    public uint Shadow => GetArgb(MaterialDynamicColors.Shadow());
    public uint Scrim => GetArgb(MaterialDynamicColors.Scrim());
    public uint SurfaceTint => GetArgb(MaterialDynamicColors.SurfaceTint());
    public uint Primary => GetArgb(MaterialDynamicColors.Primary());
    public uint OnPrimary => GetArgb(MaterialDynamicColors.OnPrimary());
    public uint PrimaryContainer => GetArgb(MaterialDynamicColors.PrimaryContainer());
    public uint OnPrimaryContainer => GetArgb(MaterialDynamicColors.OnPrimaryContainer());
    public uint InversePrimary => GetArgb(MaterialDynamicColors.InversePrimary());
    public uint Secondary => GetArgb(MaterialDynamicColors.Secondary());
    public uint OnSecondary => GetArgb(MaterialDynamicColors.OnSecondary());
    public uint SecondaryContainer => GetArgb(MaterialDynamicColors.SecondaryContainer());
    public uint OnSecondaryContainer => GetArgb(MaterialDynamicColors.OnSecondaryContainer());
    public uint Tertiary => GetArgb(MaterialDynamicColors.Tertiary());
    public uint OnTertiary => GetArgb(MaterialDynamicColors.OnTertiary());
    public uint TertiaryContainer => GetArgb(MaterialDynamicColors.TertiaryContainer());
    public uint OnTertiaryContainer => GetArgb(MaterialDynamicColors.OnTertiaryContainer());
    public uint Error => GetArgb(MaterialDynamicColors.Error());
    public uint OnError => GetArgb(MaterialDynamicColors.OnError());
    public uint ErrorContainer => GetArgb(MaterialDynamicColors.ErrorContainer());
    public uint OnErrorContainer => GetArgb(MaterialDynamicColors.OnErrorContainer());
    public uint Warning => GetArgb(MaterialDynamicColors.Warning());
    public uint OnWarning => GetArgb(MaterialDynamicColors.OnWarning());
    public uint WarningContainer => GetArgb(MaterialDynamicColors.WarningContainer());
    public uint OnWarningContainer => GetArgb(MaterialDynamicColors.OnWarningContainer());
    public uint Info => GetArgb(MaterialDynamicColors.Info());
    public uint OnInfo => GetArgb(MaterialDynamicColors.OnInfo());
    public uint InfoContainer => GetArgb(MaterialDynamicColors.InfoContainer());
    public uint OnInfoContainer => GetArgb(MaterialDynamicColors.OnInfoContainer());
    public uint Success => GetArgb(MaterialDynamicColors.Success());
    public uint OnSuccess => GetArgb(MaterialDynamicColors.OnSuccess());
    public uint SuccessContainer => GetArgb(MaterialDynamicColors.SuccessContainer());

    public uint OnSuccessContainer => GetArgb(MaterialDynamicColors.OnSuccessContainer());

    // Properties for fixed colors
    public uint PrimaryFixed => GetArgb(MaterialDynamicColors.PrimaryFixed());
    public uint PrimaryFixedDim => GetArgb(MaterialDynamicColors.PrimaryFixedDim());
    public uint OnPrimaryFixed => GetArgb(MaterialDynamicColors.OnPrimaryFixed());
    public uint OnPrimaryFixedVariant => GetArgb(MaterialDynamicColors.OnPrimaryFixedVariant());
    public uint SecondaryFixed => GetArgb(MaterialDynamicColors.SecondaryFixed());
    public uint SecondaryFixedDim => GetArgb(MaterialDynamicColors.SecondaryFixedDim());
    public uint OnSecondaryFixed => GetArgb(MaterialDynamicColors.OnSecondaryFixed());
    public uint OnSecondaryFixedVariant => GetArgb(MaterialDynamicColors.OnSecondaryFixedVariant());
    public uint TertiaryFixed => GetArgb(MaterialDynamicColors.TertiaryFixed());
    public uint TertiaryFixedDim => GetArgb(MaterialDynamicColors.TertiaryFixedDim());
    public uint OnTertiaryFixed => GetArgb(MaterialDynamicColors.OnTertiaryFixed());
    public uint OnTertiaryFixedVariant => GetArgb(MaterialDynamicColors.OnTertiaryFixedVariant());

    // Properties for controls and text
    public uint ControlActivated => GetArgb(MaterialDynamicColors.ControlActivated());
    public uint ControlNormal => GetArgb(MaterialDynamicColors.ControlNormal());
    public uint ControlHighlight => GetArgb(MaterialDynamicColors.ControlHighlight());
    public uint TextPrimaryInverse => GetArgb(MaterialDynamicColors.TextPrimaryInverse());
    public uint TextSecondaryAndTertiaryInverse => GetArgb(MaterialDynamicColors.TextSecondaryAndTertiaryInverse());
    public uint TextPrimaryInverseDisableOnly => GetArgb(MaterialDynamicColors.TextPrimaryInverseDisableOnly());

    public uint TextSecondaryAndTertiaryInverseDisabled =>
        GetArgb(MaterialDynamicColors.TextSecondaryAndTertiaryInverseDisabled());

    public uint TextHintInverse => GetArgb(MaterialDynamicColors.TextHintInverse());

    public static double GetRotatedHue(Hct sourceColorHct, double[] hues, double[] rotations)
    {
        var sourceHue = sourceColorHct.Hue;
        if (rotations.Length == 1) return MathUtils.SanitizeDegreesDouble(sourceHue + rotations[0]);

        for (var i = 0; i < hues.Length - 1; i++)
        {
            var thisHue = hues[i];
            var nextHue = hues[i + 1];
            if (thisHue < sourceHue && sourceHue < nextHue)
                return MathUtils.SanitizeDegreesDouble(sourceHue + rotations[i]);
        }

        return sourceHue;
    }

    public Hct GetHct(DynamicColor dynamicColor)
    {
        return dynamicColor.GetHct(this);
    }

    public uint GetArgb(DynamicColor dynamicColor)
    {
        var hct = dynamicColor.GetHct(this);
        return hct.ToInt();
    }
}