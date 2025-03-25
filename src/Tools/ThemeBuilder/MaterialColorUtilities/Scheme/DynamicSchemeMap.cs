using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.DynamicColor;

namespace Bdziam.UI.Theming.MaterialColors.Scheme;

public static class DynamicSchemeMap
{
    public static DynamicScheme GetDynamicScheme(Hct sourceColorHct, bool isDark, double contrastLevel,
        DynamicSchemeVariant dynamicSchemeVariant)
    {
        switch (dynamicSchemeVariant)
        {
            case DynamicSchemeVariant.Monochrome:
                return new MonochromeScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.Neutral:
                return new NeutralScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.TonalSpot:
                return new TonalSpotScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.Vibrant:
                return new VibrantScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.Expressive:
                return new ExpressiveScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.Fidelity:
                return new FidelityScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.Content:
                return new ContentScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.Rainbow:
                return new RainbowScheme(sourceColorHct, isDark, contrastLevel);
            case DynamicSchemeVariant.FruitSalad:
                return new FruitSaladScheme(sourceColorHct, isDark, contrastLevel);
            default:
                throw new ArgumentOutOfRangeException(nameof(dynamicSchemeVariant), dynamicSchemeVariant, null);
        }
    }
}