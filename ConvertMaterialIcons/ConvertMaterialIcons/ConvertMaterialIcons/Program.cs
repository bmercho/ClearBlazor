using ConvertMaterialIcons;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string _destinationFolder = "C:\\Work\\ClearBlazor\\ConvertMaterialIcons\\MaterialIconFiles";

        await ConvertIcons.Convert(_destinationFolder);  
    }
}