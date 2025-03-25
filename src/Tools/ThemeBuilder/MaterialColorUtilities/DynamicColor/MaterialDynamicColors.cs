using Bdziam.UI.Theming.MaterialColors.ColorSpace;
using Bdziam.UI.Theming.MaterialColors.Dislike;

namespace Bdziam.UI.Theming.MaterialColors.DynamicColor;

public static class MaterialDynamicColors
{
    public static bool IsExtendedFidelity { get; }

    public static DynamicColor HighestSurface(DynamicScheme s)
    {
        return s.IsDark ? SurfaceBright() : SurfaceDim();
    }

    public static DynamicColor PrimaryPaletteKeyColor()
    {
        return DynamicColor.FromPalette(
            "primary_palette_key_color",
            s => s.PrimaryPalette,
            s => s.PrimaryPalette.KeyColor.Tone
        );
    }

    public static DynamicColor SecondaryPaletteKeyColor()
    {
        return DynamicColor.FromPalette(
            "secondary_palette_key_color",
            s => s.SecondaryPalette,
            s => s.SecondaryPalette.KeyColor.Tone
        );
    }

    public static DynamicColor TertiaryPaletteKeyColor()
    {
        return DynamicColor.FromPalette(
            "tertiary_palette_key_color",
            s => s.TertiaryPalette,
            s => s.TertiaryPalette.KeyColor.Tone
        );
    }

    public static DynamicColor NeutralPaletteKeyColor()
    {
        return DynamicColor.FromPalette(
            "neutral_palette_key_color",
            s => s.NeutralPalette,
            s => s.NeutralPalette.KeyColor.Tone
        );
    }

    public static DynamicColor NeutralVariantPaletteKeyColor()
    {
        return DynamicColor.FromPalette(
            "neutral_variant_palette_key_color",
            s => s.NeutralVariantPalette,
            s => s.NeutralVariantPalette.KeyColor.Tone
        );
    }

    public static DynamicColor Background()
    {
        return new DynamicColor(
            "background",
            s => s.NeutralPalette,
            s => s.IsDark ? 6.0 : 98.0,
            true
        );
    }

    public static DynamicColor OnBackground()
    {
        return new DynamicColor(
            "on_background",
            s => s.NeutralPalette,
            s => s.IsDark ? 90.0 : 10.0,
            false,
            s => Background(),
            null,
            new ContrastCurve(3.0, 3.0, 4.5, 7.0)
        );
    }

    public static DynamicColor Surface()
    {
        return new DynamicColor(
            "surface",
            s => s.NeutralPalette,
            s => s.IsDark ? 6.0 : 98.0,
            true
        );
    }

    public static DynamicColor SurfaceDim()
    {
        return new DynamicColor(
            "surface_dim",
            s => s.NeutralPalette,
            s => s.IsDark ? 6.0 : new ContrastCurve(87.0, 87.0, 80.0, 75.0).Get(s.ContrastLevel),
            true
        );
    }

    public static DynamicColor SurfaceBright()
    {
        return new DynamicColor(
            "surface_bright",
            s => s.NeutralPalette,
            s => s.IsDark ? new ContrastCurve(24.0, 24.0, 29.0, 34.0).Get(s.ContrastLevel) : 98.0,
            true
        );
    }

    public static DynamicColor SurfaceContainerLowest()
    {
        return new DynamicColor(
            "surface_container_lowest",
            s => s.NeutralPalette,
            s => s.IsDark ? new ContrastCurve(4.0, 4.0, 2.0, 0.0).Get(s.ContrastLevel) : 100.0,
            true
        );
    }

    public static DynamicColor SurfaceContainerLow()
    {
        return new DynamicColor(
            "surface_container_low",
            s => s.NeutralPalette,
            s =>
                s.IsDark
                    ? new ContrastCurve(10.0, 10.0, 11.0, 12.0).Get(s.ContrastLevel)
                    : new ContrastCurve(96.0, 96.0, 96.0, 95.0).Get(s.ContrastLevel),
            true
        );
    }

