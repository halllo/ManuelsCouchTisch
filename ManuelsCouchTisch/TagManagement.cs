using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			Brushes.Red,
			Brushes.Green,
			Brushes.Blue,
			Brushes.Lime,
			Brushes.DeepPink,
			Brushes.DarkGray,
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

		public MBusClient MBus = new MBusClient("couchtisch");

		Dictionary<long, TagVisualModel> viewModels = new Dictionary<long, TagVisualModel>();

		public event Action<string> OnLog;
		public void RaiseLog(string log)
		{
			var h = OnLog;
			if (h != null) h(log);
		}

		public event Action OnShowDemo;
		public void RaiseShowDemo()
		{
			var h = OnShowDemo;
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

		public void ConnectToMBus(string url = "http://mbus.de:8000/signalr")
		{
			MBus.OnDisconnect += Mbus_OnDisconnect;
			MBus.On += Mbus_On;
			try
			{
				RaiseLog("connecting to " + url);
				MBus.Connect(url).Wait();
				RaiseLog("connected");
				MBus.Emit("hallo");
			}
			catch (AggregateException e)
			{
				RaiseLog("cannot connect: " + e.InnerException.Message);
			}
		}

		private void Mbus_On(string clientname, string message)
		{
			var messageItems = message.Split(new[] { ";" }, StringSplitOptions.None);
			if (messageItems[0] == "couchtisch")
			{
				RaiseLog(clientname + ": " + message);
				if (messageItems.Length == 4)
				{
					try
					{
						var tag = long.Parse(messageItems[1]);
						var name = messageItems[2];
						var color = AllColors[int.Parse(messageItems[3])];

						var viewModel = viewModels[tag];
						viewModel.Dispatch(() =>
						{
							viewModel.Name = name;
							viewModel.Color = color;
						});
					}
					catch (Exception e)
					{
						RaiseLog(e.Message);
					}
				}
			}
		}

		private void Mbus_OnDisconnect()
		{
			RaiseLog("disconnect");
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
