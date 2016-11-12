using System.Windows;

namespace ManuelsCouchTisch.Test
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			
			Loaded += (s, e) =>
			{
				TagManagement.Instance.Value.ConnectToMBus(/*"http://localhost:8000/signalr"*/);

				area.ScatterViewOverlay.Items.Add(Tag(value: 0));
				area.ScatterViewOverlay.Items.Add(Tag(value: 3));
				area.ScatterViewOverlay.Items.Add(Ding(majorAxis: 100));
				area.ScatterViewOverlay.Items.Add(Ding(majorAxis: 50));
			};

			((Area)Content).TrackingCanvasLayer.LogTrackedObjects = true;
		}

		Object Tag(long value)
		{
			return new Object(new ObjectViewModel(area.TrackingCanvasLayer)
			{
				IsTag = true,
				TagValue = value
			});
		}

		Object Ding(double majorAxis, double minorAxis = 0)
		{
			return new Object(new ObjectViewModel(area.TrackingCanvasLayer)
			{
				Size = new Size(majorAxis, minorAxis)
			});
		}
	}
}
