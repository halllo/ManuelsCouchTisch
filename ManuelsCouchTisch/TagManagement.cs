using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ManuelsCouchTisch
{
	public class TagManagement
	{
		public class Data
		{
			public string Name { get; set; }
			public Brush Color { get; set; }
			public Visibility QrCodeVisible { get; set; }
		}

		public static readonly Dictionary<string, Brush> AllColors = new Dictionary<string, Brush>
		{
			{ "rot", Brushes.Red },
			{ "grün", Brushes.Green },
			{ "blau", Brushes.Blue },
			{ "lime", Brushes.Lime },
			{ "magenta", Brushes.DeepPink },
			{ "grau", Brushes.DarkGray },
		};

		public Dictionary<long, Data> Tags = new Dictionary<long, Data>
		{
			{ 0, new Data { Name = "Manuel",    Color = AllColors["blau"],      QrCodeVisible=Visibility.Visible } },
			{ 1, new Data { Name = "1",         Color = AllColors["grün"],      QrCodeVisible=Visibility.Visible } },
			{ 2, new Data { Name = "2",         Color = AllColors["rot"],       QrCodeVisible=Visibility.Visible } },
			{ 3, new Data { Name = "3",         Color = AllColors["lime"],      QrCodeVisible=Visibility.Visible } },
			{ 4, new Data { Name = "4",         Color = AllColors["magenta"],   QrCodeVisible=Visibility.Visible } },
			{ 5, new Data { Name = "5",         Color = AllColors["grau"],      QrCodeVisible=Visibility.Visible } },
		};

		Dictionary<long, TagVisualModel> viewModels = new Dictionary<long, TagVisualModel>();

		private GastGedaechtnis _gedaechtnis;


		public readonly static Lazy<TagManagement> Instance = new Lazy<TagManagement>(() => new TagManagement());
		private TagManagement()
		{
			_gedaechtnis = new GastGedaechtnis(Tags, AllColors);
		}


		public event Action OnTagsChanged;
		public void RaiseTagsChanged()
		{
			var h = OnTagsChanged;
			if (h != null) h();
		}

		public event Action OnShowNamenUndFarben;
		public void RaiseShowNamenUndFarben()
		{
			var h = OnShowNamenUndFarben;
			if (h != null) h();
		}

		public event Action OnShowKonsole;
		public void RaiseShowKonsole()
		{
			var h = OnShowKonsole;
			if (h != null) h();
		}

		public void Connect()
		{
			_gedaechtnis.Restore();
			RaiseTagsChanged();
		}

		public void UpdateTags(string message)
		{
			var messageItems = message.Split(new[] { ";" }, StringSplitOptions.None);
			if (messageItems.Length == 4)
			{
				var tag = long.Parse(messageItems[1]);
				var name = messageItems[2];
				var color = AllColors[messageItems[3]];

				var tagData = Tags[tag];
				tagData.Name = name;
				tagData.Color = color;
				tagData.QrCodeVisible = Visibility.Collapsed;
				_gedaechtnis.Store();
				RaiseTagsChanged();

				var viewModel = viewModels[tag];
				viewModel.Dispatch(() =>
				{
					viewModel.Name = name;
					viewModel.Color = color;
					viewModel.QrCodeVisible = Visibility.Collapsed;
				});
			}
		}

		public void MBusEmitTags()
		{
			var tagDump = _gedaechtnis.AsTagDump();

			var mbus = RemoteZentrale.Instance.Value.MBus;
			if (mbus.ConnectionId != null)
			{
				mbus.Emit(tagDump);
			}
		}

		public void Register(long tag, TagVisualModel viewModel)
		{
			viewModels[tag] = viewModel;

			viewModel.Name = Tags[tag].Name;
			viewModel.Color = Tags[tag].Color;
			viewModel.QrCodeVisible = Tags[tag].QrCodeVisible;
		}

		public void Update(Dictionary<long, Data> namenUndFarben)
		{
			Tags = namenUndFarben;
			_gedaechtnis = new GastGedaechtnis(Tags, AllColors);

			foreach (var tag in viewModels)
			{
				viewModels[tag.Key].Name = Tags[tag.Key].Name;
				viewModels[tag.Key].Color = Tags[tag.Key].Color;
				viewModels[tag.Key].QrCodeVisible = Tags[tag.Key].QrCodeVisible;
			}

			MBusEmitTags();
			_gedaechtnis.Store();
		}
	}
}
