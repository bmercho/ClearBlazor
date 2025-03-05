namespace Bdziam.UI.Theming.MaterialColors.DynamicColor;

/// <summary>
///     Documents a constraint between two DynamicColors, in which their tones must have a certain distance from each
///     other.
/// </summary>
public sealed class ToneDeltaPair
{
    /// <summary>
    ///     Documents a constraint in tone distance between two DynamicColors.
    /// </summary>
    /// <param name="roleA">The first role in a pair.</param>
    /// <param name="roleB">The second role in a pair.</param>
    /// <param name="delta">Required difference between tones. Absolute value, negative values have undefined behavior.</param>
    /// <param name="polarity">The relative relation between tones of RoleA and RoleB, as described above.</param>
    /// <param name="stayTogether">Whether these two roles should stay on the same side of the "awkward zone" (T50-59).</param>
    public ToneDeltaPair(DynamicColor roleA, DynamicColor roleB, double delta, TonePolarity polarity, bool stayTogether)
    {
        RoleA = roleA;
        RoleB = roleB;
        Delta = delta;
        Polarity = polarity;
        StayTogether = stayTogether;
    }

    /// <summary>
    ///     The first role in a pair.
    /// </summary>
    public DynamicColor RoleA { get; }

    /// <summary>
    ///     The second role in a pair.
    /// </summary>
    public DynamicColor RoleB { get; }

    /// <summary>
    ///     Required difference between tones. Absolute value, negative values have undefined behavior.
    /// </summary>
    public double Delta { get; }

    /// <summary>
    ///     The relative relation between tones of RoleA and RoleB, as described above.
    /// </summary>
    public TonePolarity Polarity { get; }

    /// <summary>
    ///     Whether these two roles should stay on the same side of the "awkward zone" (T50-59).
    ///     This is necessary for certain cases where one role has two backgrounds.
    /// </summary>
    public bool StayTogether { get; }
}