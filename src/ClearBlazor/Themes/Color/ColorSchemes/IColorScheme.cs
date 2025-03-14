namespace ClearBlazor
{
    public interface IColorScheme
    {
        Color Primary { get; }
        Color Secondary { get; }
        Color Tertiary { get; }
        Color Error { get; }
        Color Info { get; }
        Color Success { get; }
        Color Warning { get; }
        Color OnPrimary { get; }
        Color OnSecondary { get; }
        Color OnTertiary { get; }
        Color OnError { get; }
        Color OnInfo { get; }
        Color OnSuccess { get; }
        Color OnWarning { get; }
        Color PrimaryContainer { get; }
        Color SecondaryContainer { get; }
        Color TertiaryContainer { get; }
        Color ErrorContainer { get; }
        Color InfoContainer { get; }
        Color SuccessContainer { get; }
        Color WarningContainer { get; }
        Color OnPrimaryContainer { get; }
        Color OnSecondaryContainer { get; }
        Color OnTertiaryContainer { get; }
        Color OnErrorContainer { get; }
        Color OnInfoContainer { get; }
        Color OnSuccessContainer { get; }
        Color OnWarningContainer { get; }
        Color PrimaryFixed { get; }
        Color PrimaryFixedDim { get; }
        Color SecondaryFixed { get; }
        Color SecondaryFixedDim { get; }
        Color TertiaryFixed { get; }
        Color TertiaryFixedDim { get; }
        Color OnPrimaryFixed { get; }
        Color OnPrimaryFixedVariant { get; }
        Color OnSecondaryFixed { get; }
        Color OnSecondaryFixedVariant { get; }
        Color OnTertiaryFixed { get; }
        Color OnTertiaryFixedVariant { get; }
        Color SurfaceDim { get; }
        Color Surface { get; }
        Color SurfaceBright { get; }
        Color InverseSurface { get; }
        Color OnInverseSurface { get; }
        Color InversePrimary { get; }
        Color SurfaceContainerLowest { get; }
        Color SurfaceContainerLow { get; }
        Color SurfaceContainer { get; }
        Color SurfaceContainerHigh { get; }
        Color SurfaceContainerHighest { get; }
        Color OnSurface { get; }
        Color OnSurfaceVariant { get; }
        Color Outline { get; }
        Color OutlineVariant { get; }
        Color Scrim { get; }
        Color Shadow { get; }


        // To be deleted
        Color BackgroundDisabled { get; }
        Color GrayLight { get; }
        Color Dark { get; }

        Color TextDisabled { get; }

        Color ToolTipTextColor { get; }

        Color ListBackgroundColor { get; }
        Color ListSelectedColor { get; }

        Color GrayLighter { get; }

        Color BackgroundGrey { get; }

        Color Background { get; }
    }
}
