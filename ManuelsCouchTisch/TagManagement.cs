﻿using System;
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


		public readonly static Lazy<TagManagement> Instance = new Lazy<TagManagement>(() => new TagManagement());


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

		public MBusClient MBus = new MBusClient("couchtisch");

		Dictionary<long, TagVisualModel> viewModels = new Dictionary<long, TagVisualModel>();

		private GastGedaechtnis _gedaechtnis;


		public TagManagement()
		{
			_gedaechtnis = new GastGedaechtnis(Tags, AllColors);
		}


		public event Action OnTagsChangedRemotly;
		public void RaiseTagsChangedRemotly()
		{
			var h = OnTagsChangedRemotly;
			if (h != null) h();
		}

		public event Action<string> OnLog;
		public void RaiseLog(string log)
		{
			var h = OnLog;
			if (h != null) h(log);
		}

		public event Action<byte[]> OnNewImage;
		public void RaiseNewImage(byte[] image)
		{
			var h = OnNewImage;
			if (h != null) h(image);
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

		public void ConnectToMBus(string url = "http://mbusrelay.azurewebsites.net/signalr")
		{
			_gedaechtnis.Restore();
			RaiseTagsChangedRemotly();

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
			if (clientname.StartsWith("gastmanager.app") && message == "hallo")
			{
				RaiseLog(clientname + ": " + message);
				MBusEmitTags();
				return;
			}

			if (message.StartsWith("<b"))
			{
				try
				{
					var image = message.Substring(15);
					var pixels = Convert.FromBase64String(image);
					RaiseNewImage(pixels);
				}
				catch (Exception e)
				{
					RaiseLog(e.Message);
				}
			}
			else
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
							var color = AllColors[messageItems[3]];

							var tagData = Tags[tag];
							tagData.Name = name;
							tagData.Color = color;
							tagData.QrCodeVisible = Visibility.Collapsed;
							_gedaechtnis.Store();
							RaiseTagsChangedRemotly();

							var viewModel = viewModels[tag];
							viewModel.Dispatch(() =>
							{
								viewModel.Name = name;
								viewModel.Color = color;
								viewModel.QrCodeVisible = Visibility.Collapsed;
							});
						}
						catch (Exception e)
						{
							RaiseLog(e.Message);
						}
					}
				}
			}
		}

		private void MBusEmitTags()
		{
			var tagDump = _gedaechtnis.AsTagDump();

			if (MBus.ConnectionId != null)
			{
				MBus.Emit(tagDump);
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
