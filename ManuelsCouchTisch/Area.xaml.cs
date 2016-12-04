using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ManuelsCouchTisch
{
	public partial class Area
	{
		public Area()
		{
			InitializeComponent();




			TagManagement.Instance.Value.OnShowNamenUndFarben += () =>
			{
				namenUndFarben.ViewModel.WindowVisible = Visibility.Visible;
			};
			namenUndFarben.ViewModel.WindowVisible = Visibility.Collapsed;

			TagManagement.Instance.Value.OnShowKonsole += () =>
			{
				konsole.ViewModel.WindowVisible = Visibility.Visible;
			};
			konsole.ViewModel.WindowVisible = Visibility.Collapsed;

			RemoteZentrale.Instance.Value.OnNewImage += imageAsBase64 =>
			{
				Dispatcher.BeginInvoke(new System.Action(() =>
				{
					ScatterViewOverlay.Items.Add(new ImageView(imageAsBase64));
				}));
			};

			RemoteZentrale.Instance.Value.OnConfirmImages += () =>
			{
				Dispatcher.BeginInvoke(new System.Action(() =>
				{
					var smartphone = TrackingCanvasLayer.TrackedBlobs.FirstOrDefault(b => b.Description == "smartphone?");
					if (smartphone != null)
					{
						var smartphonePosition = smartphone.Center;
						var imagePosition = new Point(smartphonePosition.X + 200, smartphonePosition.Y);
						var imageViews = ScatterViewOverlay.Items.OfType<ImageView>();
						foreach (var imageView in imageViews)
						{
							imageView.Center = imagePosition;
							imageView.Visibility = Visibility.Visible;
						}
					}
				}));
			};
		}






		private void TrackingCanvas_StartTracking(TrackingCanvas.ITrackedBlob trackedBlob)
		{
			if (trackedBlob.IsTag)
			{
				var tagVisual = new TagVisual();
				tagVisual.RenderTransform = new RotateTransform(trackedBlob.Axis.Orientation, tagVisual.Width / 2, tagVisual.Height / 2);

				tagVisual.ViewModel.TagAvailable(trackedBlob.TagValue);
				trackedBlob.Display(tagVisual);
			}
			else
			{
				var majorAxis = trackedBlob.Axis.MajorAxis;
				if (160 >= majorAxis && majorAxis >= 90)
				{
					trackedBlob.Description = "smartphone?";
					trackedBlob.Display(new Ellipse
					{
						Width = 350,
						Height = 350,
						Stroke = new SolidColorBrush(Colors.Orange),
						StrokeThickness = 2,
					});
				}
				else
				{
					trackedBlob.Display(new Ellipse
					{
						Width = 220,
						Height = 220,
						Opacity = 0.5,
						Stroke = new SolidColorBrush(Colors.Gray),
						StrokeThickness = 2,
					});
				}
			}
		}
	}
}
