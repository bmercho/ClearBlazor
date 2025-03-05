using Bdziam.UI.Theming.MaterialColors.Scheme;

namespace ThemeBuilder
{
    public static class CreateColorSchemes
    {
        public static void CreateColorSchemeFiles(ContentScheme colorScheme, string name, 
                                                  string folder)
        { 
            var filename = $@"{folder}\{name}";
            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                sw.WriteLine("namespace ClearBlazor");
                sw.WriteLine("{");
                if (name.Contains("Light"))
                    sw.WriteLine("    public class LightColourScheme : IColorScheme");
                else
                    sw.WriteLine("    public class DarkColourScheme : IColorScheme");
                sw.WriteLine("    {");

                sw.WriteLine(GetColorLine("Primary", colorScheme.Primary));
                sw.WriteLine(GetColorLine("Secondary", colorScheme.Secondary));
                sw.WriteLine(GetColorLine("Tertiary", colorScheme.Tertiary));
                sw.WriteLine(GetColorLine("Error", colorScheme.Error));
                sw.WriteLine(GetColorLine("Info", colorScheme.Info));
                sw.WriteLine(GetColorLine("Success", colorScheme.Success));
                sw.WriteLine(GetColorLine("Warning", colorScheme.Warning));
                sw.WriteLine(GetColorLine("OnPrimary", colorScheme.OnPrimary));
                sw.WriteLine(GetColorLine("OnSecondary", colorScheme.OnSecondary));
                sw.WriteLine(GetColorLine("OnTertiary", colorScheme.OnTertiary));
                sw.WriteLine(GetColorLine("OnError", colorScheme.OnError));
                sw.WriteLine(GetColorLine("OnInfo", colorScheme.OnInfo));
                sw.WriteLine(GetColorLine("OnSuccess", colorScheme.OnSuccess));
                sw.WriteLine(GetColorLine("OnWarning", colorScheme.OnWarning));
                sw.WriteLine(GetColorLine("PrimaryContainer", colorScheme.PrimaryContainer));
                sw.WriteLine(GetColorLine("SecondaryContainer", colorScheme.SecondaryContainer));
                sw.WriteLine(GetColorLine("TertiaryContainer", colorScheme.TertiaryContainer));
                sw.WriteLine(GetColorLine("ErrorContainer", colorScheme.ErrorContainer));
                sw.WriteLine(GetColorLine("InfoContainer", colorScheme.InfoContainer));
                sw.WriteLine(GetColorLine("SuccessContainer", colorScheme.SuccessContainer));
                sw.WriteLine(GetColorLine("WarningContainer", colorScheme.WarningContainer));
                sw.WriteLine(GetColorLine("OnPrimaryContainer", colorScheme.OnPrimaryContainer));
                sw.WriteLine(GetColorLine("OnSecondaryContainer", colorScheme.OnSecondaryContainer));
                sw.WriteLine(GetColorLine("OnTertiaryContainer", colorScheme.OnTertiaryContainer));
                sw.WriteLine(GetColorLine("OnErrorContainer", colorScheme.OnErrorContainer));
                sw.WriteLine(GetColorLine("OnInfoContainer", colorScheme.OnInfoContainer));
                sw.WriteLine(GetColorLine("OnSuccessContainer", colorScheme.OnSuccessContainer));
                sw.WriteLine(GetColorLine("OnWarningContainer", colorScheme.OnWarningContainer));
                sw.WriteLine(GetColorLine("PrimaryFixed", colorScheme.PrimaryFixed));
                sw.WriteLine(GetColorLine("PrimaryFixedDim", colorScheme.PrimaryFixedDim));
                sw.WriteLine(GetColorLine("SecondaryFixed", colorScheme.SecondaryFixed));
                sw.WriteLine(GetColorLine("SecondaryFixedDim", colorScheme.SecondaryFixedDim));
                sw.WriteLine(GetColorLine("TertiaryFixed", colorScheme.TertiaryFixed));
                sw.WriteLine(GetColorLine("TertiaryFixedDim", colorScheme.TertiaryFixedDim));
                sw.WriteLine(GetColorLine("OnPrimaryFixed", colorScheme.OnPrimaryFixed));
                sw.WriteLine(GetColorLine("OnPrimaryFixedVariant", colorScheme.OnPrimaryFixedVariant));
                sw.WriteLine(GetColorLine("OnSecondaryFixed", colorScheme.OnSecondaryFixed));
                sw.WriteLine(GetColorLine("OnSecondaryFixedVariant", colorScheme.OnSecondaryFixedVariant));
                sw.WriteLine(GetColorLine("OnTertiaryFixed", colorScheme.OnTertiaryFixed));
                sw.WriteLine(GetColorLine("OnTertiaryFixedVariant", colorScheme.OnTertiaryFixedVariant));
                sw.WriteLine(GetColorLine("SurfaceDim", colorScheme.SurfaceDim));
                sw.WriteLine(GetColorLine("Surface", colorScheme.Surface));
                sw.WriteLine(GetColorLine("SurfaceBright", colorScheme.SurfaceBright));
                sw.WriteLine(GetColorLine("InverseSurface", colorScheme.InverseSurface));
                sw.WriteLine(GetColorLine("OnInverseSurface", colorScheme.InverseOnSurface));
                sw.WriteLine(GetColorLine("InversePrimary", colorScheme.InversePrimary));
                sw.WriteLine(GetColorLine("SurfaceContainerLowest", colorScheme.SurfaceContainerLowest));
                sw.WriteLine(GetColorLine("SurfaceContainerLow", colorScheme.SurfaceContainerLow));
                sw.WriteLine(GetColorLine("SurfaceContainer", colorScheme.SurfaceContainer));
                sw.WriteLine(GetColorLine("SurfaceContainerHigh", colorScheme.SurfaceContainerHigh));
                sw.WriteLine(GetColorLine("SurfaceContainerHighest", colorScheme.SurfaceContainerHighest));
                sw.WriteLine(GetColorLine("OnSurface", colorScheme.OnSurface));
                sw.WriteLine(GetColorLine("OnSurfaceVariant", colorScheme.OnSurfaceVariant));
                sw.WriteLine(GetColorLine("Outline", colorScheme.Outline));
                sw.WriteLine(GetColorLine("OutlineVariant", colorScheme.OutlineVariant));
                sw.WriteLine(GetColorLine("Scrim", colorScheme.Scrim));
                sw.WriteLine(GetColorLine("Shadow", colorScheme.Shadow));

                sw.WriteLine("    }");
                sw.WriteLine("}");
            }
        }

        private static string GetColorLine(string name, uint color)
        {
            return $"        public Color {name} => new Color({GetColorString(color)});";
        }

        private static string GetColorString(uint color)
        {
            var colorString = color.ToString("X");
            var alpha = colorString.Substring(0, 2);

            return $"\"#{colorString.Substring(2, 6) + alpha}\"";
        }
    }
}
