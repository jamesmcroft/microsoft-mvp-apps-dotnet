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
            if (color.A != 255)
            {
                color = color.AlphaBlend(Colors.White);
            }

            return (int)Math.Sqrt((color.R * color.R * .299) + (color.G * color.G * .587) + (color.B * color.B * .114));
        }

        /// <summary>
        /// Blends the given color with an alpha layer with the given background color.
        /// </summary>
        /// <param name="color">
        /// The color with alpha.
        /// </param>
        /// <param name="background">
        /// The background to blend with.
        /// </param>
        /// <returns>
        /// Returns the blended color.
        /// </returns>
        public static Color AlphaBlend(this Color color, Color background)
        {
            byte r = (byte)Math.Round((((1 - (color.A / 255.0)) * (background.R / 255.0)) + ((color.A / 255.0) * (color.R / 255.0))) * 255.0);
            byte g = (byte)Math.Round((((1 - (color.A / 255.0)) * (background.G / 255.0)) + ((color.A / 255.0) * (color.G / 255.0))) * 255.0);
            byte b = (byte)Math.Round((((1 - (color.A / 255.0)) * (background.B / 255.0)) + ((color.A / 255.0) * (color.B / 255.0))) * 255.0);

            return new Color { A = 255, R = r, B = b, G = g };
        }
    }
}