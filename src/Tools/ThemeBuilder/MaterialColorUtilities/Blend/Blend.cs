// Copyright 2021 Google LLC
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

namespace Bdziam.UI.Theming.MaterialColors.Blend;

/// <summary>
///     Functions for blending in HCT and CAM16.
/// </summary>
public static class Blender
{
    /// <summary>
    ///     Blend the design color's HCT hue towards the key color's HCT hue, in a way that leaves the
    ///     original color recognizable and recognizably shifted towards the key color.
    /// </summary>
    /// <param name="designColor">ARGB representation of an arbitrary color.</param>
    /// <param name="sourceColor">ARGB representation of the main theme color.</param>
    /// <returns>
    ///     The design color with a hue shifted towards the system's color, a slightly
    ///     warmer/cooler variant of the design color's hue.
    /// </returns>
    public static uint Harmonize(uint designColor, uint sourceColor)
    {
        var fromHct = Hct.FromInt(designColor);
        var toHct = Hct.FromInt(sourceColor);
        var differenceDegrees = MathUtils.DifferenceDegrees(fromHct.Hue, toHct.Hue);
        var rotationDegrees = Math.Min(differenceDegrees * 0.5, 15.0);
        var outputHue =
            MathUtils.SanitizeDegreesDouble(
                fromHct.Hue
                + rotationDegrees * MathUtils.RotationDirection(fromHct.Hue, toHct.Hue));
        return Hct.From(outputHue, fromHct.Chroma, fromHct.Tone).ToInt();
    }

    /// <summary>
    ///     Blends hue from one color into another. The chroma and tone of the original color are
    ///     maintained.
    /// </summary>
    /// <param name="from">ARGB representation of color</param>
    /// <param name="to">ARGB representation of color</param>
    /// <param name="amount">How much blending to perform; Between 0.0 and 1.0</param>
    /// <returns><paramref name="from" />, with a hue blended towards <paramref name="to" />. Chroma and tone are constant.</returns>
    public static uint HctHue(uint from, uint to, double amount)
    {
        var ucs = Cam16Ucs(from, to, amount);
        var ucsCam = Cam16.FromInt(ucs);
        var fromCam = Cam16.FromInt(from);
        return Hct.From(ucsCam.Hue, fromCam.Chroma, ColorUtils.LStarFromArgb(from)).ToInt();
    }

    /// <summary>
    ///     Blend in CAM16-UCS space.
    /// </summary>
    /// <param name="from">ARGB representation of color</param>
    /// <param name="to">ARGB representation of color</param>
    /// <param name="amount">How much blending to perform; between 0.0 and 1.0</param>
    /// <returns><paramref name="from" />, blended towards <paramref name="to" />. Hue, chroma, and tone will change.</returns>
    public static uint Cam16Ucs(uint from, uint to, double amount)
    {
        var fromCam = Cam16.FromInt(from);
        var toCam = Cam16.FromInt(to);
        var fromJ = fromCam.Jstar;
        var fromA = fromCam.Astar;
        var fromB = fromCam.Bstar;
        var toJ = toCam.Jstar;
        var toA = toCam.Astar;
        var toB = toCam.Bstar;
        var jstar = fromJ + (toJ - fromJ) * amount;
        var astar = fromA + (toA - fromA) * amount;
        var bstar = fromB + (toB - fromB) * amount;
        return Cam16.FromUcs(jstar, astar, bstar).ToInt();
    }
}