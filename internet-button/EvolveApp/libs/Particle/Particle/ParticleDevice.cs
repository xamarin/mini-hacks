using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.String;
using static System.Diagnostics.Debug;

using ModernHttpClient;
using static Newtonsoft.Json.JsonConvert;

using Particle.Models;
using Particle.Helpers;

namespace Particle
{
	public enum ParticleDeviceType
	{
		Core = 0,
		Photon = 6,
		Electron = 10
	}

	public class ParticleDevice
	{
		#region Constants

		readonly int MAX_SPARK_FUNCTION_ARG_LENGTH = 63;
		readonly string DEVICE_URI_ENDPOINT = "https://api.spark.io/v1/devices/";

		#endregion

		#region Constructors 

		public ParticleDevice(ParticleDeviceObject attributes)
		{
			if (attributes.Name == "" || attributes.Name == null)
				throw new ArgumentNullException("Attributes");

			Id = attributes.Id;
			Name = attributes.Name;
			Connected = attributes.Connected;

			Variables = attributes.Variables;
			Functions = attributes.Functions;

			Version = attributes.CC3000PatchVersion;

			if (attributes.LastHeard != null)
				LastHeard = DateTime.Parse(attributes.LastHeard);

			RequiresUpdate = false;

			if (attributes.RequiresUpdate)
				RequiresUpdate = true;
		}

		#endregion

		#region Public Properties

		public string Category { get; internal set; }
		public string Id { get; internal set; }
		public string Name { get; internal set; }
		public bool Connected { get; set; }
		public List<string> Functions { get; internal set; }
		public Dictionary<string, string> Variables { get; internal set; }
		public string LastApp { get; internal set; }
		public DateTime LastHeard { get; internal set; }
		public bool IsFlashing { get; internal set; }
		public string Version { get; internal set; }
		public bool RequiresUpdate { get; internal set; }
		public ParticleDeviceType Type { get; internal set; }

		#endregion

