using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

/// <summary>
///     A playful theme - the source color's hue does not appear in the theme.
/// </summary>
public class FruitSaladScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.FruitSalad,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue - 50.0), 48.0),
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue - 50.0), 36.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 36.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 10.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 16.0));