using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

/// <summary>
///     A loud theme, colorfulness is maximum for Primary palette, increased for others.
/// </summary>
public class VibrantScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.Vibrant,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 200.0),
    TonalPalette.FromHueAndChroma(
        GetRotatedHue(sourceColorHct, Hues, SecondaryRotations), 24.0),
    TonalPalette.FromHueAndChroma(
        GetRotatedHue(sourceColorHct, Hues, TertiaryRotations), 32.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 10.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 12.0))
{
    private static readonly double[] Hues = [0, 41, 61, 101, 131, 181, 251, 301, 360];
    private static readonly double[] SecondaryRotations = [18, 15, 10, 12, 15, 18, 15, 12, 12];
    private static readonly double[] TertiaryRotations = [35, 30, 20, 25, 30, 35, 30, 25, 25];
}