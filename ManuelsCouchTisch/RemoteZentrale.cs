using System;

namespace ManuelsCouchTisch
{
	public class RemoteZentrale
	{
		public readonly static Lazy<RemoteZentrale> Instance = new Lazy<RemoteZentrale>(() => new RemoteZentrale());
		private RemoteZentrale()
		{
		}

		public MBusClient MBus = new MBusClient("couchtisch");

		public void ConnectToMBus(string url = "http://mbusrelay.azurewebsites.net/signalr")
		{
			TagManagement.Instance.Value.Connect();

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

		public event Action<string> OnLog;
		public void RaiseLog(string log)
		{
			var h = OnLog;
			if (h != null) h(log);
		}

		public event Action<string> OnNewImage;
		public void RaiseNewImage(string imageAsBase64)
		{
			var h = OnNewImage;
			if (h != null) h(imageAsBase64);
		}

		private void Mbus_OnDisconnect()
		{
			RaiseLog("disconnect");
		}

		private void Mbus_On(string clientname, string message)
		{
			if (clientname.StartsWith("gastmanager.app") && message == "hallo")
			{
				RaiseLog(clientname + ": " + message);
				TagManagement.Instance.Value.MBusEmitTags();
				return;
			}

			if (message.StartsWith("<bild"))
			{
				try
				{
					RaiseLog(clientname + ": " + message.Substring(0, 18) + "...");
					var imageAsBase64 = message.Substring(18);
					RaiseNewImage(imageAsBase64);
				}
				catch (Exception e)
				{
					RaiseLog(e.Message);
				}
			}
			else if (message == "Bilder zeigen bestätigt")
			{
				RaiseLog(clientname + ": " + message);
			}
			else if (message.StartsWith("couchtisch"))
			{
				try
				{
					RaiseLog(clientname + ": " + message);
					TagManagement.Instance.Value.UpdateTags(message);
				}
				catch (Exception e)
				{
					RaiseLog(e.Message);
				}
			}
		}
	}
}
