using Com.Telerik.Android.Common;
using Com.Telerik.Widget.Chart.Engine.Axes;

using Java.Interop;

namespace MVP.App.Common.Charting
{
    public class LabelValueConverter : Java.Lang.Object, IFunction
    {
        public Java.Lang.Object Apply(Java.Lang.Object argument)
        {
            double labelValue = (argument.JavaCast<MajorTickModel>()).Value();
            string format = "Value is: {0}";
            return string.Format(format, labelValue);
        }
    }
}