    public static DynamicColor SurfaceContainer()
    {
        return new DynamicColor(
            "surface_container",
            s => s.NeutralPalette,
            s =>
                s.IsDark
                    ? new ContrastCurve(12.0, 12.0, 16.0, 20.0).Get(s.ContrastLevel)
                    : new ContrastCurve(94.0, 94.0, 92.0, 90.0).Get(s.ContrastLevel),
            true
        );
    }

    public static DynamicColor SurfaceContainerHigh()
    {
        return new DynamicColor(
            "surface_container_high",
            s => s.NeutralPalette,
            s =>
                s.IsDark
                    ? new ContrastCurve(17.0, 17.0, 21.0, 25.0).Get(s.ContrastLevel)
                    : new ContrastCurve(92.0, 92.0, 88.0, 85.0).Get(s.ContrastLevel),
            true
        );
    }

    public static DynamicColor SurfaceContainerHighest()
    {
        return new DynamicColor(
            "surface_container_highest",
            s => s.NeutralPalette,
            s =>
                s.IsDark
                    ? new ContrastCurve(22.0, 22.0, 26.0, 30.0).Get(s.ContrastLevel)
                    : new ContrastCurve(90.0, 90.0, 84.0, 80.0).Get(s.ContrastLevel),
            true
        );
    }

