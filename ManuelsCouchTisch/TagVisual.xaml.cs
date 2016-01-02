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

		public Command NamenUndFarben { get; set; }
		public Command Konsole { get; set; }

		public TagVisualModel()
		{
			MasterMenuVisible = Visibility.Collapsed;
			NamenUndFarben = new Command(o => { TagManagement.Instance.Value.RaiseShowNamenUndFarben(); });
			Konsole = new Command(o => { TagManagement.Instance.Value.RaiseShowKonsole(); });
		}

		public void TagAvailable(TagData tag)
		{
			var tagValue = tag.Value % 6;

			MasterMenuVisible = tagValue == 0 ? Visibility.Visible : Visibility.Collapsed;

			TagManagement.Instance.Value.Register(tagValue, this);
		}
	}
}
