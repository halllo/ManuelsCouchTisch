using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ManuelsCouchTisch
{
	public partial class NamenUndFarbenView
	{
		public NamenUndFarbenViewModel ViewModel { get; private set; }

		public NamenUndFarbenView()
		{
			InitializeComponent();
			DataContext = ViewModel = new NamenUndFarbenViewModel();
		}
	}

	public class NamenUndFarbenViewModel : ViewModel
	{
		public List<Brush> Colors { get; set; }

		string _Name0; public string Name0 { get { return _Name0; } set { _Name0 = value; NotifyChanged("Name0"); } }
		string _Name1; public string Name1 { get { return _Name1; } set { _Name1 = value; NotifyChanged("Name1"); } }
		string _Name2; public string Name2 { get { return _Name2; } set { _Name2 = value; NotifyChanged("Name2"); } }
		string _Name3; public string Name3 { get { return _Name3; } set { _Name3 = value; NotifyChanged("Name3"); } }
		string _Name4; public string Name4 { get { return _Name4; } set { _Name4 = value; NotifyChanged("Name4"); } }
		string _Name5; public string Name5 { get { return _Name5; } set { _Name5 = value; NotifyChanged("Name5"); } }

		Brush _Color0; public Brush Color0 { get { return _Color0; } set { _Color0 = value; NotifyChanged("Color0"); } }
		Brush _Color1; public Brush Color1 { get { return _Color1; } set { _Color1 = value; NotifyChanged("Color1"); } }
		Brush _Color2; public Brush Color2 { get { return _Color2; } set { _Color2 = value; NotifyChanged("Color2"); } }
		Brush _Color3; public Brush Color3 { get { return _Color3; } set { _Color3 = value; NotifyChanged("Color3"); } }
		Brush _Color4; public Brush Color4 { get { return _Color4; } set { _Color4 = value; NotifyChanged("Color4"); } }
		Brush _Color5; public Brush Color5 { get { return _Color5; } set { _Color5 = value; NotifyChanged("Color5"); } }

		public Command Save { get; set; }
		public Command Close { get; set; }

		Visibility _WindowVisible;
		public Visibility WindowVisible { get { return _WindowVisible; } set { _WindowVisible = value; NotifyChanged("WindowVisible"); } }

		public NamenUndFarbenViewModel()
		{
			Colors = TagManagement.AllColors;

			Name0 = TagManagement.Instance.Value.Tags[0].Name;
			Name1 = TagManagement.Instance.Value.Tags[1].Name;
			Name2 = TagManagement.Instance.Value.Tags[2].Name;
			Name3 = TagManagement.Instance.Value.Tags[3].Name;
			Name4 = TagManagement.Instance.Value.Tags[4].Name;
			Name5 = TagManagement.Instance.Value.Tags[5].Name;

			Color0 = TagManagement.Instance.Value.Tags[0].Color;
			Color1 = TagManagement.Instance.Value.Tags[1].Color;
			Color2 = TagManagement.Instance.Value.Tags[2].Color;
			Color3 = TagManagement.Instance.Value.Tags[3].Color;
			Color4 = TagManagement.Instance.Value.Tags[4].Color;
			Color5 = TagManagement.Instance.Value.Tags[5].Color;

			Close = new Command(o => { WindowVisible = Visibility.Collapsed; });
			Save = new Command(o =>
			{
				TagManagement.Instance.Value.Update(new Dictionary<long, TagManagement.Data>
				{
					{ 0, new TagManagement.Data { Name = Name0, Color = Color0 } },
					{ 1, new TagManagement.Data { Name = Name1, Color = Color1 } },
					{ 2, new TagManagement.Data { Name = Name2, Color = Color2 } },
					{ 3, new TagManagement.Data { Name = Name3, Color = Color3 } },
					{ 4, new TagManagement.Data { Name = Name4, Color = Color4 } },
					{ 5, new TagManagement.Data { Name = Name5, Color = Color5 } },
				});
			});
		}
	}
}
