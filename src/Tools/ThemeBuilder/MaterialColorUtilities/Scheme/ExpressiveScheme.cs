using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

public class ExpressiveScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.Expressive,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue + 240.0), 40.0),
    TonalPalette.FromHueAndChroma(
        GetRotatedHue(sourceColorHct, Hues, SecondaryRotations), 24.0),
    TonalPalette.FromHueAndChroma(
        GetRotatedHue(sourceColorHct, Hues, TertiaryRotations), 32.0),
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue + 15.0), 8.0),
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue + 15.0), 12.0))
{
    // NOMUTANTS--arbitrary increments/decrements, correctly, still passes tests.
    private static readonly double[] Hues = [0, 21, 51, 121, 151, 191, 271, 321, 360];
    private static readonly double[] SecondaryRotations = [45, 95, 45, 20, 45, 90, 45, 45, 45];
    private static readonly double[] TertiaryRotations = [120, 120, 20, 45, 20, 15, 20, 120, 120];
}