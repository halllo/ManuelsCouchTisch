using System.Windows;

namespace ManuelsCouchTisch.Test
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();







			var setTags = new RoutedEventHandler((s, e) =>
			{
				tag00.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 0));
				tag01.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 1));
				tag08.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 8));//tag02
			});

			tag00.Loaded += setTags;
			tag01.Loaded += setTags;
			tag08.Loaded += setTags;






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





			Loaded += (s, e) => TagManagement.Instance.Value.ConnectToMBus("http://localhost:8000/signalr");
		}
	}
}
