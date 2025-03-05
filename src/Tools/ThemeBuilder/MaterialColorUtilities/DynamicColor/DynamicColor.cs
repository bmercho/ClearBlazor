using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Palettes;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.DynamicColor;

/// <summary>
///     A color that adjusts itself based on UI state, represented by DynamicScheme.
/// </summary>
public sealed class DynamicColor
{
    public DynamicColor(
        string name,
        Func<DynamicScheme, TonalPalette> palette,
        Func<DynamicScheme, double> tone,
        bool isBackground,
        Func<DynamicScheme, DynamicColor>? background = null,
        Func<DynamicScheme, DynamicColor>? secondBackground = null,
        ContrastCurve? contrastCurve = null,
        Func<DynamicScheme, ToneDeltaPair>? toneDeltaPair = null,
        Func<DynamicScheme, double>? opacity = null)
    {
        Name = name;
        Palette = palette;
        Tone = tone;
        IsBackground = isBackground;
        Background = background;
        SecondBackground = secondBackground;
        ContrastCurve = contrastCurve;
        ToneDeltaPair = toneDeltaPair;
        Opacity = opacity;
    }

    public string Name { get; }
    public Func<DynamicScheme, TonalPalette> Palette { get; }
    public Func<DynamicScheme, double> Tone { get; }
    public bool IsBackground { get; }
    public Func<DynamicScheme, DynamicColor>? Background { get; }
    public Func<DynamicScheme, DynamicColor>? SecondBackground { get; }
    public ContrastCurve? ContrastCurve { get; }
    public Func<DynamicScheme, ToneDeltaPair>? ToneDeltaPair { get; }
    public Func<DynamicScheme, double>? Opacity { get; }

    public static DynamicColor FromPalette(
        string name,
        Func<DynamicScheme, TonalPalette> palette,
        Func<DynamicScheme, double> tone,
        bool isBackground = false)
    {
        return new DynamicColor(name, palette, tone, isBackground);
    }

    public static DynamicColor FromArgb(string name, uint argb)
    {
        var hct = Hct.FromInt(argb);
        var palette = TonalPalette.FromInt(argb);
        return FromPalette(name, _ => palette, _ => hct.Tone);
    }

    public uint GetArgb(DynamicScheme scheme)
    {
        var argb = GetHct(scheme).ToInt();
        if (Opacity == null) return argb;

        var percentage = Opacity(scheme);
        var alpha = MathUtils.ClampInt(0, 255, (uint)Math.Round(percentage * 255));
        return (argb & 0x00FFFFFF) | (alpha << 24);
    }

    public Hct GetHct(DynamicScheme scheme)
    {
        var tone = GetTone(scheme);
        var result = Hct.FromInt(Palette(scheme).Tone((uint)tone));
        return result;
    }

