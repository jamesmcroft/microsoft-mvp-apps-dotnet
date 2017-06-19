using Com.Telerik.Android.Common;
using Com.Telerik.Widget.Chart.Engine.Axes;

using Java.Interop;

namespace MVP.App.Common.Charting
{
    public class LabelValueConverter : Java.Lang.Object, IFunction
    {
        public Java.Lang.Object Apply(Java.Lang.Object argument)
        {
            return $"Value is: {argument?.JavaCast<MajorTickModel>()?.Value()}";
        }
    }
}