using System;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Surface.Presentation.Controls;

namespace ManuelsCouchTisch
{
	public partial class ImageView
	{
		public ImageView()
		{
			InitializeComponent();
		}

		public ImageView(string imageAsBase64) : this()
		{
			Visibility = System.Windows.Visibility.Collapsed;

			var pixels = Convert.FromBase64String(imageAsBase64);
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

			var scatterView = Parent as ScatterView;
			scatterView.Items.Remove(this);
		}
	}
}
