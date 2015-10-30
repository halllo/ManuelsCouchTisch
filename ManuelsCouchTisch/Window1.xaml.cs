using Microsoft.Surface;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace ManuelsCouchTisch
{
	public partial class Window1
	{
		public Window1()
		{
			InitializeComponent();

			AddWindowAvailabilityHandlers();







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








			TagManagement.Instance.Value.OnShowDemo += () =>
			{
				var v = libraryContainer1ScatterViewItem.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                libraryContainer1ScatterViewItem.Visibility = v;
				libraryContainer2ScatterViewItem.Visibility = v;
			};
			libraryContainer1ScatterViewItem.Visibility = System.Windows.Visibility.Collapsed;
			libraryContainer2ScatterViewItem.Visibility = System.Windows.Visibility.Collapsed;

			TagManagement.Instance.Value.OnShowAdmin += () =>
			{
				namenUndFarben.ViewModel.WindowVisible = System.Windows.Visibility.Visible;
			};
			namenUndFarben.ViewModel.WindowVisible = System.Windows.Visibility.Collapsed;


        }

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);
			
			RemoveWindowAvailabilityHandlers();
		}

		#region window availability handlers
		/// <summary>
		/// Adds handlers for window availability events.
		/// </summary>
		private void AddWindowAvailabilityHandlers()
		{
			// Subscribe to surface window availability events
			ApplicationServices.WindowInteractive += OnWindowInteractive;
			ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
			ApplicationServices.WindowUnavailable += OnWindowUnavailable;
		}

		/// <summary>
		/// Removes handlers for window availability events.
		/// </summary>
		private void RemoveWindowAvailabilityHandlers()
		{
			// Unsubscribe from surface window availability events
			ApplicationServices.WindowInteractive -= OnWindowInteractive;
			ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
			ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
		}

		/// <summary>
		/// This is called when the user can interact with the application's window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowInteractive(object sender, EventArgs e)
		{
			//TODO: enable audio, animations here
		}

		/// <summary>
		/// This is called when the user can see but not interact with the application's window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowNoninteractive(object sender, EventArgs e)
		{
			//TODO: Disable audio here if it is enabled

			//TODO: optionally enable animations here
		}

		/// <summary>
		/// This is called when the application's window is not visible or interactive.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnWindowUnavailable(object sender, EventArgs e)
		{
			//TODO: disable audio, animations here
		}
		#endregion
	}
}