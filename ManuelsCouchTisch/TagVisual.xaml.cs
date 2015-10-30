using Microsoft.Surface.Presentation.Input;
using System.Windows;

namespace ManuelsCouchTisch
{
	public partial class TagVisual
	{
		public TagVisual()
		{
			InitializeComponent();
			
			Loaded += (s, e) => circle.ViewModel.TagAvailable(VisualizedTag);
		}
	}
}
