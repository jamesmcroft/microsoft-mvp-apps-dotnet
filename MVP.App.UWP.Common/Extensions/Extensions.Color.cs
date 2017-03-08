namespace MVP.App
{
    using System;

    using Windows.UI;

    public static partial class Extensions
    {
        /// <summary>
        /// Gets a value indicating the perceived brightness of a color.
        /// </summary>
        /// <param name="color">
        /// The color to check.
        /// </param>
        /// <returns>
        /// THe perceived brightness as an integer.
        /// </returns>
        public static int PerceivedBrightness(this Color color)
        {
            return (int)Math.Sqrt((color.R * color.R * .299) + (color.G * color.G * .587) + (color.B * color.B * .114));
        }
    }
}