namespace MVP.App.Behaviors
{
    using Microsoft.Xaml.Interactivity;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class Base64StringImageSourceBehavior : Behavior<Image>
    {
        public static readonly DependencyProperty Base64StringProperty =
            DependencyProperty.Register(
                nameof(Base64String),
                typeof(string),
                typeof(Base64StringImageSourceBehavior),
                new PropertyMetadata(
                    string.Empty,
                    (d, e) => ((Base64StringImageSourceBehavior)d).UpdateImageSource((string)e.NewValue)));

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