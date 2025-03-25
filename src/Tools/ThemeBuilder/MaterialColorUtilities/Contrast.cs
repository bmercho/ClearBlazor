using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors;

public static class Contrast
{
    // Constants
    public const double RATIO_MIN = 1.0;
    public const double RATIO_MAX = 21.0;
    public const double RATIO_30 = 3.0;
    public const double RATIO_45 = 4.5;
    public const double RATIO_70 = 7.0;

    private const double CONTRAST_RATIO_EPSILON = 0.04;
    private const double LUMINANCE_GAMUT_MAP_TOLERANCE = 0.4;

    /// <summary>
    ///     Calculates the contrast ratio between two Y (relative luminance) values.
    /// </summary>
    public static double RatioOfYs(double y1, double y2)
    {
        var lighter = Math.Max(y1, y2);
        var darker = lighter == y2 ? y1 : y2;
        return (lighter + 5.0) / (darker + 5.0);
    }

    /// <summary>
    ///     Calculates the contrast ratio between two tones.
    /// </summary>
    public static double RatioOfTones(double t1, double t2)
    {
        return RatioOfYs(ColorUtils.YFromLstar(t1), ColorUtils.YFromLstar(t2));
    }

    /// <summary>
    ///     Returns the tone >= the input tone that ensures the specified contrast ratio.
    ///     Returns -1 if the ratio cannot be achieved.
    /// </summary>
    public static double Lighter(double tone, double ratio)
    {
        if (tone < 0.0 || tone > 100.0) return -1.0;

        var darkY = ColorUtils.YFromLstar(tone);
        var lightY = ratio * (darkY + 5.0) - 5.0;

        if (lightY < 0.0 || lightY > 100.0) return -1.0;

        var realContrast = RatioOfYs(lightY, darkY);
        var delta = Math.Abs(realContrast - ratio);

        if (realContrast < ratio && delta > CONTRAST_RATIO_EPSILON) return -1.0;

        var returnValue = ColorUtils.LStarFromY(lightY) + LUMINANCE_GAMUT_MAP_TOLERANCE;

        if (returnValue < 0 || returnValue > 100) return -1.0;

        return returnValue;
    }

    /// <summary>
    ///     Returns the tone >= the input tone that ensures the specified contrast ratio.
    ///     Returns 100 if the ratio cannot be achieved.
    /// </summary>
    public static double LighterUnsafe(double tone, double ratio)
    {
        var lighterSafe = Lighter(tone, ratio);
        return lighterSafe < 0.0 ? 100.0 : lighterSafe;
    }

    /// <summary>
    ///     Returns the tone <= the input tone that ensures the specified contrast ratio.
    ///     Returns -1 if the ratio cannot be achieved.
    /// </summary>
    public static double Darker(double tone, double ratio)
    {
        if (tone < 0.0 || tone > 100.0) return -1.0;

        var lightY = ColorUtils.YFromLstar(tone);
        var darkY = (lightY + 5.0) / ratio - 5.0;

        if (darkY < 0.0 || darkY > 100.0) return -1.0;

        var realContrast = RatioOfYs(lightY, darkY);
        var delta = Math.Abs(realContrast - ratio);

        if (realContrast < ratio && delta > CONTRAST_RATIO_EPSILON) return -1.0;

        var returnValue = ColorUtils.LStarFromY(darkY) - LUMINANCE_GAMUT_MAP_TOLERANCE;

        if (returnValue < 0 || returnValue > 100) return -1.0;

        return returnValue;
    }

    /// <summary>
    ///     Returns the tone <= the input tone that ensures the specified contrast ratio.
    ///     Returns 0 if the ratio cannot be achieved.
    /// </summary>
    public static double DarkerUnsafe(double tone, double ratio)
    {
        var darkerSafe = Darker(tone, ratio);
        return Math.Max(0.0, darkerSafe);
    }
}