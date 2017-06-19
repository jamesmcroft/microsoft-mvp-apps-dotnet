using Android.Views;
using Android.Widget;

// Install this to resolve these dependencies: http://www.telerik.com/download/xamarin-ui 
using Com.Telerik.Widget.Chart.Engine.Axes.Common;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical;
using Com.Telerik.Widget.Chart.Visualization.PieChart;
using Com.Telerik.Widget.Primitives.Legend;

using GalaSoft.MvvmLight.Helpers;

using Java.Util;

using MVP.App.Common.Charting;

namespace MVP.App
{
    public partial class InsightsActivity
    {
        private ProgressBar loadingProgressRing;

        private Binding<ViewStates, ViewStates> loadingProgressRingVisibilityBinding;

        private TextView loadingProgress;

        private Binding<string, string> loadingProgressTextBinding;

        private Binding<ViewStates, ViewStates> loadingProgressVisibilityBinding;

        private RadCartesianChartView barChart;

        private RadPieChartView pieChart;

        private RadLegendView pieLegend;

        public ProgressBar LoadingProgressRing
            =>
                this.loadingProgressRing
                ?? (this.loadingProgressRing = this.FindViewById<ProgressBar>(Resource.Id.loading_progressring));

        public TextView LoadingProgress
            =>
                this.loadingProgress
                ?? (this.loadingProgress = this.FindViewById<TextView>(Resource.Id.loading_progress));

        public RadCartesianChartView BarChart
            =>
                this.barChart
                ?? (this.barChart = this.FindViewById<RadCartesianChartView>(Resource.Id.bar_chart));

        public RadPieChartView PieChart
            =>
                this.pieChart
                ?? (this.pieChart = this.FindViewById<RadPieChartView>(Resource.Id.pie_chart));

        public RadLegendView PieLegend
            =>
                this.pieLegend
                ?? (this.pieLegend = this.FindViewById<RadLegendView>(Resource.Id.pie_legend));

        private async void SetupBindings()
        {
            this.loadingProgressRingVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadingState,
                () => this.LoadingProgressRing.Visibility);

            this.loadingProgressTextBinding = this.SetBinding(
                () => this.ViewModel.LoadingProgress,
                () => this.LoadingProgress.Text);

            this.loadingProgressVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadingState,
                () => this.LoadingProgress.Visibility);

            // Needs to be called first because ObservableCollection isn't working as it should
            await this.ViewModel.UpdateChartDataAsync();

            // Initial bar chart setup
            this.BarChart.HorizontalAxis = new CategoricalAxis { LabelFitMode = AxisLabelFitMode.Rotate };
            this.BarChart.VerticalAxis = new LinearAxis { LabelFormat = "%.0f" };

            var s1 = new BarSeries();
            s1.CategoryBinding = new ChartDataItemBinding("CategoryName");
            s1.ValueBinding = new ChartDataItemBinding("CategoryValue");
            s1.Data = new ArrayList(ViewModel.GroupedContributionsData);

            this.BarChart.Series.Add(s1);

            // Initial pie chart setup
            var s2 = new DoughnutSeries();
            s2.NameBinding = new ChartDataItemBinding("CategoryName");
            s2.ValueBinding = new ChartDataItemBinding("CategoryValue");
            s2.Data = new ArrayList(ViewModel.GroupedContributionsData);

            this.PieChart.Series.Add(s2);

            // Legened setup
            this.PieLegend.LegendProvider = this.PieChart;
        }
    }
}