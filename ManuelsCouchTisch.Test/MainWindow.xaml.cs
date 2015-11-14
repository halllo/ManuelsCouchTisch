using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace ManuelsCouchTisch.Test
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();




			var images1 = new ObservableCollection<ImageInfo>();
			var images2 = new ObservableCollection<ImageInfo>();
			string[] groups = { "Blue", "Green", "Orange", "Rhodamine" };
			//create two lists of images
			for (int i = 0; i <= 3; ++i)
			{
				for (int groupName = 0; groupName < 4; ++groupName)
				{
					images1.Add(new ImageInfo(groups[groupName] + i.ToString("0") + ".jpg", groups[groupName]));
				}
			}
			for (int i = 4; i <= 9; ++i)
			{
				for (int groupName = 0; groupName < 4; ++groupName)
				{
					images2.Add(new ImageInfo(groups[groupName] + i.ToString("0") + ".jpg", groups[groupName]));
				}
			}
			//map the images to the first library container
			libraryContainer1.DataContext = "libraryContainer1";
			ICollectionView view = CollectionViewSource.GetDefaultView(images1);
			view.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
			libraryContainer1.ItemsSource = view;
			//map the images to the second library container
			libraryContainer2.DataContext = "libraryContainer2";
			ICollectionView view2 = CollectionViewSource.GetDefaultView(images2);
			view2.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
			libraryContainer2.ItemsSource = view2;






			tag01.Loaded += (s, e) => { tag01.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 1)); };
			tag02.Loaded += (s, e) => { tag02.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 2)); };






			TagManagement.Instance.Value.OnShowDemo += () =>
			{
				var v = libraryContainer1ScatterViewItem.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
				libraryContainer1ScatterViewItem.Visibility = v;
				libraryContainer2ScatterViewItem.Visibility = v;
			};
			libraryContainer1ScatterViewItem.Visibility = System.Windows.Visibility.Collapsed;
			libraryContainer2ScatterViewItem.Visibility = System.Windows.Visibility.Collapsed;

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

			

			
			Loaded += (s, e) => TagManagement.Instance.Value.ConnectToMBus("http://localhost:8000/signalr");
		}
	}
}
