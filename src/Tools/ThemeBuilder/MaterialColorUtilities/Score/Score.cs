﻿// Copyright 2021 Google LLC
// Copyright 2021-2022 project contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.Score;

public static class Scorer
{
    /// <summary>
    ///     Given a large set of colors, remove colors that are unsuitable for a UI theme, and rank the rest
    ///     based on suitability.
    /// </summary>
    /// <remarks>
    ///     Enables use of a high cluster count for image quantization, thus ensuring colors aren't
    ///     muddied, while curating the high cluster count to a much smaller number of appropriate choices.
    /// </remarks>
    private const double CUTOFF_CHROMA = 15;

    private const double CUTOFF_EXCITED_PROPORTION = 0.01;
    private const double CUTOFF_TONE = 10;
    private const double TARGET_CHROMA = 48;
    private const double WEIGHT_PROPORTION = 0.7;
    private const double WEIGHT_CHROMA_ABOVE = 0.3;
    private const double WEIGHT_CHROMA_BELOW = 0.1;

    public const uint Default = 0xff4285F4; // Google Blue

    /// <summary>
    ///     Given a map with keys of colors and values of how often the color appears, rank the colors
    ///     based on suitability for being used for a UI theme.
    /// </summary>
    /// <param name="colorsToPopulation">
    ///     A dictionary with keys of colors and values of how often the color
    ///     appears, usually from a source image.
    /// </param>
    /// <returns>
    ///     A list of colors sorted by suitability for a UI theme. The most suitable color is the first
    ///     item, the least suitable is the last. There will always be at least one color returned. If
    ///     all the input colors were not suitable for a theme, a default fallback color will be provided,
    ///     Google Blue.
    /// </returns>
    public static List<uint> Score(Dictionary<uint, uint> colorsToPopulation)
    {
        // Determine the total count of all colors.
        double populationSum = 0;
        foreach (var entry in colorsToPopulation) populationSum += entry.Value;

        // Turn the count of each color into a proportion by dividing by the total
        // count. Also, fill a cache of CAM16 colors representing each color, and
        // record the proportion of colors for each CAM16 hue.
        Dictionary<uint, Cam16> colorsToCam = new();
        var hueProportions = new double[361];
        foreach (var entry in colorsToPopulation)
        {
            var color = entry.Key;
            double population = entry.Value;
            var proportion = population / populationSum;

            var cam = Cam16.FromInt(color);
            colorsToCam[color] = cam;

            var hue = (uint)Math.Round(cam.Hue);
            hueProportions[hue] += proportion;
        }

        // Determine the proportion of the colors around each color, by summing the
        // proportions around each color's hue.
        Dictionary<uint, double> colorsToExcitedProportion = new();
        foreach (var entry in colorsToCam)
        {
            var color = entry.Key;
            var cam = entry.Value;
            var hue = (int)Math.Round(cam.Hue);

            double excitedProportion = 0;
            for (var j = hue - 15; j < hue + 15; j++)
            {
                var neighborHue = MathUtils.SanitizeDegreesInt(j);
                excitedProportion += hueProportions[neighborHue];
            }

            colorsToExcitedProportion[color] = excitedProportion;
        }

        // Score the colors by their proportion, as well as how chromatic they are.
        Dictionary<uint, double> colorsToScore = new();
        foreach (var entry in colorsToCam)
        {
            var color = entry.Key;
            var cam = entry.Value;

            var proportion = colorsToExcitedProportion[color];
            var proportionScore = proportion * 100.0 * WEIGHT_PROPORTION;

            var chromaWeight =
                cam.Chroma < TARGET_CHROMA ? WEIGHT_CHROMA_BELOW : WEIGHT_CHROMA_ABOVE;
            var chromaScore = (cam.Chroma - TARGET_CHROMA) * chromaWeight;

            var score = proportionScore + chromaScore;
            colorsToScore[color] = score;
        }

        // Remove colors that are unsuitable, ex. very dark or unchromatic colors.
        // Also, remove colors that are very similar in hue.
        var filteredColors = Filter(colorsToExcitedProportion, colorsToCam);
        Dictionary<uint, double> filteredColorsToScore = new();
        foreach (var color in filteredColors) filteredColorsToScore[color] = colorsToScore[color];

        // Ensure the list of colors returned is sorted such that the first in the
        // list is the most suitable, and the last is the least suitable.
        List<KeyValuePair<uint, double>> entryList = new(filteredColorsToScore);
        entryList.Sort((a, b) => b.Value.CompareTo(a.Value));
        List<uint> colorsByScoreDescending = new();
        foreach (var entry in entryList)
        {
            var color = entry.Key;
            var cam = colorsToCam[color];
            var duplicateHue = false;

            foreach (var alreadyChosenColor in colorsByScoreDescending)
            {
                var alreadyChosenCam = colorsToCam[alreadyChosenColor];
                if (MathUtils.DifferenceDegrees(cam.Hue, alreadyChosenCam.Hue) < 15)
                {
                    duplicateHue = true;
                    break;
                }
            }

            if (duplicateHue) continue;
            colorsByScoreDescending.Add(entry.Key);
        }

        // Ensure that at least one color is returned.
        if (!colorsByScoreDescending.Any()) colorsByScoreDescending.Add(Default);
        return colorsByScoreDescending;
    }

    private static List<uint> Filter(
        Dictionary<uint, double> colorsToExcitedProportion, Dictionary<uint, Cam16> colorsToCam)
    {
        List<uint> filtered = new();
        foreach (var entry in colorsToCam)
        {
            var color = entry.Key;
            var cam = entry.Value;
            var proportion = colorsToExcitedProportion[color];

            if (cam.Chroma >= CUTOFF_CHROMA
                && ColorUtils.LStarFromArgb(color) >= CUTOFF_TONE
                && proportion >= CUTOFF_EXCITED_PROPORTION)
                filtered.Add(color);
        }

        return filtered;
    }
}