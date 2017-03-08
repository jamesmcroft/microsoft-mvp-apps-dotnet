namespace MVP.App.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Defines a header control for a page.
    /// </summary>
    public sealed partial class PageHeader : UserControl
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="Title"/> property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(PageHeader),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHeader"/> class.
        /// </summary>
        public PageHeader()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }

            set
            {
                this.SetValue(TitleProperty, value);
            }
        }
    }
}