    public double GetTone(DynamicScheme scheme)
    {
        var decreasingContrast = scheme.ContrastLevel < 0;

        // Case 1: dual foreground, pair of colors with delta constraint.
        if (ToneDeltaPair != null)
        {
            var toneDeltaPair = ToneDeltaPair(scheme);
            var roleA = toneDeltaPair.RoleA;
            var roleB = toneDeltaPair.RoleB;
            var delta = toneDeltaPair.Delta;
            var polarity = toneDeltaPair.Polarity;
            var stayTogether = toneDeltaPair.StayTogether;

            var bg = Background(scheme);
            var bgTone = bg.GetTone(scheme);

            var aIsNearer =
                polarity == TonePolarity.Nearer ||
                (polarity == TonePolarity.Lighter && !scheme.IsDark) ||
                (polarity == TonePolarity.Darker && scheme.IsDark);

            var nearer = aIsNearer ? roleA : roleB;
            var farther = aIsNearer ? roleB : roleA;
            var amNearer = Name.Equals(nearer.Name, StringComparison.Ordinal);
            double expansionDir = scheme.IsDark ? 1 : -1;

            // 1st round: solve to min, each
            var nContrast = nearer.ContrastCurve.Get(scheme.ContrastLevel);
            var fContrast = farther.ContrastCurve.Get(scheme.ContrastLevel);

            // Initial and adjusted tones for `nearer`
            var nInitialTone = nearer.Tone(scheme);
            var nTone = Contrast.RatioOfTones(bgTone, nInitialTone) >= nContrast
                ? nInitialTone
                : ForegroundTone(bgTone, nContrast);

            // Initial and adjusted tones for `farther`
            var fInitialTone = farther.Tone(scheme);
            var fTone = Contrast.RatioOfTones(bgTone, fInitialTone) >= fContrast
                ? fInitialTone
                : ForegroundTone(bgTone, fContrast);

            if (decreasingContrast)
            {
                nTone = ForegroundTone(bgTone, nContrast);
                fTone = ForegroundTone(bgTone, fContrast);
            }

            // If constraint is not satisfied, try another round.
            if ((fTone - nTone) * expansionDir < delta)
            {
                fTone = Math.Clamp(nTone + delta * expansionDir, 0, 100);
                if ((fTone - nTone) * expansionDir < delta) nTone = Math.Clamp(fTone - delta * expansionDir, 0, 100);
            }

            // Avoid the 50-59 awkward zone.
            if (50 <= nTone && nTone < 60)
            {
                if (expansionDir > 0)
                {
                    nTone = 60;
                    fTone = Math.Max(fTone, nTone + delta * expansionDir);
                }
                else
                {
                    nTone = 49;
                    fTone = Math.Min(fTone, nTone + delta * expansionDir);
                }
            }
            else if (50 <= fTone && fTone < 60)
            {
                if (stayTogether)
                {
                    if (expansionDir > 0)
                    {
                        nTone = 60;
                        fTone = Math.Max(fTone, nTone + delta * expansionDir);
                    }
                    else
                    {
                        nTone = 49;
                        fTone = Math.Min(fTone, nTone + delta * expansionDir);
                    }
                }
                else
                {
                    if (expansionDir > 0)
                        fTone = 60;
                    else
                        fTone = 49;
                }
            }

            return amNearer ? nTone : fTone;
        }
        else
        {
            // Case 2: No contrast pair; just solve for itself.
            var answer = Tone(scheme);

            if (Background == null) return answer; // No adjustment for colors with no background.

            var bgTone = Background(scheme).GetTone(scheme);
            var desiredRatio = ContrastCurve.Get(scheme.ContrastLevel);

            if (Contrast.RatioOfTones(bgTone, answer) >= desiredRatio)
            {
                // Do nothing; it's good enough.
            }
            else
            {
                answer = ForegroundTone(bgTone, desiredRatio);
            }

            if (decreasingContrast) answer = ForegroundTone(bgTone, desiredRatio);

            if (IsBackground && 50 <= answer && answer < 60)
                answer = Contrast.RatioOfTones(49, bgTone) >= desiredRatio ? 49 : 60;

            if (SecondBackground != null)
            {
                // Case 3: Adjust for dual backgrounds.
                var bgTone1 = Background(scheme).GetTone(scheme);
                var bgTone2 = SecondBackground(scheme).GetTone(scheme);

                var upper = Math.Max(bgTone1, bgTone2);
                var lower = Math.Min(bgTone1, bgTone2);

                if (Contrast.RatioOfTones(upper, answer) >= desiredRatio &&
                    Contrast.RatioOfTones(lower, answer) >= desiredRatio)
                    return answer;

                var lightOption = Contrast.Lighter(upper, desiredRatio);
                var darkOption = Contrast.Darker(lower, desiredRatio);

                var availables = new List<double>();
                if (lightOption != -1) availables.Add(lightOption);

                if (darkOption != -1) availables.Add(darkOption);

                var prefersLight = TonePrefersLightForeground(bgTone1) ||
                                   TonePrefersLightForeground(bgTone2);

                if (prefersLight) return lightOption == -1 ? 100 : lightOption;

                if (availables.Count == 1) return availables[0];

                return darkOption == -1 ? 0 : darkOption;
            }

            return answer;
        }
    }

    public static double ForegroundTone(double bgTone, double ratio)
    {
        var lighterTone = Contrast.LighterUnsafe(bgTone, ratio);
        var darkerTone = Contrast.DarkerUnsafe(bgTone, ratio);
        var lighterRatio = Contrast.RatioOfTones(lighterTone, bgTone);
        var darkerRatio = Contrast.RatioOfTones(darkerTone, bgTone);

        return TonePrefersLightForeground(bgTone)
            ? lighterRatio >= ratio || lighterRatio >= darkerRatio ? lighterTone : darkerTone
            : darkerRatio >= ratio || darkerRatio >= lighterRatio
                ? darkerTone
                : lighterTone;
    }

    public static bool TonePrefersLightForeground(double tone)
    {
        return Math.Round(tone) < 60;
    }

    public static bool ToneAllowsLightForeground(double tone)
    {
        return Math.Round(tone) <= 49;
    }

    public static double EnableLightForeground(double tone)
    {
        return TonePrefersLightForeground(tone) && !ToneAllowsLightForeground(tone) ? 49.0 : tone;
    }
}