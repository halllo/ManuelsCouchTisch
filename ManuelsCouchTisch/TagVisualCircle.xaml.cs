using Microsoft.Surface.Presentation.Input;
using System.Windows;

namespace ManuelsCouchTisch
{
	public partial class TagVisualCircle
	{
		public TagVisualModel ViewModel { get; private set; }

		public TagVisualCircle()
		{
			InitializeComponent();

			DataContext = ViewModel = new TagVisualModel();
		}
	}

	public class TagVisualModel : ViewModel
	{
		string _Name;
		public string Name { get { return _Name; } set { _Name = value; NotifyChanged("Name"); } }

		System.Windows.Media.Brush _Color;
		public System.Windows.Media.Brush Color { get { return _Color; } set { _Color = value; NotifyChanged("Color"); } }

		Visibility _MasterMenuVisible;
		public Visibility MasterMenuVisible { get { return _MasterMenuVisible; } set { _MasterMenuVisible = value; NotifyChanged("MasterMenuVisible"); } }

		public Command Click { get; set; }
		public Command NamenUndFarben { get; set; }
		public Command ToggleDemo { get; set; }

		public TagVisualModel()
		{
			MasterMenuVisible = Visibility.Collapsed;
			Click = new Command(o => { });
			NamenUndFarben = new Command(o => { TagManagement.Instance.Value.RaiseShowAdmin(); });
			ToggleDemo = new Command(o => { TagManagement.Instance.Value.RaiseShowDemo(); });
		}

		public void TagAvailable(TagData tag)
		{
			MasterMenuVisible = tag.Value == 2 ? Visibility.Visible : Visibility.Collapsed;

			TagManagement.Instance.Value.Register(tag.Value, this);
		}
	}
}
