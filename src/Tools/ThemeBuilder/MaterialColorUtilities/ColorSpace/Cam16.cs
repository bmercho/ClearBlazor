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
///     CAM16, a color appearance model. Colors are not just defined by their hex code,
///     but rather, a hex code and viewing conditions.
/// </summary>
/// <remarks>
///     CAM16 instances also have coordinates in the CAM16-UCS space, called J*, a*, b*, or jstar,
///     astar, bstar in code. CAM16-UCS is included in the CAM16 specification, and should be used when
///     measuring distances between colors.
///     In traditional color spaces, a color can be identified solely by the observer's measurement of
///     the color. Color appearance models such as CAM16 also use information about the environment where
///     the color was observed, known as the viewing conditions.
///     For example, white under the traditional assumption of a midday sun white point is accurately
///     measured as a slightly chromatic blue by CAM16. (roughly, hue 203, chroma 3, lightness 100)
/// </remarks>
public class Cam16
{
    // Transforms XYZ color space coordinates to 'cone'/'RGB' responses in CAM16.
    internal static readonly double[][] XyzToCam16Rgb =
    {
        new[] { 0.401288, 0.650173, -0.051461 },
        new[] { -0.250268, 1.204414, 0.045854 },
        new[] { -0.002079, 0.048952, 0.953127 }
    };

    // Transforms 'cone'/'RGB' responses in CAM16 to XYZ color space coordinates.
    internal static readonly double[][] Cam16RgbToXyz =
    {
        new[] { 1.8620678, -1.0112547, 0.14918678 },
        new[] { 0.38752654, 0.62144744, -0.00897398 },
        new[] { -0.01584150, -0.03412294, 1.0499644 }
    };

    /// <summary>
    ///     All of the CAM16 dimensions can be calculated from 3 of the dimensions, in the following
    ///     combinations: - {j or q} and {c, m, or s} and hue - jstar, astar, bstar.
    ///     Prefer using a static
    ///     method that constructs from 3 of those dimensions. This constructor is intended for those
    ///     methods to use to return all possible dimensions.
    /// </summary>
    /// <param name="hue">for example, red, orange, yellow, green, etc.</param>
    /// <param name="chroma">
    ///     informally, colorfulness / color intensity. like saturation in HSL, except
    ///     perceptually accurate.
    /// </param>
    /// <param name="j">lightness</param>
    /// <param name="q">brightness; ratio of lightness to white point's lightness</param>
    /// <param name="m">colorfulness</param>
    /// <param name="s">saturation; ratio of chroma to white point's chroma</param>
    /// <param name="jStar">CAM16-UCS J* coordinate</param>
    /// <param name="aStar">CAM16-UCS a* coordinate</param>
    /// <param name="bStar">CAM16-UCS b* coordinate</param>
    private Cam16(double hue, double chroma, double j, double q, double m, double s, double jStar, double aStar,
        double bStar)
    {
        Hue = hue;
        Chroma = chroma;
        J = j;
        Q = q;
        M = m;
        S = s;
        Jstar = jStar;
        Astar = aStar;
        Bstar = bStar;
    }

    /// <summary>Hue in CAM16</summary>
    public double Hue { get; set; }

    /// <summary>Chroma in CAM16</summary>
    public double Chroma { get; set; }

    /// <summary>Lightness in CAM16</summary>
    public double J { get; set; }

    /// <summary>Brightness in CAM16.</summary>
    /// <remarks>
    ///     Prefer lightness, brightness is an absolute quantity. For example, a sheet of white paper is
    ///     much brighter viewed in sunlight than in indoor light, but it is the lightest object under any
    ///     lighting.
    /// </remarks>
    public double Q { get; set; }

    /// <summary>Colorfulness in CAM16.</summary>
    /// <remarks>
    ///     Prefer chroma, colorfulness is an absolute quantity. For example, a yellow toy car is much
    ///     more colorful outside than inside, but it has the same chroma in both environments.
    /// </remarks>
    public double M { get; set; }

    /// <summary>Saturation in CAM16.</summary>
    /// <remarks>
    ///     Colorfulness in proportion to brightness. Prefer chroma, saturation measures colorfulness
    ///     relative to the color's own brightness, where chroma is colorfulness relative to white.
    /// </remarks>
    public double S { get; set; }

