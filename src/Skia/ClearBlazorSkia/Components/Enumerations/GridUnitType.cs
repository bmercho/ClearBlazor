namespace ClearBlazor
{
    [System.Flags]
    public enum GridUnitType:byte
    {
        None = 0x00,
        Pixel = 0x01, 
        Star = 0x02, 
        Auto = 0x04
    }
}