		#region public Methods
		/// <summary>
		/// Gets the value of the requested variable registered on Particle Device. 
		/// Specific Variable will not be updated in 'Variable'. 
		/// Use RefreshAsync() to update all the 'Variable' Values.
		/// </summary>
		/// <returns>The value of the requested variable registered on Particle Device</returns>
		/// <param name="variableName">Variable name trying to get value of</param>
		public async Task<ParticleVariableResponse> GetVariableAsync(string variableName)
		{
			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					client.Timeout = TimeSpan.FromSeconds(3);
					var response = await client.GetAsync(
						DEVICE_URI_ENDPOINT + Id + "/" + variableName + "?access_token=" + ParticleCloud.AccessToken.Token);
					var particleArgs = DeserializeObject<ParticleVariableResponse>(await response.Content.ReadAsStringAsync());

					LastApp = particleArgs?.Core?.LastApp;
					return particleArgs ?? null;
				}

			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return null;
		}
		/// <summary>
		/// Calls the registered function from the Particle Device.
		/// </summary>
		/// <returns>The function async.</returns>
		/// <param name="functionName">Name of the function to invoke on Particle Device</param>
		/// <param name="args">Parameter to be passed into the Particle Device function</param>
		public async Task<string> CallFunctionAsync(string functionName, string args = null)
		{
			if (args?.Length > MAX_SPARK_FUNCTION_ARG_LENGTH)
				throw new Exception("Argument length exceeded maximum allowable");

			var requestContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("arg", args) });

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(
						DEVICE_URI_ENDPOINT + Id + "/" + functionName + "?access_token=" + ParticleCloud.AccessToken.Token,
						requestContent);
					var responseText = await response.Content.ReadAsStringAsync();
					var particleResponse = DeserializeObject<ParticleFunctionResponse>(responseText);

					if (IsNullOrEmpty(particleResponse.Id))
					{
						var error = DeserializeObject<ParticleErrorResponse>(responseText);
						return error.Error;
					}

					return particleResponse?.Value.ToString() ?? "-1";
				}

			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}
			return "-1";
		}
		/// <summary>
		/// Refreshs the device. This includes the varibles and functions on the device. 
		/// A return value of false indicates that ParticleCloud could not retreive the given device.
		/// </summary>
		/// <returns>A boolean indicating whether the refresh was successful or not.</returns>
		public async Task<bool> RefreshAsync()
		{
			var updatedDevice = await ParticleCloud.SharedInstance.GetDeviceAsync(Id);

			if (updatedDevice == null)
				return false;

			Id = updatedDevice?.Id;
			Name = updatedDevice?.Name;
			RequiresUpdate = updatedDevice.RequiresUpdate;
			Variables = updatedDevice?.Variables;
			Functions = updatedDevice?.Functions;
			Version = updatedDevice?.Version;
			LastHeard = updatedDevice.LastHeard;

			return true;
		}
		/// <summary>
		/// Sets the name of the device.
		/// </summary>
		/// <returns>A boolean indicating whether the name change was successful or not.</returns>
		/// <param name="name">Name.</param>
		public async Task<bool> SetNameAsync(string name)
		{
			var requestContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("name", name) });

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PutAsync(
						DEVICE_URI_ENDPOINT + Id + "?access_token=" + ParticleCloud.AccessToken.Token,
						requestContent);
					var particleArgs = DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());

					if (name == particleArgs["name"])
					{
						await RefreshAsync();
						return true;
					}
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return false;
		}
		/// <summary>
		/// Flashs the known app based on it's given name.
		/// </summary>
		/// <returns>A boolean indicating whether the flash was successful or not.</returns>
		/// <param name="knownAppName">Known app name.</param>
		public async Task<bool> FlashKnownAppAsync(string knownAppName)
		{
			var requestContent = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string> ("app", knownAppName),
			});

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PutAsync(
						DEVICE_URI_ENDPOINT + Id + "?access_token=" + ParticleCloud.AccessToken.Token,
						requestContent);
					var particleArgs = DeserializeObject<ParticleFlashResponse>(await response.Content.ReadAsStringAsync());

					if (particleArgs.Status == "Update started")
						return true;
				}

			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return false;
		}
		/// <summary>
		/// Unclaims the Particle Device.
		/// </summary>
		/// <returns>A boolean indicating whether the Particle Device was unclaimed successful or not.</returns>
		public async Task<bool> UnclaimAsync()
		{
			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.DeleteAsync(
						DEVICE_URI_ENDPOINT + Id + "?access_token=" + ParticleCloud.AccessToken.Token
					);
					var particleArgs = DeserializeObject<ParticleGeneralResponse>(await response.Content.ReadAsStringAsync());

					if (particleArgs.Ok)
						return true;
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return false;
		}
		/// <summary>
		/// Flash a binary file to the ParticleDevice. 
		/// </summary>
		/// <returns>A boolean indicating whether the Particle Device was successfully flashed or not.</returns>
		/// <param name="fileBytes">File in the form of a byte array</param>
		/// <param name="filename">name of the binary file being flashed. This should include the '.bin'</param>
		public async Task<bool> FlashFilesAsync(byte[] fileBytes, string filename)
		{
			var byteContent = new ByteArrayContent(fileBytes);
			byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

			MultipartFormDataContent content = new MultipartFormDataContent();
			content.Add(byteContent, "\"file\"", filename);

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PutAsync(
						DEVICE_URI_ENDPOINT + Id + "?access_token=" + ParticleCloud.AccessToken.Token,
						content);
					response.EnsureSuccessStatusCode();

					var particleArgs = DeserializeObject<ParticleFlashResponse>(await response.Content.ReadAsStringAsync());

					if (particleArgs.Status == "Update started")
						return true;
				}

			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return false;
		}
		/// <summary>
		/// Subscribes to events with a given prefix.
		/// </summary>
		/// <returns>A Guid that uniquely identifies the subscription</returns>
		/// <param name="eventNamePrefix">Event name prefix.</param>
		/// <param name="handler">ParticleEventHandler to invoke as events are received</param>
		public async Task<Guid> SubscribeToEventsWithPrefixAsync(string eventNamePrefix, ParticleEventHandler handler)
		{
			string endpoint;
			if (IsNullOrEmpty(eventNamePrefix))
				endpoint = $"https://api.particle.io/v1/devices/{Id}/events/";
			else
				endpoint = $"https://api.particle.io/v1/devices/{Id}/events/{eventNamePrefix}";

			var eventListenerId = await subscribeToEventWithUrlAsync(endpoint, handler, eventNamePrefix);
			System.Diagnostics.Debug.WriteLine(endpoint);
			return eventListenerId;
		}
		/// <summary>
		/// Unsubscribes from event based on its unique identifier.
		/// </summary>
		/// <returns>The raw event data.</returns>
		/// <param name="eventId">Event Subscription Unique Identifier</param>
		public async Task<Event> UnsubscribeToEventsWithIdAsync(Guid eventId)
		{
			return await ParticleCloud.SharedInstance.UnsubscribeFromEventWithIdAsync(eventId);
		}

		#endregion

		async Task<Guid> subscribeToEventWithUrlAsync(string url, ParticleEventHandler handler, string eventNamePrefix)
		{
			var guid = Guid.NewGuid();
			var source = new EventSource(url, ParticleCloud.AccessToken.Token, eventNamePrefix);
			source.AddEventListener(guid.ToString(), handler);

			await Task.Factory.StartNew(() => source.StartHandlingEvents().ConfigureAwait(false), TaskCreationOptions.LongRunning);

			ParticleCloud.SharedInstance.SubscibedEvents.Add(guid, source);
			return guid;
		}
	}
}