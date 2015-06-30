using System.Windows;
using Microsoft.Surface.Presentation.Controls;

namespace ControlsBox
{
	/// <summary>
	/// Interaction logic for TagVisual.xaml
	/// </summary>
	public partial class TagVisual : TagVisualization
	{
		public TagVisual()
		{
			InitializeComponent();
		}

		private void TagVisual_Loaded(object sender, RoutedEventArgs e)
		{
			this.halloWeltTextBox.Text = this.VisualizedTag.Value.ToString();
			//TODO: customize TagVisualization1's UI based on this.VisualizedTag here
		}
	}
}
