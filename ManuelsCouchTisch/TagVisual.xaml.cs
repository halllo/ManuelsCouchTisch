using Microsoft.Surface.Presentation.Input;
using System.Windows;
using System.Windows.Media;

namespace ManuelsCouchTisch
{
	public partial class TagVisual
	{
		public TagVisualModel ViewModel { get; private set; }

		public TagVisual()
		{
			InitializeComponent();

			DataContext = ViewModel = new TagVisualModel();

			Loaded += (s, e) => ViewModel.TagAvailable(VisualizedTag);
		}
	}

	public class TagVisualModel : ViewModel
	{
		string _Name;
		public string Name { get { return _Name; } set { _Name = value; NotifyChanged("Name"); } }

		Brush _Color;
		public Brush Color { get { return _Color; } set { _Color = value; NotifyChanged("Color"); } }

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
