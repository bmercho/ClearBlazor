using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

/// <summary>
///     A monochrome theme, colors are purely black / white / gray.
/// </summary>
public class MonochromeScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.Monochrome,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 0.0));