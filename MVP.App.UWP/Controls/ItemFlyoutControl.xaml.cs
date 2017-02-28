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

        public static readonly DependencyProperty ReadonlyContentTemplateProperty =
            DependencyProperty.Register(
                nameof(ReadonlyContentTemplate),
                typeof(DataTemplate),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty EditContentTemplateProperty =
            DependencyProperty.Register(
                nameof(EditContentTemplate),
                typeof(DataTemplate),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsInEditProperty = DependencyProperty.Register(
            nameof(IsInEdit),
            typeof(bool),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(false));

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
            nameof(SaveCommand),
            typeof(ICommand),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(nameof(EditCommand), typeof(ICommand), typeof(ItemFlyoutControl), new PropertyMetadata(default(ICommand)));

        public ItemFlyoutControl()
        {
            this.InitializeComponent();
        }

        public ICommand EditCommand
        {
            get
            {
                return (ICommand)GetValue(EditCommandProperty);
            }
            set
            {
                SetValue(EditCommandProperty, value);
            }
        }

        public DataTemplate EditContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(EditContentTemplateProperty);
            }

            set
            {
                this.SetValue(EditContentTemplateProperty, value);
            }
        }

        public DataTemplate ReadonlyContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ReadonlyContentTemplateProperty);
            }

            set
            {
                this.SetValue(ReadonlyContentTemplateProperty, value);
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

        public bool IsInEdit
        {
            get
            {
                return (bool)this.GetValue(IsInEditProperty);
            }

            set
            {
                this.SetValue(IsInEditProperty, value);
            }
        }
    }
}