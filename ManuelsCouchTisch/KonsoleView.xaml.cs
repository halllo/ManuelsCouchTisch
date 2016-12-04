using System.Collections.ObjectModel;
using System.Windows;

namespace ManuelsCouchTisch
{
	public partial class KonsoleView
	{
		public KonsoleViewModel ViewModel { get; private set; }

		public KonsoleView()
		{
			InitializeComponent();
			DataContext = ViewModel = new KonsoleViewModel();
		}
	}

	public class KonsoleViewModel : ViewModel
	{
		public Command Close { get; set; }

		Visibility _WindowVisible;
		public Visibility WindowVisible { get { return _WindowVisible; } set { _WindowVisible = value; NotifyChanged("WindowVisible"); } }

		public ObservableCollection<string> KonsoleItems { get; set; }

		public KonsoleViewModel()
		{
			Close = new Command(o => { WindowVisible = Visibility.Collapsed; });
			KonsoleItems = new ObservableCollection<string>();
			RemoteZentrale.Instance.Value.OnLog += Value_OnLog;
		}

		private void Value_OnLog(string log)
		{
			Dispatch(() =>
			{
				KonsoleItems.Add(log);
			});
		}
	}
}
