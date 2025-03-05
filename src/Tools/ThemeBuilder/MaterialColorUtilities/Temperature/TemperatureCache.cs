using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.Temperature;

/**
 * Design utilities using color temperature theory.
 * <p>
 *     Analogous colors, complementary color, and cache to efficiently, lazily, generate data for
 *     calculations when needed.
 */
public sealed class TemperatureCache
{
    private readonly Hct _input;

    private Hct _precomputedComplement;
    private List<Hct> _precomputedHctsByHue;
    private List<Hct> _precomputedHctsByTemp;
    private Dictionary<Hct, double> _precomputedTempsByHct;

    private TemperatureCache()
    {
        throw new NotSupportedException();
    }

    /**
     * Create a cache that allows calculation of ex. complementary and analogous colors.
     * 
     * @param input Color to find complement/analogous colors of. Any colors will have the same tone,
     * and chroma as the input color, modulo any restrictions due to the other hues having lower
     * limits on chroma.
     */
    public TemperatureCache(Hct input)
    {
        _input = input;
    }

    /**
     * A color that complements the input color aesthetically.
     * <p>
     *     In art, this is usually described as being across the color wheel. History of this shows
     *     intent as a color that is just as cool-warm as the input color is warm-cool.
     */
    public Hct GetComplement()
    {
        if (_precomputedComplement != null) return _precomputedComplement;

        var coldestHue = GetColdest().Hue;
        var coldestTemp = GetTempsByHct()[GetColdest()];

        var warmestHue = GetWarmest().Hue;
        var warmestTemp = GetTempsByHct()[GetWarmest()];
        var range = warmestTemp - coldestTemp;
        var startHueIsColdestToWarmest = IsBetween(_input.Hue, coldestHue, warmestHue);
        var startHue = startHueIsColdestToWarmest ? warmestHue : coldestHue;
        var endHue = startHueIsColdestToWarmest ? coldestHue : warmestHue;
        var directionOfRotation = 1.0;
        var smallestError = 1000.0;
        var answer = GetHctsByHue()[(int)Math.Round(_input.Hue)];

        var complementRelativeTemp = 1.0 - GetRelativeTemperature(_input);

        for (var hueAddend = 0.0; hueAddend <= 360.0; hueAddend += 1.0)
        {
            var hue = MathUtils.SanitizeDegreesDouble(startHue + directionOfRotation * hueAddend);
            if (!IsBetween(hue, startHue, endHue)) continue;
            var possibleAnswer = GetHctsByHue()[(int)Math.Round(hue)];
            var relativeTemp = (GetTempsByHct()[possibleAnswer] - coldestTemp) / range;
            var error = Math.Abs(complementRelativeTemp - relativeTemp);
            if (error < smallestError)
            {
                smallestError = error;
                answer = possibleAnswer;
            }
        }

        _precomputedComplement = answer;
        return _precomputedComplement;
    }

    /**
     * 5 colors that pair well with the input color.
     * <p>The colors are equidistant in temperature and adjacent in hue.
     */
    public List<Hct> GetAnalogousColors()
    {
        return GetAnalogousColors(5, 12);
    }