    /// <summary>Lightness coordinate in CAM16-UCS</summary>
    public double Jstar { get; set; }

    /// <summary>a* coordinate in CAM16-UCS</summary>
    public double Astar { get; set; }

    /// <summary>b* coordinate in CAM16-UCS</summary>
    public double Bstar { get; set; }

    /// <summary>
    ///     CAM16 instances also have coordinates in the CAM16-UCS space, called J*,
    ///     a*, b*, or jstar, astar, bstar in code. CAM16-UCS is included in the CAM16
    ///     specification, and should be used when measuring distances between colors.
    /// </summary>
    public double Distance(Cam16 other)
    {
        var dJ = Jstar - other.Jstar;
        var dA = Astar - other.Astar;
        var dB = Bstar - other.Bstar;
        var dEPrime = Math.Sqrt(dJ * dJ + dA * dA + dB * dB);
        var dE = 1.41 * Math.Pow(dEPrime, .63);
        return dE;
    }

    /// <summary>
    ///     Create a CAM16 color from a color, assuming the color was viewed in default viewing conditions.
    /// </summary>
    /// <param name="argb">ARGB representation of a color.</param>
    public static Cam16 FromInt(uint argb)
    {
        return FromIntInViewingConditions(argb, ViewingConditions.Default);
    }

    /// <summary>
    ///     Create a CAM16 color from a color in defined viewing conditions.
    /// </summary>
    /// <param name="argb">ARGB representation of a color.</param>
    /// <param name="viewingConditions">Information about the environment where the color was observed.</param>
    public static Cam16 FromIntInViewingConditions(uint argb, ViewingConditions viewingConditions)
    {
        // Transform ARGB int to XYZ
        var red = (argb & 0x00ff0000) >> 16;
        var green = (argb & 0x0000ff00) >> 8;
        var blue = argb & 0x000000ff;
        var redL = ColorUtils.Linearized(red);
        var greenL = ColorUtils.Linearized(green);
        var blueL = ColorUtils.Linearized(blue);
        var x = 0.41233895 * redL + 0.35762064 * greenL + 0.18051042 * blueL;
        var y = 0.2126 * redL + 0.7152 * greenL + 0.0722 * blueL;
        var z = 0.01932141 * redL + 0.11916382 * greenL + 0.95034478 * blueL;

        // Transform XYZ to 'cone'/'rgb' responses
        var matrix = XyzToCam16Rgb;
        var rT = x * matrix[0][0] + y * matrix[0][1] + z * matrix[0][2];
        var gT = x * matrix[1][0] + y * matrix[1][1] + z * matrix[1][2];
        var bT = x * matrix[2][0] + y * matrix[2][1] + z * matrix[2][2];

        // Discount illuminant
        var rD = viewingConditions.RgbD[0] * rT;
        var gD = viewingConditions.RgbD[1] * gT;
        var bD = viewingConditions.RgbD[2] * bT;

        // Chromatic adaptation
        var rAF = Math.Pow(viewingConditions.Fl * Math.Abs(rD) / 100.0, 0.42);
        var gAF = Math.Pow(viewingConditions.Fl * Math.Abs(gD) / 100.0, 0.42);
        var bAF = Math.Pow(viewingConditions.Fl * Math.Abs(bD) / 100.0, 0.42);
        var rA = Math.Sign(rD) * 400.0 * rAF / (rAF + 27.13);
        var gA = Math.Sign(gD) * 400.0 * gAF / (gAF + 27.13);
        var bA = Math.Sign(bD) * 400.0 * bAF / (bAF + 27.13);

        // redness-greenness
        var a = (11.0 * rA + -12.0 * gA + bA) / 11.0;
        // yellowness-blueness
        var b = (rA + gA - 2.0 * bA) / 9.0;

        // auxiliary components
        var u = (20.0 * rA + 20.0 * gA + 21.0 * bA) / 20.0;
        var p2 = (40.0 * rA + 20.0 * gA + bA) / 20.0;

        // hue
        var atan2 = Math.Atan2(b, a);
        var atanDegrees = atan2 * 180.0 / Math.PI;
        var hue =
            atanDegrees < 0
                ? atanDegrees + 360.0
                : atanDegrees >= 360
                    ? atanDegrees - 360.0
                    : atanDegrees;
        var hueRadians = hue * Math.PI / 180.0;

        // achromatic response to color
        var ac = p2 * viewingConditions.Nbb;

        // CAM16 lightness and brightness
        var j = 100.0 * Math.Pow(
            ac / viewingConditions.Aw,
            viewingConditions.C * viewingConditions.Z);
        var q = 4.0
                / viewingConditions.C
                * Math.Sqrt(j / 100.0)
                * (viewingConditions.Aw + 4.0)
                * viewingConditions.FlRoot;

        // CAM16 chroma, colorfulness, and saturation.
        var huePrime = hue < 20.14 ? hue + 360 : hue;
        var eHue = 0.25 * (Math.Cos(MathUtils.ToRadians(huePrime) + 2.0) + 3.8);
        var p1 = 50000.0 / 13.0 * eHue * viewingConditions.Nc * viewingConditions.Ncb;
        var t = p1 * MathUtils.Hypot(a, b) / (u + 0.305);
        var alpha = Math.Pow(1.64 - Math.Pow(0.29, viewingConditions.N), 0.73)
                    * Math.Pow(t, 0.9);

        // CAM16 chroma, colorfulness, saturation
        var c = alpha * Math.Sqrt(j / 100.0);
        var m = c * viewingConditions.FlRoot;
        var s =
            50.0 * Math.Sqrt(alpha * viewingConditions.C / (viewingConditions.Aw + 4.0));

        // CAM16-UCS components
        var jstar = (1.0 + 100.0 * 0.007) * j / (1.0 + 0.007 * j);
        var mstar = 1.0 / 0.0228 * MathUtils.Log1p(0.0228 * m);
        var astar = mstar * Math.Cos(hueRadians);
        var bstar = mstar * Math.Sin(hueRadians);

        return new Cam16(hue, c, j, q, m, s, jstar, astar, bstar);
    }

