using System.IO;
using System.Windows.Media.Imaging;

namespace ManuelsCouchTisch
{
	public partial class ImageView
	{
		public ImageView()
		{
			InitializeComponent();
		}

		public ImageView(byte[] pixels) : this()
		{
			Bild.Source = ConvertImage(pixels);
		}

		private BitmapImage ConvertImage(byte[] pixels)
		{
			using (var stream = new MemoryStream(pixels))
			{
				var bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = stream;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				return bitmapImage;
			}
		}

		private void CloseClick(object sender, System.Windows.RoutedEventArgs e)
		{
			Bild.Source = null;
			Visibility = System.Windows.Visibility.Collapsed;
		}
	}
}
