using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.String;

using static Newtonsoft.Json.JsonConvert;

namespace Particle.Helpers
{
	public delegate void ParticleEventHandler(object myObject, ParticleEventArgs myArgs);

	public class EventSource : IDisposable
	{
		internal EventSource(string url, string accessToken, string eventNamePrefix, int timeOutInSeconds = 30)
		{
			EventUrl = url;
			Listeners = new Dictionary<string, ParticleEventHandler>();
			AccessToken = accessToken;
			ClientTimeOut = timeOutInSeconds;
			SourceEvent.Name = eventNamePrefix;
		}

		public string EventUrl { get; internal set; }
		public Dictionary<string, ParticleEventHandler> Listeners { get; internal set; }
		public int ClientTimeOut { get; set; }
		public int LastEventID { get; internal set; }
		public string AccessToken { get; internal set; }
		public Event SourceEvent { get; internal set; } = new Event();

		public void AddEventListener(string eventName, ParticleEventHandler handler)
		{
			if (!Listeners.ContainsKey(eventName))
			{
				Listeners.Add(eventName, handler);
				OnMessage += handler;
			}
			else
				throw new Exception("Event Name is present in dictionary. Consider using a GUID to ensure a unique number is generated");
		}

		public void RemoveEventListener(string eventName)
		{
			if (Listeners[eventName] != null)
			{
				OnMessage -= Listeners[eventName];
				Listeners.Remove(eventName);
			}
			else
				throw new Exception("Event Name is not present in dictionary. Consider using a GUID to ensure a unique number is generated");
		}

		event ParticleEventHandler OnMessage;
		public event ParticleEventHandler OnError;
		public event ParticleEventHandler OnOpen;

		void ReceivedMessage(ParticleEvent receivedEvent)
		{
			if (OnMessage != null)
				OnMessage(this, new ParticleEventArgs(receivedEvent));
		}

		void ErrorReceived(string error)
		{
			if (!IsNullOrEmpty(error) && OnError != null)
				OnError(this, new ParticleEventArgs(error));
		}

		internal void CloseConnection()
		{
			isSubscribed = false;
			SourceEvent.State = EventState.Closed;
		}

		bool isSubscribed;
		public async Task StartHandlingEvents()
		{
			if (SourceEvent.State == EventState.Open)
				return;

			SourceEvent.State = EventState.Connecting;
			var url = EventUrl + "?access_token=" + AccessToken;
			string eventName = "";
			isSubscribed = true;
			try
			{
				using (var client = new HttpClient())
				{
					client.Timeout = TimeSpan.FromSeconds(ClientTimeOut);

					var request = new HttpRequestMessage(HttpMethod.Get, url);

					using (var response = await client.SendAsync(
						request,
						HttpCompletionOption.ResponseHeadersRead))
					{
						SourceEvent.State = EventState.Open;

						using (var body = await response.Content.ReadAsStreamAsync())
						using (var reader = new StreamReader(body))
						{
							if (OnOpen != null)
								OnOpen(this, new ParticleEventArgs());

							while (!reader.EndOfStream && isSubscribed)
							{
								var outputString = reader.ReadLine();
								if (outputString.Contains("event"))
								{
									eventName = outputString.Substring(6);
								}
								else if (outputString.Contains("data"))
								{
									var values = DeserializeObject<Dictionary<string, string>>(outputString.Substring(5));
									var lastevent = new ParticleEvent(values, eventName);
									ReceivedMessage(lastevent);
								}
							}
						}
					}
				}
			}
			catch (HttpRequestException e)
			{
				ErrorReceived(e.Message);
			}
		}

		public void Dispose()
		{
			EventUrl = null;
			AccessToken = null;
			SourceEvent = null;

			foreach (var listener in Listeners)
				OnMessage -= listener.Value;

			Listeners = null;
		}
	}
}