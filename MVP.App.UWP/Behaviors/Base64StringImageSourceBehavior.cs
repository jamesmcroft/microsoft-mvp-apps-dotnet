namespace MVP.App.Behaviors
{
    using System.Threading.Tasks;

    using Microsoft.Xaml.Interactivity;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Defines a behavior for converting a base64 image string to an ImageSource.
    /// </summary>
    public class Base64StringImageSourceBehavior : Behavior<Image>
    {
        /// <summary>
        /// Defines the dependency property for the <see cref="Base64String"/> property.
        /// </summary>
        public static readonly DependencyProperty Base64StringProperty = DependencyProperty.Register(
            nameof(Base64String),
            typeof(string),
            typeof(Base64StringImageSourceBehavior),
            new PropertyMetadata(
                string.Empty,
                async (d, e) => await ((Base64StringImageSourceBehavior)d).UpdateImageSourceAsync((string)e.NewValue)));

        /// <summary>
        /// Gets or sets the base64 string to convert.
        /// </summary>
        public string Base64String
        {
            get => (string)this.GetValue(Base64StringProperty);
            set => this.SetValue(Base64StringProperty, value);
        }

        /// <summary>
        /// Called after the behavior is attached to the AssociatedObject.
        /// </summary>
        protected override async void OnAttached()
        {
            base.OnAttached();

            if (this.AssociatedObject != null)
            {
                await this.UpdateImageSourceAsync(this.Base64String);
            }
        }

        private async Task UpdateImageSourceAsync(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64) || this.AssociatedObject == null)
            {
                return;
            }

            BitmapImage source = await base64.ToImageSourceAsync();
            this.AssociatedObject.Source = source;
        }
    }
}