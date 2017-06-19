using Android.Views;
using Android.Widget;
using Android.Graphics;

// Install this to resolve these dependencies: http://www.telerik.com/download/xamarin-ui 
using Com.Telerik.Widget.Chart.Engine.Axes.Common;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart.Axes;
using Com.Telerik.Widget.Chart.Visualization.CartesianChart.Series.Categorical;
using Com.Telerik.Widget.Chart.Visualization.PieChart;
using Com.Telerik.Widget.Numberpicker;
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

        private Binding<string, string> loadingProgressTextBinding;

        private Binding<ViewStates, ViewStates> loadingProgressVisibilityBinding;

        private TextView loadingProgress;
        
        private Color[] seriesColors = ChartHelpers.GetColors(null);

        private RadCartesianChartView barChart;

        private RadPieChartView pieChart;

        private RadLegendView pieLegend;
        
        private RadNumberPicker numberPicker;
        private Button updateChartsButton;
        private Binding<int, double> numberPickerValueBinding;

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
        
        public BarSeries FirstBarSeries { get; set; }

        public DoughnutSeries FirstDoughnutSeries { get; set; }

        public RadLegendView PieLegend => pieLegend ?? (pieLegend = FindViewById<RadLegendView>(Resource.Id.pie_legend));

        public RadNumberPicker MyNumberPicker => numberPicker ?? (numberPicker = FindViewById<RadNumberPicker>(Resource.Id.number_picker));

        public Button UpdateChartsButton => updateChartsButton ?? (updateChartsButton = FindViewById<Button>(Resource.Id.update_charts_button));

        private async void SetupBindings()
        {
            // Base setup
            this.loadingProgressRingVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadingState,
                () => this.LoadingProgressRing.Visibility);

            this.loadingProgressTextBinding = this.SetBinding(
                () => this.ViewModel.LoadingProgress,
                () => this.LoadingProgress.Text);

            this.loadingProgressVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadingState,
                () => this.LoadingProgress.Visibility);
            
            // View specific setup
            await ViewModel.GroupChartDataAsync();

            // Setup refresh button
            UpdateChartsButton.Click += UpdateChartsButton_Click;

            // RadNumberPicker setup
            numberPickerValueBinding = this.SetBinding(() => ViewModel.ContributionsToRetrieve, () => MyNumberPicker.Value, BindingMode.TwoWay);
            MyNumberPicker.Maximum = 100;

            var spinner = FindViewById<Spinner>(Resource.Id.group_spinner);
            spinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, ViewModel.GroupByTypes);
            spinner.ItemSelected += Spinner_ItemSelected;
            
            InitializeCharts();
        }

        private async void UpdateChartsButton_Click(object sender, System.EventArgs e)
        {
            // group by the selected type
            await ViewModel.GroupChartDataAsync();

            UpdateChartSeries();
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            if(spinner != null)
                ViewModel.SelectedGroupByType = spinner.GetItemAtPosition(e.Position).ToString();
        }

        private void InitializeCharts()
        {
            // ------------ CartesianChart ------------- //

            FirstBarSeries = new BarSeries();
            FirstBarSeries.CategoryBinding = new ChartDataItemBinding("CategoryName");
            FirstBarSeries.ValueBinding = new ChartDataItemBinding("CategoryValue");
            FirstBarSeries.Data = new ArrayList(ViewModel.GroupedContributionsData);

            // TODO int to float Java exception needs to be resolved
            //FirstBarSeries.CanApplyPalette = false;
            //FirstBarSeries.DataPointRenderer = new CustomBarRenderer(FirstBarSeries, seriesColors);

            BarChart.Series.Add(FirstBarSeries);
            BarChart.HorizontalAxis = new CategoricalAxis { LabelFitMode = AxisLabelFitMode.Rotate };
            BarChart.VerticalAxis = new LinearAxis { LabelFormat = "%.0f" };


            // ------------ PieChart ------------- //

            FirstDoughnutSeries = new DoughnutSeries();
            FirstDoughnutSeries.NameBinding = new ChartDataItemBinding("CategoryName");
            FirstDoughnutSeries.ValueBinding = new ChartDataItemBinding("CategoryValue");
            FirstDoughnutSeries.Data = new ArrayList(ViewModel.GroupedContributionsData);


            FirstDoughnutSeries.CanApplyPalette = false;
            FirstDoughnutSeries.SliceStyles = ChartHelpers.GeneratePieSlices(seriesColors);

            PieChart.Series.Add(FirstDoughnutSeries);

            // Legened setup
            PieLegend.LegendProvider = PieChart;
        }

        private void UpdateChartSeries()
        {
            // Clear any previous series
            if (!BarChart.Series.IsEmpty)
                BarChart.Series.Clear();

            FirstBarSeries = new BarSeries
            {
                CategoryBinding = new ChartDataItemBinding("CategoryName"),
                ValueBinding = new ChartDataItemBinding("CategoryValue"),
                DataPointRenderer = new CustomBarRenderer(FirstBarSeries, seriesColors),
                Data = new ArrayList(ViewModel.GroupedContributionsData)
            };

            BarChart.Series.Add(FirstBarSeries);
            BarChart.HorizontalAxis = new CategoricalAxis { LabelFitMode = AxisLabelFitMode.Rotate };
            BarChart.VerticalAxis = new LinearAxis { LabelFormat = "%.0f" };

            // Clear any previous series
            if (!PieChart.Series.IsEmpty)
                PieChart.Series.Clear();

            FirstDoughnutSeries = new DoughnutSeries
            {
                NameBinding = new ChartDataItemBinding("CategoryName"),
                ValueBinding = new ChartDataItemBinding("CategoryValue"),
                Data = new ArrayList(ViewModel.GroupedContributionsData),
                CanApplyPalette = false,
                SliceStyles = ChartHelpers.GeneratePieSlices(seriesColors)
            };

            PieChart.Series.Add(FirstDoughnutSeries);
        }
    }
}