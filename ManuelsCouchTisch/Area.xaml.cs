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
				namenUndFarben.ViewModel.WindowVisible = System.Windows.Visibility.Visible;
			};
			namenUndFarben.ViewModel.WindowVisible = System.Windows.Visibility.Collapsed;

			TagManagement.Instance.Value.OnShowKonsole += () =>
			{
				konsole.ViewModel.WindowVisible = System.Windows.Visibility.Visible;
			};
			konsole.ViewModel.WindowVisible = System.Windows.Visibility.Collapsed;

			RemoteZentrale.Instance.Value.OnNewImage += imageAsBase64 =>
			{
				Dispatcher.BeginInvoke(new System.Action(() =>
				{
					ScatterViewOverlay.Items.Add(new ImageView(imageAsBase64));
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
					trackedBlob.Display(new Ellipse
					{
						Width = 350,
						Height = 350,
						StrokeThickness = 2,
						Stroke = new SolidColorBrush(Colors.Orange)
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
