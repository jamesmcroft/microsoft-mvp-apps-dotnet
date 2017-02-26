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

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
            nameof(SaveCommand),
            typeof(ICommand),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SaveButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(SaveButtonVisibility),
                typeof(Visibility),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(Visibility.Collapsed));

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

        public Visibility SaveButtonVisibility
        {
            get
            {
                return (Visibility)this.GetValue(SaveButtonVisibilityProperty);
            }

            set
            {
                this.SetValue(SaveButtonVisibilityProperty, value);
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return (ICommand)this.GetValue(SaveCommandProperty);
            }

            set
            {
                this.SetValue(SaveCommandProperty, value);
            }
        }
    }
}