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

namespace Bdziam.UI.Theming.MaterialColors.Palettes;

/// <summary>
///     A convenience class for retrieving colors that are constant in hue and
///     chroma, but vary in tone.
/// </summary>
public class TonalPalette
{
    protected Dictionary<uint, uint> _cache = new();

    private TonalPalette(double hue, double chroma, Hct keyColor)
    {
        _cache = new Dictionary<uint, uint>();
        Hue = hue;
        Chroma = chroma;
        KeyColor = keyColor;
    }

    public double Hue { get; protected set; }
    public double Chroma { get; protected set; }
    public Hct KeyColor { get; protected set; }


    /// <summary>Creates an ARGB color with HCT hue and chroma of this TonalPalette instance, and the provided HCT tone.</summary>
    /// <param name="tone">HCT tone, measured from 0 to 100.</param>
    /// <returns>ARGB representation of a color with that tone.</returns>
    public uint this[uint tone] => Tone(tone);

    /**
     * Create tones using the HCT hue and chroma from a color.
     * 
     * @param argb ARGB representation of a color
     * @return Tones matching that color's hue and chroma.
     */
    public static TonalPalette FromInt(uint argb)
    {
        return FromHct(Hct.FromInt(argb));
    }

    /**
     * Create tones using a HCT color.
     * 
     * @param hct HCT representation of a color.
     * @return Tones matching that color's hue and chroma.
     */
    public static TonalPalette FromHct(Hct hct)
    {
        return new TonalPalette(hct.Hue, hct.Chroma, hct);
    }

    /**
     * Create tones from a defined HCT hue and chroma.
     * 
     * @param hue HCT hue
     * @param chroma HCT chroma
     * @return Tones matching hue and chroma.
     */
    public static TonalPalette FromHueAndChroma(double hue, double chroma)
    {
        var keyColor = new KeyColor(hue, chroma).Create();
        return new TonalPalette(hue, chroma, keyColor);
    }

    /**
     * Create an ARGB color with HCT hue and chroma of this Tones instance, and the provided HCT tone.
     * 
     * @param tone HCT tone, measured from 0 to 100.
     * @return ARGB representation of a color with that tone.
     */
    public uint Tone(uint tone)
    {
        if (!_cache.TryGetValue(tone, out var color))
        {
            color = Hct.From(Hue, Chroma, tone).ToInt();
            _cache.Add(tone, color);
        }

        return color;
    }
}