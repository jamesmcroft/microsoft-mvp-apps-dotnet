namespace MVP.App
{
    using System;

    using Windows.UI;

    public static partial class Extensions
    {
        public static int PerceivedBrightness(this Color color)
        {
            return (int)Math.Sqrt((color.R * color.R * .299) + (color.G * color.G * .587) + (color.B * color.B * .114));
        }
    }
}