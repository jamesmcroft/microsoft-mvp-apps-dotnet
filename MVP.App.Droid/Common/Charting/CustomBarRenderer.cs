using System;

using Android.Graphics;
using Android.Runtime;

using Com.Telerik.Android.Common;
using Com.Telerik.Android.Common.Math;
using Com.Telerik.Widget.Chart.Engine.DataPoints;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Pointrenderers;

namespace MVP.App.Common.Charting
{
    public class CustomBarRenderer : BarPointRenderer
    {
        private Color[] colors;

        public CustomBarRenderer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        // extra param added for colors
        public CustomBarRenderer(BarSeries p0, Color[] colors) : base(p0)
        {
            this.colors = colors;
        }

        protected override void RenderPointCore(Canvas canvas, DataPoint point)
        {
            RadRect layoutSlot = point.LayoutSlot;

            if (Math.Abs(layoutSlot.Height) < 0.1 || Math.Abs(layoutSlot.Width) < 0.1)
                return;

            BarSeries series = this.Series as BarSeries;

            if (series == null)
                return;

            int colorIndex = point.CollectionIndex() % colors.Length;

            Paint strokePaint = new Paint();
            strokePaint.SetStyle(Paint.Style.Stroke);
            strokePaint.Color = colors[colorIndex];
            strokePaint.StrokeWidth = series.StrokeWidth;

            Paint fillPaint = new Paint();
            fillPaint.Color = colors[colorIndex];

            var strokeWidth = series.StrokeWidth;
            strokePaint.StrokeWidth = strokeWidth;

            RectF pointRect = Util.ConvertToRectF(layoutSlot);
            strokeWidth /= 2.0f;
            pointRect.Left += strokeWidth;
            pointRect.Right -= strokeWidth;
            pointRect.Top += strokeWidth;
            pointRect.Bottom -= strokeWidth;

            float roundBarsRadius = series.RoundBarsRadius;

            if (series.AreBarsRounded)
            {
                canvas.DrawRoundRect(pointRect, roundBarsRadius, roundBarsRadius, fillPaint);
                canvas.DrawRoundRect(pointRect, roundBarsRadius, roundBarsRadius, strokePaint);
            }
            else
            {
                canvas.DrawRect(pointRect, fillPaint);
                canvas.DrawRect(pointRect, strokePaint);
            }
        }
    }
}