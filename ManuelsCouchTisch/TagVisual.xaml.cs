using Microsoft.Surface.Presentation.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using ZXing;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;

namespace ManuelsCouchTisch
{
	public partial class TagVisual
	{
		public TagVisualModel ViewModel { get; private set; }

		public TagVisual()
		{
			InitializeComponent();
			DataContext = ViewModel = new TagVisualModel();
		}
	}

	public class TagVisualModel : ViewModel
	{
		string _Name;
		public string Name { get { return _Name; } set { _Name = value; NotifyChanged("Name"); } }

		Brush _Color;
		public Brush Color { get { return _Color; } set { _Color = value; NotifyChanged("Color"); } }

		ImageSource _QrCode;
		public ImageSource QrCode { get { return _QrCode; } set { _QrCode = value; NotifyChanged("QrCode"); } }

		Visibility _QrCodeVisible;
		public Visibility QrCodeVisible { get { return _QrCodeVisible; } set { _QrCodeVisible = value; NotifyChanged("QrCodeVisible"); } }

		Visibility _MasterMenuVisible;
		public Visibility MasterMenuVisible { get { return _MasterMenuVisible; } set { _MasterMenuVisible = value; NotifyChanged("MasterMenuVisible"); } }

		public Command NamenUndFarben { get; set; }
		public Command Konsole { get; set; }

		public TagVisualModel()
		{
			MasterMenuVisible = Visibility.Collapsed;
			NamenUndFarben = new Command(o => { TagManagement.Instance.Value.RaiseShowNamenUndFarben(); });
			Konsole = new Command(o => { TagManagement.Instance.Value.RaiseShowKonsole(); });
		}

		public void TagAvailable(long tag)
		{
			var tagValue = tag % 6;

			MasterMenuVisible = tagValue == 0 ? Visibility.Visible : Visibility.Collapsed;
			QrCode = QRify("https://labs.neokc.de/gast?id=" + Hash((tagValue + 1) + " on " + DateTime.Today.ToString("ddMMyyyy")));

			TagManagement.Instance.Value.Register(tagValue, this);
		}

		static string Hash(string input)
		{
			var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
			return string.Join("", hash.Select(b => b.ToString("x2")).ToArray()).ToUpper();
		}

		static ImageSource QRify(string url)
		{
			var write = new BarcodeWriter();
			write.Format = BarcodeFormat.QR_CODE;
			var wb = write.Write(url);
			return ConvertBitmap(wb);
		}

		static BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
		{
			MemoryStream ms = new MemoryStream();
			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			ms.Seek(0, SeekOrigin.Begin);
			image.StreamSource = ms;
			image.EndInit();
			return image;
		}
	}
}