    /// <param name="j">lightness</param>
    /// <param name="c">chroma</param>
    /// <param name="h">hue</param>
    public static Cam16 FromJch(double j, double c, double h)
    {
        return FromJchInViewingConditions(j, c, h, ViewingConditions.Default);
    }

    /// <param name="j">lightness</param>
    /// <param name="c">chroma</param>
    /// <param name="h">hue</param>
    /// <param name="viewingConditions">Information about the environment where the color was observed.</param>
    public static Cam16 FromJchInViewingConditions(
        double j, double c, double h, ViewingConditions viewingConditions)
    {
        var q =
            4.0
            / viewingConditions.C
            * Math.Sqrt(j / 100.0)
            * (viewingConditions.Aw + 4.0)
            * viewingConditions.FlRoot;
        var m = c * viewingConditions.FlRoot;
        var alpha = c / Math.Sqrt(j / 100.0);
        var s =
            50.0 * Math.Sqrt(alpha * viewingConditions.C / (viewingConditions.Aw + 4.0));

        var hueRadians = h * Math.PI / 180.0;
        var jstar = (1.0 + 100.0 * 0.007) * j / (1.0 + 0.007 * j);
        var mstar = 1.0 / 0.0228 * MathUtils.Log1p(0.0228 * m);
        var astar = mstar * Math.Cos(hueRadians);
        var bstar = mstar * Math.Sin(hueRadians);
        return new Cam16(h, c, j, q, m, s, jstar, astar, bstar);
    }

    /// <summary>
    ///     Create a CAM16 color from CAM16-UCS coordinates.
    /// </summary>
    /// <param name="jstar">CAM16-UCS lightness.</param>
    /// <param name="astar">CAM16-UCS a dimension. Like a* in L*a*b*, it is a Cartesian coordinate on the Y axis.</param>
    /// <param name="bstar">CAM16-UCS b dimension. Like a* in L*a*b*, it is a Cartesian coordinate on the X axis.</param>
    public static Cam16 FromUcs(double jstar, double astar, double bstar)
    {
        return FromUcsInViewingConditions(jstar, astar, bstar, ViewingConditions.Default);
    }

