using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ManuelsCouchTisch
{
	public partial class ColorChooser
	{
		public ColorChooser()
		{
			InitializeComponent();
		}


		public Brush SelectedColor
		{
			get { return (Brush)GetValue(SelectedColorProperty); }
			set { SetValue(SelectedColorProperty, value); }
		}
		public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(ColorChooser), new PropertyMetadata(Brushes.White));


		public List<Brush> AllColors
		{
			get { return (List<Brush>)GetValue(AllColorsProperty); }
			set { SetValue(AllColorsProperty, value); }
		}
		public static readonly DependencyProperty AllColorsProperty = DependencyProperty.Register("AllColors", typeof(List<Brush>), typeof(ColorChooser), new PropertyMetadata(new List<Brush> { Brushes.White, Brushes.Black }, AllColorsChanged));
		private static void AllColorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var colorChooser = d as ColorChooser;
			colorChooser.ColorStack.Children.Clear();

			var colors = e.NewValue as List<Brush>;
			foreach (var color in colors)
			{
				var button = new Microsoft.Surface.Presentation.Controls.SurfaceButton { Background = color, Margin = new Thickness(0) };
				button.Click += colorChooser.ColorChoosen;
				colorChooser.ColorStack.Children.Add(button);
			}
		}


		private void ColorChoosen(object sender, RoutedEventArgs e)
		{
			SelectedColor = (sender as Control).Background;
		}
	}
}