    public static DynamicColor OnSurface()
    {
        return new DynamicColor(
            "on_surface",
            s => s.NeutralPalette,
            s => s.IsDark ? 90.0 : 10.0,
            false,
            s => HighestSurface(s),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor SurfaceVariant()
    {
        return new DynamicColor(
            "surface_variant",
            s => s.NeutralVariantPalette,
            s => s.IsDark ? 30.0 : 90.0,
            true
        );
    }

    public static DynamicColor OnSurfaceVariant()
    {
        return new DynamicColor(
            "on_surface_variant",
            s => s.NeutralVariantPalette,
            s => s.IsDark ? 80.0 : 30.0,
            false,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor InverseSurface()
    {
        return new DynamicColor(
            "inverse_surface",
            s => s.NeutralPalette,
            s => s.IsDark ? 90.0 : 20.0,
            false
        );
    }

    public static DynamicColor InverseOnSurface()
    {
        return new DynamicColor(
            "inverse_on_surface",
            s => s.NeutralPalette,
            s => s.IsDark ? 20.0 : 95.0,
            false,
            s => InverseSurface(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor Outline()
    {
        return new DynamicColor(
            "outline",
            s => s.NeutralVariantPalette,
            s => s.IsDark ? 60.0 : 50.0,
            false,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.5, 3.0, 4.5, 7.0)
        );
    }

    public static DynamicColor OutlineVariant()
    {
        return new DynamicColor(
            "outline_variant",
            s => s.NeutralVariantPalette,
            s => s.IsDark ? 30.0 : 80.0,
            false,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5)
        );
    }

    public static DynamicColor Shadow()
    {
        return new DynamicColor(
            "shadow",
            s => s.NeutralPalette,
            s => 0.0,
            false
        );
    }

    public static DynamicColor Scrim()
    {
        return new DynamicColor(
            "scrim",
            s => s.NeutralPalette,
            s => 0.0,
            false
        );
    }

    public static DynamicColor SurfaceTint()
    {
        return new DynamicColor(
            "surface_tint",
            s => s.PrimaryPalette,
            s => s.IsDark ? 80.0 : 40.0,
            true
        );
    }

    public static DynamicColor Primary()
    {
        return new DynamicColor(
            "primary",
            s => s.PrimaryPalette,
            s => IsMonochrome(s) ? s.IsDark ? 100.0 : 0.0 : s.IsDark ? 80.0 : 40.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s => new ToneDeltaPair(PrimaryContainer(), Primary(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnPrimary()
    {
        return new DynamicColor(
            "on_primary",
            s => s.PrimaryPalette,
            s => IsMonochrome(s) ? s.IsDark ? 10.0 : 90.0 : s.IsDark ? 20.0 : 100.0,
            false,
            s => Primary(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor PrimaryContainer()
    {
        return new DynamicColor(
            "primary_container",
            s => s.PrimaryPalette,
            s =>
            {
                if (IsFidelity(s)) return s.SourceColorHct.Tone;

                if (IsMonochrome(s)) return s.IsDark ? 85.0 : 25.0;

                return s.IsDark ? 30.0 : 90.0;
            },
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(PrimaryContainer(), Primary(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnPrimaryContainer()
    {
        return new DynamicColor(
            "on_primary_container",
            s => s.PrimaryPalette,
            s =>
            {
                if (IsFidelity(s)) return DynamicColor.ForegroundTone(PrimaryContainer().Tone(s), 4.5);

                if (IsMonochrome(s)) return s.IsDark ? 0.0 : 100.0;

                return s.IsDark ? 90.0 : 30.0;
            },
            false,
            s => PrimaryContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor InversePrimary()
    {
        return new DynamicColor(
            "inverse_primary",
            s => s.PrimaryPalette,
            s => s.IsDark ? 40.0 : 80.0,
            false,
            s => InverseSurface(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0)
        );
    }

    public static DynamicColor Secondary()
    {
        return new DynamicColor(
            "secondary",
            s => s.SecondaryPalette,
            s => s.IsDark ? 80.0 : 40.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s =>
                new ToneDeltaPair(SecondaryContainer(), Secondary(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnSecondary()
    {
        return new DynamicColor(
            "on_secondary",
            s => s.SecondaryPalette,
            s =>
            {
                if (IsMonochrome(s))
                    return s.IsDark ? 10.0 : 100.0;
                return s.IsDark ? 20.0 : 100.0;
            },
            false,
            s => Secondary(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor SecondaryContainer()
    {
        return new DynamicColor(
            "secondary_container",
            s => s.SecondaryPalette,
            s =>
            {
                var initialTone = s.IsDark ? 30.0 : 90.0;
                if (IsMonochrome(s)) return s.IsDark ? 30.0 : 85.0;

                if (!IsFidelity(s)) return initialTone;

                return FindDesiredChromaByTone(
                    s.SecondaryPalette.Hue, s.SecondaryPalette.Chroma, initialTone, !s.IsDark
                );
            },
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(SecondaryContainer(), Secondary(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnSecondaryContainer()
    {
        return new DynamicColor(
            "on_secondary_container",
            s => s.SecondaryPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 90.0 : 10.0;

                if (!IsFidelity(s)) return s.IsDark ? 90.0 : 30.0;

                return DynamicColor.ForegroundTone(SecondaryContainer().Tone(s), 4.5);
            },
            false,
            s => SecondaryContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor Tertiary()
    {
        return new DynamicColor(
            "tertiary",
            s => s.TertiaryPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 90.0 : 25.0;

                return s.IsDark ? 80.0 : 40.0;
            },
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s => new ToneDeltaPair(TertiaryContainer(), Tertiary(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnTertiary()
    {
        return new DynamicColor(
            "on_tertiary",
            s => s.TertiaryPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 10.0 : 90.0;

                return s.IsDark ? 20.0 : 100.0;
            },
            false,
            s => Tertiary(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor TertiaryContainer()
    {
        return new DynamicColor(
            "tertiary_container",
            s => s.TertiaryPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 60.0 : 49.0;

                if (!IsFidelity(s)) return s.IsDark ? 30.0 : 90.0;

                var proposedHct = s.TertiaryPalette[s.SourceColorHct.Tone];
                return DislikeAnalyzer.FixIfDisliked(Hct.FromInt(proposedHct)).Tone;
            },
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(TertiaryContainer(), Tertiary(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnTertiaryContainer()
    {
        return new DynamicColor(
            "on_tertiary_container",
            s => s.TertiaryPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 0.0 : 100.0;

                if (!IsFidelity(s)) return s.IsDark ? 90.0 : 30.0;

                return DynamicColor.ForegroundTone(TertiaryContainer().Tone(s), 4.5);
            },
            false,
            s => TertiaryContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor Error()
    {
        return new DynamicColor(
            "error",
            s => s.ErrorPalette,
            s => s.IsDark ? 80.0 : 40.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s => new ToneDeltaPair(ErrorContainer(), Error(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor ErrorContainer()
    {
        return new DynamicColor(
            "error_container",
            s => s.ErrorPalette,
            s => s.IsDark ? 30.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(ErrorContainer(), Error(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnErrorContainer()
    {
        return new DynamicColor(
            "on_error_container",
            s => s.ErrorPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 90.0 : 10.0;

                return s.IsDark ? 90.0 : 30.0;
            },
            false,
            s => ErrorContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor OnError()
    {
        return new DynamicColor(
            "on_error",
            s => s.ErrorPalette,
            s => s.IsDark ? 20.0 : 100.0,
            false,
            s => Error(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor Success()
    {
        return new DynamicColor(
            "success",
            s => s.SuccessPalette,
            s => s.IsDark ? 80.0 : 40.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s => new ToneDeltaPair(SuccessContainer(), Success(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor SuccessContainer()
    {
        return new DynamicColor(
            "success_container",
            s => s.SuccessPalette,
            s => s.IsDark ? 30.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(SuccessContainer(), Success(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnSuccessContainer()
    {
        return new DynamicColor(
            "on_success_container",
            s => s.ErrorPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 90.0 : 10.0;

                return s.IsDark ? 90.0 : 30.0;
            },
            false,
            s => SuccessContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor OnSuccess()
    {
        return new DynamicColor(
            "on_success",
            s => s.SuccessPalette,
            s => s.IsDark ? 20.0 : 100.0,
            false,
            s => Success(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor Info()
    {
        return new DynamicColor(
            "info",
            s => s.InfoPalette,
            s => s.IsDark ? 80.0 : 40.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s => new ToneDeltaPair(InfoContainer(), Info(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor InfoContainer()
    {
        return new DynamicColor(
            "info_container",
            s => s.InfoPalette,
            s => s.IsDark ? 30.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(InfoContainer(), Info(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnInfoContainer()
    {
        return new DynamicColor(
            "on_info_container",
            s => s.InfoPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 90.0 : 10.0;

                return s.IsDark ? 90.0 : 30.0;
            },
            false,
            s => InfoContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor OnInfo()
    {
        return new DynamicColor(
            "on_info",
            s => s.InfoPalette,
            s => s.IsDark ? 20.0 : 100.0,
            false,
            s => Info(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor Warning()
    {
        return new DynamicColor(
            "warning",
            s => s.WarningPalette,
            s => s.IsDark ? 80.0 : 40.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 7.0),
            s => new ToneDeltaPair(WarningContainer(), Warning(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor WarningContainer()
    {
        return new DynamicColor(
            "warning_container",
            s => s.WarningPalette,
            s => s.IsDark ? 30.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(WarningContainer(), Warning(), 10.0, TonePolarity.Nearer, false)
        );
    }

    public static DynamicColor OnWarningContainer()
    {
        return new DynamicColor(
            "on_warning_container",
            s => s.WarningPalette,
            s =>
            {
                if (IsMonochrome(s)) return s.IsDark ? 90.0 : 10.0;

                return s.IsDark ? 90.0 : 30.0;
            },
            false,
            s => WarningContainer(),
            null,
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor OnWarning()
    {
        return new DynamicColor(
            "on_warning",
            s => s.WarningPalette,
            s => s.IsDark ? 20.0 : 100.0,
            false,
            s => Warning(),
            null,
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor PrimaryFixed()
    {
        return new DynamicColor(
            "primary_fixed",
            s => s.PrimaryPalette,
            s => IsMonochrome(s) ? 40.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(PrimaryFixed(), PrimaryFixedDim(), 10.0, TonePolarity.Lighter, true)
        );
    }

    public static DynamicColor PrimaryFixedDim()
    {
        return new DynamicColor(
            "primary_fixed_dim",
            s => s.PrimaryPalette,
            s => IsMonochrome(s) ? 30.0 : 80.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s => new ToneDeltaPair(PrimaryFixed(), PrimaryFixedDim(), 10.0, TonePolarity.Lighter, true)
        );
    }

    public static DynamicColor OnPrimaryFixed()
    {
        return new DynamicColor(
            "on_primary_fixed",
            s => s.PrimaryPalette,
            s => IsMonochrome(s) ? 100.0 : 10.0,
            false,
            s => PrimaryFixedDim(),
            s => PrimaryFixed(),
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor OnPrimaryFixedVariant()
    {
        return new DynamicColor(
            "on_primary_fixed_variant",
            s => s.PrimaryPalette,
            s => IsMonochrome(s) ? 90.0 : 30.0,
            false,
            s => PrimaryFixedDim(),
            s => PrimaryFixed(),
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor SecondaryFixed()
    {
        return new DynamicColor(
            "secondary_fixed",
            s => s.SecondaryPalette,
            s => IsMonochrome(s) ? 80.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s =>
                new ToneDeltaPair(SecondaryFixed(), SecondaryFixedDim(), 10.0, TonePolarity.Lighter, true)
        );
    }

    public static DynamicColor SecondaryFixedDim()
    {
        return new DynamicColor(
            "secondary_fixed_dim",
            s => s.SecondaryPalette,
            s => IsMonochrome(s) ? 70.0 : 80.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s =>
                new ToneDeltaPair(SecondaryFixed(), SecondaryFixedDim(), 10.0, TonePolarity.Lighter, true)
        );
    }

    public static DynamicColor OnSecondaryFixed()
    {
        return new DynamicColor(
            "on_secondary_fixed",
            s => s.SecondaryPalette,
            s => 10.0,
            false,
            s => SecondaryFixedDim(),
            s => SecondaryFixed(),
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor OnSecondaryFixedVariant()
    {
        return new DynamicColor(
            "on_secondary_fixed_variant",
            s => s.SecondaryPalette,
            s => IsMonochrome(s) ? 25.0 : 30.0,
            false,
            s => SecondaryFixedDim(),
            s => SecondaryFixed(),
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor TertiaryFixed()
    {
        return new DynamicColor(
            "tertiary_fixed",
            s => s.TertiaryPalette,
            s => IsMonochrome(s) ? 40.0 : 90.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s =>
                new ToneDeltaPair(TertiaryFixed(), TertiaryFixedDim(), 10.0, TonePolarity.Lighter, true)
        );
    }

    public static DynamicColor TertiaryFixedDim()
    {
        return new DynamicColor(
            "tertiary_fixed_dim",
            s => s.TertiaryPalette,
            s => IsMonochrome(s) ? 30.0 : 80.0,
            true,
            s => HighestSurface(s),
            null,
            new ContrastCurve(1.0, 1.0, 3.0, 4.5),
            s =>
                new ToneDeltaPair(TertiaryFixed(), TertiaryFixedDim(), 10.0, TonePolarity.Lighter, true)
        );
    }

    public static DynamicColor OnTertiaryFixed()
    {
        return new DynamicColor(
            "on_tertiary_fixed",
            s => s.TertiaryPalette,
            s => IsMonochrome(s) ? 100.0 : 10.0,
            false,
            s => TertiaryFixedDim(),
            s => TertiaryFixed(),
            new ContrastCurve(4.5, 7.0, 11.0, 21.0)
        );
    }

    public static DynamicColor OnTertiaryFixedVariant()
    {
        return new DynamicColor(
            "on_tertiary_fixed_variant",
            s => s.TertiaryPalette,
            s => IsMonochrome(s) ? 90.0 : 30.0,
            false,
            s => TertiaryFixedDim(),
            s => TertiaryFixed(),
            new ContrastCurve(3.0, 4.5, 7.0, 11.0)
        );
    }

    public static DynamicColor ControlActivated()
    {
        return DynamicColor.FromPalette(
            "control_activated", s => s.PrimaryPalette, s => s.IsDark ? 30.0 : 90.0
        );
    }

    public static DynamicColor ControlNormal()
    {
        return DynamicColor.FromPalette(
            "control_normal", s => s.NeutralVariantPalette, s => s.IsDark ? 80.0 : 30.0
        );
    }

    public static DynamicColor ControlHighlight()
    {
        return new DynamicColor(
            "control_highlight",
            s => s.NeutralPalette,
            s => s.IsDark ? 100.0 : 0.0,
            false,
            null,
            null,
            null,
            null,
            s => s.IsDark ? 0.20 : 0.12
        );
    }

    public static DynamicColor TextPrimaryInverse()
    {
        return DynamicColor.FromPalette(
            "text_primary_inverse",
            s => s.NeutralPalette,
            s => s.IsDark ? 10.0 : 90.0
        );
    }

    public static DynamicColor TextSecondaryAndTertiaryInverse()
    {
        return DynamicColor.FromPalette(
            "text_secondary_and_tertiary_inverse",
            s => s.NeutralVariantPalette,
            s => s.IsDark ? 30.0 : 80.0
        );
    }

    public static DynamicColor TextPrimaryInverseDisableOnly()
    {
        return DynamicColor.FromPalette(
            "text_primary_inverse_disable_only",
            s => s.NeutralPalette,
            s => s.IsDark ? 10.0 : 90.0
        );
    }

    public static DynamicColor TextSecondaryAndTertiaryInverseDisabled()
    {
        return DynamicColor.FromPalette(
            "text_secondary_and_tertiary_inverse_disabled",
            s => s.NeutralPalette,
            s => s.IsDark ? 10.0 : 90.0
        );
    }

    public static DynamicColor TextHintInverse()
    {
        return DynamicColor.FromPalette(
            "text_hint_inverse",
            s => s.NeutralPalette,
            s => s.IsDark ? 10.0 : 90.0
        );
    }

    private static bool IsFidelity(DynamicScheme scheme)
    {
        if (IsExtendedFidelity &&
            scheme.Variant != DynamicSchemeVariant.Monochrome &&
            scheme.Variant != DynamicSchemeVariant.Neutral)
            return true;

        return scheme.Variant == DynamicSchemeVariant.Fidelity || scheme.Variant == DynamicSchemeVariant.Content;
    }

    private static bool IsMonochrome(DynamicScheme scheme)
    {
        return scheme.Variant == DynamicSchemeVariant.Monochrome;
    }

    public static double FindDesiredChromaByTone(double hue, double chroma, double tone, bool byDecreasingTone)
    {
        var answer = tone;

        var closestToChroma = Hct.From(hue, chroma, tone);
        if (closestToChroma.Chroma < chroma)
        {
            var chromaPeak = closestToChroma.Chroma;
            while (closestToChroma.Chroma < chroma)
            {
                answer += byDecreasingTone ? -1.0 : 1.0;
                var potentialSolution = Hct.From(hue, chroma, answer);
                if (chromaPeak > potentialSolution.Chroma) break;

                if (Math.Abs(potentialSolution.Chroma - chroma) < 0.4) break;

                var potentialDelta = Math.Abs(potentialSolution.Chroma - chroma);
                var currentDelta = Math.Abs(closestToChroma.Chroma - chroma);
                if (potentialDelta < currentDelta) closestToChroma = potentialSolution;

                chromaPeak = Math.Max(chromaPeak, potentialSolution.Chroma);
            }
        }

        return answer;
    }
}