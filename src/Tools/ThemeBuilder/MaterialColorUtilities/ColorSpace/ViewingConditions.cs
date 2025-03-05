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

using Bdziam.UI.Theming.MaterialColors.Utils;

namespace Bdziam.UI.Theming.MaterialColors.ColorSpace;

/// <summary>
///     In traditional color spaces, a color can be identified solely by the observer's measurement of
///     the color. Color appearance models such as CAM16 also use information about the environment
///     where the color was observed, known as the viewing conditions.
/// </summary>
/// <remarks>
///     For example, white under the traditional assumption of a midday sun white point is accurately
///     measured as a slightly chromatic blue by CAM16. (roughly, hue 203, chroma 3, lightness 100)
///     <para />
///     This class caches intermediate values of the CAM16 conversion process that depend only on
///     viewing conditions, enabling speed ups.
/// </remarks>
public class ViewingConditions
{
    /// <summary>
    ///     Parameters are intermediate values of the CAM16 conversion process.
    ///     Their names are shorthand for technical color science terminology,
    ///     this class would not benefit from documenting them individually.
    ///     A brief overview is available in the CAM16 specification,
    ///     and a complete overview requires a color science textbook,
    ///     osuch as Fairchild's Color Appearance Models.
    /// </summary>
    public ViewingConditions(
        double n,
        double aw,
        double nbb,
        double ncb,
        double c,
        double nc,
        double[] rgbD,
        double fl,
        double flRoot,
        double z)
    {
        N = n;
        Aw = aw;
        Nbb = nbb;
        Ncb = ncb;
        C = c;
        Nc = nc;
        RgbD = rgbD;
        Fl = fl;
        FlRoot = flRoot;
        Z = z;
    }

    /// <summary>
    ///     sRGB-like viewing conditions.
    /// </summary>
    public static ViewingConditions Default { get; } = Make(
        new[]
        {
            ColorUtils.WhitePointD65[0],
            ColorUtils.WhitePointD65[1],
            ColorUtils.WhitePointD65[2]
        },
        200.0 / Math.PI * ColorUtils.YFromLstar(50.0) / 100.0,
        50.0,
        2.0,
        false);

    public double N { get; }
    public double Aw { get; }
    public double Nbb { get; }
    public double Ncb { get; }
    public double C { get; }
    public double Nc { get; }
    public double[] RgbD { get; }
    public double Fl { get; }
    public double FlRoot { get; }
    public double Z { get; }

    public static ViewingConditions Make(
        double[] whitePoint,
        double adaptingLuminance,
        double backgroundLstar,
        double surround,
        bool discountingIlluminant)
    {
        // Transform white point XYZ to 'cone'/'rgb' responses
        var matrix = Cam16.XyzToCam16Rgb;
        var xyz = whitePoint;
        var rW = xyz[0] * matrix[0][0] + xyz[1] * matrix[0][1] + xyz[2] * matrix[0][2];
        var gW = xyz[0] * matrix[1][0] + xyz[1] * matrix[1][1] + xyz[2] * matrix[1][2];
        var bW = xyz[0] * matrix[2][0] + xyz[1] * matrix[2][1] + xyz[2] * matrix[2][2];
        var f = 0.8 + surround / 10.0;
        var c =
            f >= 0.9
                ? MathUtils.Lerp(0.59, 0.69, (f - 0.9) * 10.0)
                : MathUtils.Lerp(0.525, 0.59, (f - 0.8) * 10.0);
        var d =
            discountingIlluminant
                ? 1.0
                : f * (1.0 - 1.0 / 3.6 * Math.Exp((-adaptingLuminance - 42.0) / 92.0));
        d = d > 1.0 ? 1.0 : d < 0.0 ? 0.0 : d;
        var nc = f;
        double[] rgbD =
        {
            d * (100.0 / rW) + 1.0 - d, d * (100.0 / gW) + 1.0 - d, d * (100.0 / bW) + 1.0 - d
        };
        var k = 1.0 / (5.0 * adaptingLuminance + 1.0);
        var k4 = k * k * k * k;
        var k4F = 1.0 - k4;
        var fl =
            k4 * adaptingLuminance + 0.1 * k4F * k4F * Math.Pow(5.0 * adaptingLuminance, 1.0 / 3.0);
        var n = ColorUtils.YFromLstar(backgroundLstar) / whitePoint[1];
        var z = 1.48 + Math.Sqrt(n);
        var nbb = 0.725 / Math.Pow(n, 0.2);
        var ncb = nbb;
        double[] rgbAFactors =
        {
            Math.Pow(fl * rgbD[0] * rW / 100.0, 0.42),
            Math.Pow(fl * rgbD[1] * gW / 100.0, 0.42),
            Math.Pow(fl * rgbD[2] * bW / 100.0, 0.42)
        };

        double[] rgbA =
        {
            400.0 * rgbAFactors[0] / (rgbAFactors[0] + 27.13),
            400.0 * rgbAFactors[1] / (rgbAFactors[1] + 27.13),
            400.0 * rgbAFactors[2] / (rgbAFactors[2] + 27.13)
        };

        var aw = (2.0 * rgbA[0] + rgbA[1] + 0.05 * rgbA[2]) * nbb;
        return new ViewingConditions(n, aw, nbb, ncb, c, nc, rgbD, fl, Math.Pow(fl, 0.25), z);
    }
}