    /**
     * A set of colors with differing hues, equidistant in temperature.
     * <p>
     *     Behavior is undefined when count or divisions is 0. When divisions
     *     < count, colors repeat.
     * 
     *         @ param count The number of colors to return, includes the input color.
     *         @ param divisions The number of divisions on the color wheel.
     */
    public List<Hct> GetAnalogousColors(int count, int divisions)
    {
        var startHue = (int)Math.Round(_input.Hue);
        var startHct = GetHctsByHue()[startHue];
        var lastTemp = GetRelativeTemperature(startHct);

        var allColors = new List<Hct> { startHct };

        var absoluteTotalTempDelta = 0.0;
        for (var i = 0; i < 360; i++)
        {
            var hue = MathUtils.SanitizeDegreesInt(startHue + i);
            var hct = GetHctsByHue()[(int)hue];
            var temp = GetRelativeTemperature(hct);
            var tempDelta = Math.Abs(temp - lastTemp);
            lastTemp = temp;
            absoluteTotalTempDelta += tempDelta;
        }

        var hueAddend = 1;
        var tempStep = absoluteTotalTempDelta / divisions;
        var totalTempDelta = 0.0;
        lastTemp = GetRelativeTemperature(startHct);
        while (allColors.Count < divisions)
        {
            var hue = MathUtils.SanitizeDegreesInt(startHue + hueAddend);
            var hct = GetHctsByHue()[(int)hue];
            var temp = GetRelativeTemperature(hct);
            var tempDelta = Math.Abs(temp - lastTemp);
            totalTempDelta += tempDelta;

            var desiredTotalTempDeltaForIndex = allColors.Count * tempStep;
            var indexSatisfied = totalTempDelta >= desiredTotalTempDeltaForIndex;
            var indexAddend = 1;

            while (indexSatisfied && allColors.Count < divisions)
            {
                allColors.Add(hct);
                desiredTotalTempDeltaForIndex = (allColors.Count + indexAddend) * tempStep;
                indexSatisfied = totalTempDelta >= desiredTotalTempDeltaForIndex;
                indexAddend++;
            }

            lastTemp = temp;
            hueAddend++;

            if (hueAddend > 360)
            {
                while (allColors.Count < divisions) allColors.Add(hct);
                break;
            }
        }

        var answers = new List<Hct> { _input };

        var ccwCount = (int)Math.Floor((count - 1) / 2.0);
        for (var i = 1; i < ccwCount + 1; i++)
        {
            var index = 0 - i;
            while (index < 0) index = allColors.Count + index;
            if (index >= allColors.Count) index %= allColors.Count;
            answers.Insert(0, allColors[index]);
        }

        var cwCount = count - ccwCount - 1;
        for (var i = 1; i < cwCount + 1; i++)
        {
            var index = i;
            while (index < 0) index = allColors.Count + index;
            if (index >= allColors.Count) index %= allColors.Count;
            answers.Add(allColors[index]);
        }

        return answers;
    }

    public double GetRelativeTemperature(Hct hct)
    {
        var range = GetTempsByHct()[GetWarmest()] - GetTempsByHct()[GetColdest()];
        var differenceFromColdest = GetTempsByHct()[hct] - GetTempsByHct()[GetColdest()];
        if (range == 0.0) return 0.5;
        return differenceFromColdest / range;
    }

    public static double RawTemperature(Hct color)
    {
        var lab = ColorUtils.LabFromArgb(color.ToInt());
        var hue = MathUtils.SanitizeDegreesDouble(Math.Atan2(lab[2], lab[1]) * (180.0 / Math.PI));
        var chroma = Math.Sqrt(Math.Pow(lab[1], 2) + Math.Pow(lab[2], 2));
        return -0.5
               + 0.02 * Math.Pow(chroma, 1.07)
                      * Math.Cos(MathUtils.SanitizeDegreesDouble(hue - 50.0) * (Math.PI / 180.0));
    }

    private Hct GetColdest()
    {
        return GetHctsByTemp()[0];
    }

    private List<Hct> GetHctsByHue()
    {
        if (_precomputedHctsByHue != null) return _precomputedHctsByHue;
        var hcts = new List<Hct>();
        for (var hue = 0.0; hue <= 360.0; hue += 1.0)
        {
            var colorAtHue = Hct.From(hue, _input.Chroma, _input.Tone);
            hcts.Add(colorAtHue);
        }

        _precomputedHctsByHue = hcts.AsReadOnly().ToList();
        return _precomputedHctsByHue;
    }

    private List<Hct> GetHctsByTemp()
    {
        if (_precomputedHctsByTemp != null) return _precomputedHctsByTemp;

        var hcts = new List<Hct>(GetHctsByHue()) { _input };
        hcts.Sort((a, b) => GetTempsByHct()[a].CompareTo(GetTempsByHct()[b]));
        _precomputedHctsByTemp = hcts;
        return _precomputedHctsByTemp;
    }

    private Dictionary<Hct, double> GetTempsByHct()
    {
        if (_precomputedTempsByHct != null) return _precomputedTempsByHct;

        var allHcts = new List<Hct>(GetHctsByHue()) { _input };

        var temperaturesByHct = new Dictionary<Hct, double>();
        foreach (var hct in allHcts) temperaturesByHct[hct] = RawTemperature(hct);

        _precomputedTempsByHct = temperaturesByHct;
        return _precomputedTempsByHct;
    }

    private Hct GetWarmest()
    {
        return GetHctsByTemp()[GetHctsByTemp().Count - 1];
    }

    private static bool IsBetween(double angle, double a, double b)
    {
        if (a < b) return a <= angle && angle <= b;
        return a <= angle || angle <= b;
    }
}