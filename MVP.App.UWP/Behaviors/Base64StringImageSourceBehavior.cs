namespace MVP.App.Behaviors
{
    using Microsoft.Xaml.Interactivity;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Defines a behavior for converting a base64 image string to an <see cref="ImageSource"/>.
    /// </summary>
    public class Base64StringImageSourceBehavior : Behavior<Image>
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="Base64String"/> property.
        /// </summary>
        public static readonly DependencyProperty Base64StringProperty =
            DependencyProperty.Register(
                nameof(Base64String),
                typeof(string),
                typeof(Base64StringImageSourceBehavior),
                new PropertyMetadata(
                    string.Empty,
                    (d, e) => ((Base64StringImageSourceBehavior)d).UpdateImageSource((string)e.NewValue)));

        /// <summary>
        /// Gets or sets the base64 string to convert.
        /// </summary>
        public string Base64String
        {
            get
            {
                return (string)this.GetValue(Base64StringProperty);
            }

            set
            {
                this.SetValue(Base64StringProperty, value);
            }
        }

        /// <inheritdoc />
        protected override void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null)
            {
                this.UpdateImageSource(this.Base64String);
            }
        }

        private async void UpdateImageSource(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64) || this.AssociatedObject == null)
            {
                return;
            }

            var source = await base64.ToImageSourceAsync();
            this.AssociatedObject.Source = source;
        }
    }
}