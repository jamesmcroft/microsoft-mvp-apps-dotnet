namespace MVP.App
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Windows.UI.Xaml.Media.Imaging;

    public partial class Extensions
    {
        public static async Task<BitmapImage> ToImageSourceAsync(this string base64)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64.Replace("\"", ""))))
            {
                await bitmapImage.SetSourceAsync(stream.AsRandomAccessStream());
            }

            return bitmapImage;
        }
    }
}