    /// <summary>
    ///     Create a CAM16 color from CAM16-UCS coordinates in defined viewing conditions.
    /// </summary>
    /// <param name="jstar">CAM16-UCS lightness.</param>
    /// <param name="astar">CAM16-UCS a dimension. Like a* in L*a*b*, it is a Cartesian coordinate on the Y axis.</param>
    /// <param name="bstar">CAM16-UCS b dimension. Like a* in L*a*b*, it is a Cartesian coordinate on the X axis.</param>
    /// <param name="viewingConditions">Information about the environment where the color was observed.</param>
    public static Cam16 FromUcsInViewingConditions(
        double jstar, double astar, double bstar, ViewingConditions viewingConditions)
    {
        var m = MathUtils.Hypot(astar, bstar);
        var m2 = MathUtils.Expm1(m * 0.0228) / 0.0228;
        var c = m2 / viewingConditions.FlRoot;
        var h = Math.Atan2(bstar, astar) * (180.0 / Math.PI);
        if (h < 0.0) h += 360.0;
        var j = jstar / (1 - (jstar - 100) * 0.007);
        return FromJchInViewingConditions(j, c, h, viewingConditions);
    }

    /// <summary>
    ///     ARGB representation of the color. Assumes the color was viewed in default viewing conditions,
    ///     which are near-identical to the default viewing conditions for sRGB.
    /// </summary>
    /// <returns></returns>
    public uint ToInt()
    {
        return Viewed(ViewingConditions.Default);
    }

    /// <summary>
    ///     ARGB representation of the color, in defined viewing conditions.
    /// </summary>
    /// <param name="viewingConditions">Information about the environment where the color will be viewed.</param>
    /// <returns>ARGB representation of color</returns>
    public uint Viewed(ViewingConditions viewingConditions)
    {
        var alpha =
            Chroma == 0.0 || J == 0.0
                ? 0.0
                : Chroma / Math.Sqrt(J / 100.0);

        var t =
            Math.Pow(
                alpha / Math.Pow(1.64 - Math.Pow(0.29, viewingConditions.N), 0.73), 1.0 / 0.9);
        var hRad = Hue * Math.PI / 180.0;

        var eHue = 0.25 * (Math.Cos(hRad + 2.0) + 3.8);
        var ac =
            viewingConditions.Aw
            *
            Math.Pow(J / 100.0, 1.0 / viewingConditions.C / viewingConditions.Z);
        var p1 = eHue * (50000.0 / 13.0) * viewingConditions.Nc * viewingConditions.Ncb;
        var p2 = ac / viewingConditions.Nbb;

        var hSin = Math.Sin(hRad);
        var hCos = Math.Cos(hRad);

        var gamma = 23.0 * (p2 + 0.305) * t / (23.0 * p1 + 11.0 * t * hCos + 108.0 * t * hSin);
        var a = gamma * hCos;
        var b = gamma * hSin;
        var rA = (460.0 * p2 + 451.0 * a + 288.0 * b) / 1403.0;
        var gA = (460.0 * p2 - 891.0 * a - 261.0 * b) / 1403.0;
        var bA = (460.0 * p2 - 220.0 * a - 6300.0 * b) / 1403.0;

        var rCBase = Math.Max(0, 27.13 * Math.Abs(rA) / (400.0 - Math.Abs(rA)));
        var rC =
            Math.Sign(rA)
            * (100.0 / viewingConditions.Fl)
            * Math.Pow(rCBase, 1.0 / 0.42);
        var gCBase = Math.Max(0, 27.13 * Math.Abs(gA) / (400.0 - Math.Abs(gA)));
        var gC =
            Math.Sign(gA)
            * (100.0 / viewingConditions.Fl)
            * Math.Pow(gCBase, 1.0 / 0.42);
        var bCBase = Math.Max(0, 27.13 * Math.Abs(bA) / (400.0 - Math.Abs(bA)));
        var bC =
            Math.Sign(bA)
            * (100.0 / viewingConditions.Fl)
            * Math.Pow(bCBase, 1.0 / 0.42);
        var rF = rC / viewingConditions.RgbD[0];
        var gF = gC / viewingConditions.RgbD[1];
        var bF = bC / viewingConditions.RgbD[2];

        var matrix = Cam16RgbToXyz;
        var x = rF * matrix[0][0] + gF * matrix[0][1] + bF * matrix[0][2];
        var y = rF * matrix[1][0] + gF * matrix[1][1] + bF * matrix[1][2];
        var z = rF * matrix[2][0] + gF * matrix[2][1] + bF * matrix[2][2];

        return ColorUtils.ArgbFromXyz(x, y, z);
    }
}