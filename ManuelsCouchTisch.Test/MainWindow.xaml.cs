using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManuelsCouchTisch.Test
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			SetupLibraryContainerImages();

			tag01.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 1));
			tag02.ViewModel.TagAvailable(new Microsoft.Surface.Presentation.Input.TagData(0, 0, 0, 2));
		}

		readonly ObservableCollection<ImageInfo> images = new ObservableCollection<ImageInfo>();
		readonly ObservableCollection<ImageInfo> images2 = new ObservableCollection<ImageInfo>();
		private void SetupLibraryContainerImages()
		{
			string[] groups = { "Blue", "Green", "Orange", "Rhodamine" };

			//create two lists of images
			for (int i = 0; i <= 3; ++i)
			{
				for (int groupName = 0; groupName < 4; ++groupName)
				{
					images.Add(new ImageInfo(groups[groupName] + i.ToString("0") + ".jpg", groups[groupName]));
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
			ICollectionView view = CollectionViewSource.GetDefaultView(images);
			view.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
			libraryContainer1.ItemsSource = view;

			//map the images to the second library container
			libraryContainer2.DataContext = "libraryContainer2";
			ICollectionView view2 = CollectionViewSource.GetDefaultView(images2);
			view2.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
			libraryContainer2.ItemsSource = view2;
		}
	}
}
