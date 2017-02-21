namespace MVP.App.Controls
{
    using System.Windows.Input;

    using Windows.UI.Xaml;

    public sealed partial class ItemFlyoutControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(
                nameof(CloseCommand),
                typeof(ICommand),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                nameof(ContentTemplate),
                typeof(DataTemplate),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        public ItemFlyoutControl()
        {
            this.InitializeComponent();
        }

        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ContentTemplateProperty);
            }

            set
            {
                this.SetValue(ContentTemplateProperty, value);
            }
        }

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

        public ICommand CloseCommand
        {
            get
            {
                return (ICommand)this.GetValue(CloseCommandProperty);
            }

            set
            {
                this.SetValue(CloseCommandProperty, value);
            }
        }
    }
}