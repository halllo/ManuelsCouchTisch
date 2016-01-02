using Microsoft.Surface;
using System;
using System.ComponentModel;

namespace ManuelsCouchTisch
{
	public partial class Window1
	{
		public Window1()
		{
			InitializeComponent();

			ApplicationServices.InactivityTimeoutOccurring += ApplicationServices_InactivityTimeoutOccurring;







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






			Loaded += (s, e) => TagManagement.Instance.Value.ConnectToMBus();
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