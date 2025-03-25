using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Dislike;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Temperature;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

public class ContentScheme(Hct sourceColorHct, bool isDark, double contrastLevel) : DynamicScheme(
    sourceColorHct,
    DynamicSchemeVariant.Content,
    isDark,
    contrastLevel,
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, sourceColorHct.Chroma),
    TonalPalette.FromHueAndChroma(
        sourceColorHct.Hue,
        Math.Max(sourceColorHct.Chroma - 32.0, sourceColorHct.Chroma * 0.5)),
    TonalPalette.FromHct(
        DislikeAnalyzer.FixIfDisliked(
            new TemperatureCache(sourceColorHct)
                .GetAnalogousColors(3, 6)[2])),
    TonalPalette.FromHueAndChroma(sourceColorHct.Hue, sourceColorHct.Chroma / 8.0),
    TonalPalette.FromHueAndChroma(
        sourceColorHct.Hue, sourceColorHct.Chroma / 8.0 + 4.0))
{
}