using Bdziam.UI.Theming.MaterialColors.ColorSpace;

namespace Bdziam.UI.Theming.MaterialColors.Dislike;

/*
 * Copyright 2022 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * Check and/or fix universally disliked colors.
 * <p>
 *     Color science studies of color preference indicate universal distaste for dark yellow-greens,
 *     and also show this is correlated to distate for biological waste and rotting food.
 *     <p>
 *         See Palmer and Schloss, 2010 or Schloss and Palmer's Chapter 21 in Handbook of Color
 *         Psychology (2015).
 */
public static class DislikeAnalyzer
{
    /**
     * Returns true if color is disliked.
     * <p>Disliked is defined as a dark yellow-green that is not neutral.
     */
    public static bool isDisliked(Hct hct)
    {
        var huePasses = Math.Round(hct.Hue) >= 90.0 && Math.Round(hct.Hue) <= 111.0;
        var chromaPasses = Math.Round(hct.Chroma) > 16.0;
        var tonePasses = hct.Tone < 65;

        return huePasses && chromaPasses && tonePasses;
    }

    /**
     * If color is disliked, lighten it to make it likable.
     */
    public static Hct FixIfDisliked(Hct hct)
    {
        if (isDisliked(hct)) return Hct.From(hct.Hue, hct.Chroma, 70.0);

        return hct;
    }
}