namespace MVP.App.Controls
{
    using System.Windows.Input;

    using Windows.UI.Xaml;

    /// <summary>
    /// Defines a flyout control designed to show data from a model.
    /// </summary>
    public sealed partial class ItemFlyoutControl
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="Title"/> property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Defines the dependency property for the <see cref="CloseCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(
                nameof(CloseCommand),
                typeof(ICommand),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines the dependency property for the <see cref="ReadonlyContentTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty ReadonlyContentTemplateProperty =
            DependencyProperty.Register(
                nameof(ReadonlyContentTemplate),
                typeof(DataTemplate),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines the dependency property for the <see cref="EditContentTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty EditContentTemplateProperty =
            DependencyProperty.Register(
                nameof(EditContentTemplate),
                typeof(DataTemplate),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Defines the dependency property for the <see cref="IsInEdit"/> property.
        /// </summary>
        public static readonly DependencyProperty IsInEditProperty = DependencyProperty.Register(
            nameof(IsInEdit),
            typeof(bool),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(false));

        /// <summary>
        /// Defines the dependency property for the <see cref="SaveCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
            nameof(SaveCommand),
            typeof(ICommand),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(null));

        /// <summary>
        /// Defines the dependency property for the <see cref="CanDeleteProperty"/> property.
        /// </summary>
        public static readonly DependencyProperty CanDeleteProperty = DependencyProperty.Register(
            nameof(CanDelete),
            typeof(bool),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(false));

        /// <summary>
        /// Defines the dependency property for the <see cref="EditCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register(
            nameof(EditCommand),
            typeof(ICommand),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Defines the dependency property for the <see cref="DeleteCommand"/> property.
        /// </summary>
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(
                nameof(DeleteCommand),
                typeof(ICommand),
                typeof(ItemFlyoutControl),
                new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Defines the dependency property for the <see cref="CanEdit"/> property.
        /// </summary>
        public static readonly DependencyProperty CanEditProperty = DependencyProperty.Register(
            nameof(CanEdit),
            typeof(bool),
            typeof(ItemFlyoutControl),
            new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemFlyoutControl"/> class.
        /// </summary>
        public ItemFlyoutControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item can be edited.
        /// </summary>
        public bool CanEdit
        {
            get
            {
                return (bool)this.GetValue(CanEditProperty);
            }

            set
            {
                this.SetValue(CanEditProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command called when the delete button is clicked.
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                return (ICommand)this.GetValue(DeleteCommandProperty);
            }

            set
            {
                this.SetValue(DeleteCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item can be deleted.
        /// </summary>
        public bool CanDelete
        {
            get
            {
                return (bool)this.GetValue(CanDeleteProperty);
            }

            set
            {
                this.SetValue(CanDeleteProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command called when the edit button is clicked.
        /// </summary>
        public ICommand EditCommand
        {
            get
            {
                return (ICommand)this.GetValue(EditCommandProperty);
            }

            set
            {
                this.SetValue(EditCommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the data template to be used when the item is being edited.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the data template to be used when the item is not being edited.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the title of the flyout.
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

        /// <summary>
        /// Gets or sets the command called when the close button is clicked.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the command called when the save button is clicked.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the item is in edit mode.
        /// </summary>
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