using GalaSoft.MvvmLight;

namespace MVP.App.Models
{
    public class ChartDataItemViewModel : ViewModelBase
    {
        private string categoryName;
        private double categoryValue;

        public string CategoryName
        {
            get { return categoryName; }
            set { Set(() => CategoryName, ref categoryName, value); }
        }

        public double CategoryValue
        {
            get { return categoryValue; }
            set { Set(() => CategoryValue, ref categoryValue, value); }
        }
    }
}
