using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

/// <summary>
///     A playful theme - the source color's hue does not appear in the theme.
/// </summary>
public class RainbowScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.Rainbow,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 48.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 16.0),
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue + 60.0), 24.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0));