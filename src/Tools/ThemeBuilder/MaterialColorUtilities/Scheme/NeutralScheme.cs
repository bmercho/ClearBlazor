using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

/// <summary>
///     A theme that's slightly more chromatic than monochrome, which is purely black / white / gray.
/// </summary>
public class NeutralScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(sourceColorHct,
    DynamicSchemeVariant.Neutral,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 12.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 8.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 16.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 2.0),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, 2.0));