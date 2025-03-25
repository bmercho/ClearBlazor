using Bdziam.UI.Theming.MaterialColors.ColorSpace;

namespace Bdziam.UI.Theming.MaterialColors.Palettes;

/**
 * Key color is a color that represents the hue and chroma of a tonal palette.
 */
public sealed class KeyColor
{
    private const double MAX_CHROMA_VALUE = 200.0;

    // Cache that maps tone to max chroma to avoid duplicated HCT calculation.
    private readonly Dictionary<uint, double> chromaCache = new();

    /**
     * Key color is a color that represents the hue and chroma of a tonal palette
     */
    public KeyColor(double hue, double requestedChroma)
    {
        Hue = hue;
        RequestedChroma = requestedChroma;
    }

    public double Hue { get; }
    private double RequestedChroma { get; }

    /**
     * Creates a key color from a [hue] and a [chroma]. The key color is the first tone, starting
     * from T50, matching the given hue and chroma.
     * 
     * @return Key color [Hct]
     */
    public Hct Create()
    {
        // Pivot around T50 because T50 has the most chroma available, on
        // average. Thus it is most likely to have a direct answer.
        uint pivotTone = 50;
        uint toneStepSize = 1;
        // Epsilon to accept values slightly higher than the requested chroma.
        var epsilon = 0.01;

        // Binary search to find the tone that can provide a chroma that is closest
        // to the requested chroma.
        uint lowerTone = 0;
        uint upperTone = 100;
        while (lowerTone < upperTone)
        {
            var midTone = (lowerTone + upperTone) / 2;
            var isAscending = maxChroma(midTone) < maxChroma(midTone + toneStepSize);
            var sufficientChroma = maxChroma(midTone) >= RequestedChroma - epsilon;

            if (sufficientChroma)
            {
                // Either range [lowerTone, midTone] or [midTone, upperTone] has
                // the answer, so search in the range that is closer the pivot tone.
                if (Math.Abs(lowerTone - pivotTone) < Math.Abs(upperTone - pivotTone))
                {
                    upperTone = midTone;
                }
                else
                {
                    if (lowerTone == midTone) return Hct.From(Hue, RequestedChroma, lowerTone);
                    lowerTone = midTone;
                }
            }
            else
            {
                // As there is no sufficient chroma in the midTone, follow the direction to the chroma
                // peak.
                if (isAscending)
                    lowerTone = midTone + toneStepSize;
                else
                    // Keep midTone for potential chroma peak.
                    upperTone = midTone;
            }
        }

        return Hct.From(Hue, RequestedChroma, lowerTone);
    }

    // Find the maximum chroma for a given tone
    private double maxChroma(uint tone)
    {
        if (!chromaCache.TryGetValue(tone, out var chroma))
        {
            var newChroma = Hct.From(Hue, MAX_CHROMA_VALUE, tone).Chroma;
            chromaCache.Add(tone, newChroma);
        }

        return chromaCache[tone];
    }
}