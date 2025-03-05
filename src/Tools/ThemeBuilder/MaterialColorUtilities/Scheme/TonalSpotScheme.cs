using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

/// <summary>
///     A calm theme, sedated colors that aren't particularly chromatic.
/// </summary>
public class TonalSpotScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.TonalSpot,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 36.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 16.0),
    TonalPalette.FromHueAndChroma(
        MathUtils.SanitizeDegreesDouble(sourceColorHct.Hue + 60.0), 24.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 6.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 8.0));