using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.DynamicColor;

/// <summary>
///     A class containing a value that changes with the contrast level.
///     Usually represents the contrast requirements for a dynamic color on its background. The four
///     values correspond to values for contrast levels -1.0, 0.0, 0.5, and 1.0, respectively.
/// </summary>
public sealed class ContrastCurve
{
    /// <summary>
    ///     Value for contrast level 1.0
    /// </summary>
    private readonly double high;

    /// <summary>
    ///     Value for contrast level -1.0
    /// </summary>
    private readonly double low;

    /// <summary>
    ///     Value for contrast level 0.5
    /// </summary>
    private readonly double medium;

    /// <summary>
    ///     Value for contrast level 0.0
    /// </summary>
    private readonly double normal;

    /// <summary>
    ///     Creates a <see cref="ContrastCurve" /> object.
    /// </summary>
    /// <param name="low">Value for contrast level -1.0</param>
    /// <param name="normal">Value for contrast level 0.0</param>
    /// <param name="medium">Value for contrast level 0.5</param>
    /// <param name="high">Value for contrast level 1.0</param>
    public ContrastCurve(double low, double normal, double medium, double high)
    {
        this.low = low;
        this.normal = normal;
        this.medium = medium;
        this.high = high;
    }

    /// <summary>
    ///     Returns the value at a given contrast level.
    /// </summary>
    /// <param name="contrastLevel">The contrast level. 0.0 is the default (normal); -1.0 is the lowest; 1.0 is the highest.</param>
    /// <returns>The value. For contrast ratios, a number between 1.0 and 21.0.</returns>
    public double Get(double contrastLevel)
    {
        if (contrastLevel <= -1.0)
            return low;
        if (contrastLevel < 0.0)
            return MathUtils.Lerp(low, normal, (contrastLevel + 1) / 1);
        if (contrastLevel < 0.5)
            return MathUtils.Lerp(normal, medium, contrastLevel / 0.5);
        if (contrastLevel < 1.0)
            return MathUtils.Lerp(medium, high, (contrastLevel - 0.5) / 0.5);
        return high;
    }
}