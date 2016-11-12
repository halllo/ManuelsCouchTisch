using System;
using System.ComponentModel;
using Microsoft.Surface;

namespace ManuelsCouchTisch
{
	public partial class Window1
	{
		public Window1()
		{
			InitializeComponent();

			ApplicationServices.InactivityTimeoutOccurring += ApplicationServices_InactivityTimeoutOccurring;

			Loaded += (s, e) => TagManagement.Instance.Value.ConnectToMBus();

			((Area)Content).TrackingCanvasLayer.LogTrackedObjects = false;
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			ApplicationServices.InactivityTimeoutOccurring -= ApplicationServices_InactivityTimeoutOccurring;
		}

		private void ApplicationServices_InactivityTimeoutOccurring(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
		}	
	}
}