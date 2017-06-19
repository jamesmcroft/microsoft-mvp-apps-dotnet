namespace MVP.App.Common.Charting
{
    public class ChartDataItemViewModel : Java.Lang.Object
    {
        public string CategoryName { get; set; }
        public double CategoryValue { get; set; }

        public ChartDataItemViewModel() { }

        public ChartDataItemViewModel(string month, double result)
        {
            this.CategoryName = month;
            this.CategoryValue = result;
        }
    }
}