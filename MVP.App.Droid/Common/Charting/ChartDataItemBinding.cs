using System;
using Com.Telerik.Widget.Chart.Engine.Databinding;

namespace MVP.App.Common.Charting
{
    internal class ChartDataItemBinding : DataPointBinding
    {
        private readonly string propertyName;

        public ChartDataItemBinding(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override Java.Lang.Object GetValue(Java.Lang.Object p0)
        {
            switch (propertyName)
            {
                case "CategoryName":
                    return ((ChartDataItemViewModel)(p0)).CategoryName;
                case "CategoryValue":
                    return ((ChartDataItemViewModel)(p0)).CategoryValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(p0), "Property Name must match one of the available properties");
            }
        }
    }
}