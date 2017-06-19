using System.Collections.Generic;

using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;

using Com.Telerik.Widget.Chart.Visualization.PieChart;

namespace MVP.App.Common.Charting
{
    internal static class ChartHelpers
    {
        public static List<SliceStyle> GeneratePieSlices(Color[] colors)
        {
            var sliceStyles = new List<SliceStyle>();

            foreach (var color in colors)
            {
                sliceStyles.Add(new SliceStyle
                {
                    FillColor = color,
                    ArcColor = Color.White,
                    ArcWidth = 2,
                    StrokeColor = color,
                    StrokeWidth = 2
                });
            }

            return sliceStyles;
        }

        public static Color[] GetColors(Context context, bool useSampleColors = false)
        {
            if (context == null || useSampleColors)
                return new[] { Color.Rgb(0, 0, 255), Color.Rgb(0, 255, 0), Color.Rgb(255, 0, 0), };

            return new[]
            {
                new Color(ContextCompat.GetColor(context, Resource.Color.MvpBlueColor)),
                new Color(ContextCompat.GetColor(context, Resource.Color.MvpBlueLightColor)),
                new Color(ContextCompat.GetColor(context, Resource.Color.MvpBlueDarkColor)),
                new Color(ContextCompat.GetColor(context, Resource.Color.MvpBlueLighterColor))
            };
        }
    }
}