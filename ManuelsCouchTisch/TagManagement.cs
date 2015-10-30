using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ManuelsCouchTisch
{
	public class TagManagement
	{
		public class Data
		{
			public string Name { get; set; }
			public Brush Color { get; set; }
		}


		public readonly static Lazy<TagManagement> Instance = new Lazy<TagManagement>(() => new TagManagement());


		public static readonly List<Brush> AllColors = new List<Brush>
		{
			Brushes.LimeGreen, Brushes.Pink, Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Purple,
		};

		public Dictionary<long, Data> Tags = new Dictionary<long, Data>
		{
			{ 0, new Data { Name = "0", Color = AllColors[0] } },
			{ 1, new Data { Name = "1", Color = AllColors[1] } },
			{ 2, new Data { Name = "Manuel", Color = AllColors[2] } },
			{ 3, new Data { Name = "3", Color = AllColors[3] } },
			{ 4, new Data { Name = "4", Color = AllColors[4] } },
			{ 5, new Data { Name = "5", Color = AllColors[5] } },
		};

		Dictionary<long, TagVisualModel> viewModels = new Dictionary<long, TagVisualModel>();

		public event Action OnShowDemo;
		public void RaiseShowDemo()
		{
			var h = OnShowDemo;
			if (h != null) h();
		}

		public event Action OnShowAdmin;
		public void RaiseShowAdmin()
		{
			var h = OnShowAdmin;
			if (h != null) h();
		}

		public void Register(long tag, TagVisualModel viewModel)
		{
			viewModels[tag] = viewModel;

			viewModel.Name = Tags[tag].Name;
			viewModel.Color = Tags[tag].Color;
		}

		public void Update(Dictionary<long, Data> namenUndFarben)
		{
			Tags = namenUndFarben;

			foreach (var tag in viewModels)
			{
				viewModels[tag.Key].Name = Tags[tag.Key].Name;
				viewModels[tag.Key].Color = Tags[tag.Key].Color;
			}
		}
	}
}
