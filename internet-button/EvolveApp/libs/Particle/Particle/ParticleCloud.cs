using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.String;
using static System.Diagnostics.Debug;
using System.Threading;
using ModernHttpClient;
using static Newtonsoft.Json.JsonConvert;

using Particle.Models;
using Particle.Helpers;
using System.Threading;

namespace Particle
{
	public class ParticleCloud : IDisposable
	{
		#region Constants

		readonly string CLIENT_URI_ENDPOINT = "https://api.particle.io/v1/clients/";
		readonly string TOKEN_URI_ENDPOINT = "https://api.particle.io/oauth/token/";
		readonly string USER_URI_ENDPOINT = "https://api.particle.io/v1/users/";
		readonly string DEVICE_URI_ENDPOINT = "https://api.spark.io/v1/devices/";

		#endregion

		#region Constructors

		public ParticleCloud()
		{
			OAuthClientId = "particle";
			OAuthClientSecret = "particle";
		}

		public ParticleCloud(string accessToken, string refreshToken, DateTime expiration)
		{
			AccessToken = new ParticleAccessToken(accessToken, refreshToken, expiration);
			OAuthClientId = "particle";
			OAuthClientSecret = "particle";
		}

		#endregion

		#region Properties

		public static ParticleCloud SharedInstance { get; internal set; } = new ParticleCloud();
		public string LoggedInUsername { get; internal set; }
		public bool IsLoggedIn { get; internal set; }
		public static ParticleAccessToken AccessToken { get; set; }
		public string OAuthClientId { get; internal set; } = "particle";
		public string OAuthClientSecret { get; internal set; } = "particle";
		public Dictionary<Guid, EventSource> SubscibedEvents { get; internal set; } = new Dictionary<Guid, EventSource>();

		#endregion

		#region IDisposable Implementation

		public void Dispose()
		{
			LoggedInUsername = null;
			AccessToken = null;
			OAuthClientId = null;
			OAuthClientSecret = null;
			SharedInstance = null;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates the OAuth client and sets the OAuth Client Id and Secret if successful.
		/// </summary>
		/// <returns>A boolean indicating whether the OAuth client was created successfully or not.</returns>
		/// <param name="accessToken">Access token.</param>
		/// <param name="appName">Authorized app name.</param>
		public async Task<bool> CreateOAuthClientAsync(string appName, string accessToken = null)
		{
			var requestContent = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string> ("name", appName),
				new KeyValuePair<string, string> ("type", "installed"),

			});

			if (accessToken != null)
				requestContent.Headers.Add("access_token", accessToken);

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(CLIENT_URI_ENDPOINT, requestContent);
					var particleResponse = DeserializeObject<ParticleOAuthResponse>(await response.Content.ReadAsStringAsync());

					if (particleResponse.Success)
					{
						OAuthClientId = particleResponse.ClientDetails.Id;
						OAuthClientSecret = particleResponse.ClientDetails.Secret;
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
		/// Log in with a given username or password.
		/// </summary>
		/// <returns>A boolean indicating whether the login attempt was successful or not.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public async Task<bool> LoginWithUserAsync(string username, string password)
		{
			var requestContent = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string> ("grant_type", "password"),
				new KeyValuePair<string, string> ("username", username),
				new KeyValuePair<string, string> ("password", password),
				new KeyValuePair<string, string> ("client_id", OAuthClientId),
				new KeyValuePair<string, string> ("client_secret", OAuthClientSecret),
			});

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(TOKEN_URI_ENDPOINT, requestContent);
					var particleResponse = DeserializeObject<ParticleLoginResponse>(await response.Content.ReadAsStringAsync());

					if (particleResponse.AccessToken != null || !IsNullOrEmpty(particleResponse.AccessToken))
					{
						AccessToken = new ParticleAccessToken(particleResponse);
						LoggedInUsername = username;
						IsLoggedIn = true;
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
		/// Logout the current user.
		/// </summary>
		public void Logout()
		{
			AccessToken = null;
			LoggedInUsername = null;
			IsLoggedIn = false;
		}
		/// <summary>
		/// Sign-up a new user with Particle.
		/// </summary>
		/// <returns>A string of either "Success" or the specific error</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		public async Task<string> SignupWithUserAsync(string username, string password)
		{
			ParticleGeneralResponse particleResponse = null;

			var requestContent = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string> ("username", username),
				new KeyValuePair<string, string> ("password", password),
			});

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(USER_URI_ENDPOINT, requestContent);
					particleResponse = DeserializeObject<ParticleGeneralResponse>(await response.Content.ReadAsStringAsync());

					if (particleResponse.Ok)
						return "Success";
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return particleResponse.Errors[0] ?? "UnknownError";
		}

		//TODO Still need to complete
		public async Task SignUpWithCusomterAsync(string email, string password, string orgSlig)
		{
			throw new NotImplementedException();
		}
		public async Task RequestPasswordResetForCustomerAsync(string orgSlug, string email)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Requests the password reset for user.
		/// </summary>
		/// <returns>A boolean indicating success or failure on password reset request</returns>
		/// <param name="email">Email of user to reset password</param>
		//public async Task<bool> RequestPasswordResetForUserAsync(string email)
		//{
		//	if (IsNullOrWhiteSpace(email))
		//	{
		//		throw new ArgumentNullException(nameof(email));
		//	}

		//	try
		//	{
		//		using (var client = new HttpClient(new NativeMessageHandler()))
		//		{
		//			HttpResponseMessage response = await client.PostAsync(method, new FormUrlEncodedContent(new KeyValuePair<string, string>("username", email)));
		//			string str = await response.Content.ReadAsStringAsync();

		//			if (str.Contains("\"ok\": true"))
		//				return true;
		//		}
		//	}
		//	catch (HttpRequestException e)
		//	{
		//	}
		//}
		/// <summary>
		/// Gets a list of the users registered Particle Devices..
		/// </summary>
		/// <returns>A list of the users Particle Devices.</returns>
		public async Task<List<ParticleDevice>> GetDevicesAsync()
		{
			if (IsNullOrEmpty(AccessToken.Token) || AccessToken.Token == "expired")
				return null;

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.GetAsync(DEVICE_URI_ENDPOINT + "?access_token=" + AccessToken.Token);
					var responseText = await response.Content.ReadAsStringAsync();

					if (responseText.Contains("error"))
						return null;

					var particleArgs = DeserializeObject<List<ParticleDeviceObject>>(responseText);
					var particleDevices = new List<ParticleDevice>();

					foreach (var device in particleArgs)
						particleDevices.Add(new ParticleDevice(device));

					return particleDevices;
				}

			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return null;
		}
		/// <summary>
		/// Gets a Particle Devices from it's unique ID.
		/// </summary>
		/// <returns>The Particle Device.</returns>
		/// <param name="deviceId">Particle Device Unique Id.</param>
		public async Task<ParticleDevice> GetDeviceAsync(string deviceId)
		{
			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.GetAsync(
						DEVICE_URI_ENDPOINT + deviceId + "/?access_token=" + AccessToken.Token);
					var particleArgs = DeserializeObject<ParticleDeviceObject>(await response.Content.ReadAsStringAsync());

					if (particleArgs != null)
						return new ParticleDevice(particleArgs);
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
				return null;
			}

			return null;
		}
		/// <summary>
		/// Refresh the ParticleCloud Access Token. The AccessToken on the ParticleCloud instance will be updated. 
		/// A return value of Null indicates the refresh was unsuccessful.
		/// </summary>
		/// <returns>The AccessToken.</returns>
		/// <param name="appName">App name.</param>
		public async Task<ParticleAccessToken> RefreshTokenAsync(string appName)
		{
			var requestContent = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string> ("name", appName),
				new KeyValuePair<string, string> ("type", "installed"),
				new KeyValuePair<string, string> ("grant_type", "refresh_token"),
				new KeyValuePair<string, string> ("client_id", OAuthClientId),
				new KeyValuePair<string, string> ("client_secret", OAuthClientSecret),
				new KeyValuePair<string, string> ("access_token", AccessToken.Token),
				new KeyValuePair<string, string> ("refresh_token", AccessToken.RefreshToken),
			});

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(
						TOKEN_URI_ENDPOINT + "?refresh_token=" + AccessToken.RefreshToken,
						requestContent
					);

					var particleResponse = DeserializeObject<ParticleLoginResponse>(await response.Content.ReadAsStringAsync());

					if (particleResponse.AccessToken != null || !IsNullOrEmpty(particleResponse.AccessToken))
					{
						AccessToken = new ParticleAccessToken(particleResponse);
						IsLoggedIn = true;
						return AccessToken;
					}
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return null;
		}
		/// <summary>
		/// Claims an unregistered Particle Device.
		/// </summary>
		/// <returns>A boolean indicating whether the device was successfully claimed or not.</returns>
		/// <param name="deviceId">Particle Device Unique Id.</param>
		public async Task<bool> ClaimDeviceAsync(string deviceId)
		{
			var requestContent = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("id", deviceId) });

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(
						DEVICE_URI_ENDPOINT + "?access_token=" + AccessToken.Token,
						requestContent);

					var particleResponse = DeserializeObject<ParticleFunctionResponse>(await response.Content.ReadAsStringAsync());

					if (particleResponse.Connected)
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
		/// Unsubscribes from event based on its unique identifier.
		/// </summary>
		/// <returns>The raw event data.</returns>
		/// <param name="eventId">Event Subscription Unique Identifier</param>
		public async Task<Event> UnsubscribeFromEventWithIdAsync(Guid eventId)
		{
			var eventSource = SubscibedEvents[eventId];

			if (eventSource == null)
				return new Event("Event Not Found");

			eventSource.CloseConnection();
			var eventRaw = eventSource.SourceEvent;
			eventSource.Dispose();

			SubscibedEvents.Remove(eventId);

			return eventRaw;
		}
		/// <summary>
		/// Subscribes to events with a given prefix.
		/// </summary>
		/// <returns>A Guid that uniquely identifies the subscription</returns>
		/// <param name="eventNamePrefix">Event name prefix.</param>
		/// <param name="handler">ParticleEventHandler to invoke as events are received</param>
		public async Task<Guid> SubscribeToAllEventsWithPrefixAsync(string eventNamePrefix, ParticleEventHandler handler)
		{
			string endpoint;
			if (IsNullOrEmpty(eventNamePrefix))
				endpoint = "https://api.particle.io/v1/events/";
			else
				endpoint = $"https://api.particle.io/v1/events/{eventNamePrefix}";

			var eventListenerId = await subscribeToEventWithUrlAsync(endpoint, handler, eventNamePrefix);

			return eventListenerId;
		}
		/// <summary>
		/// Subscribes to events with a given prefix.
		/// </summary>
		/// <returns>A Guid that uniquely identifies the subscription</returns>
		/// <param name="eventNamePrefix">Event name prefix.</param>
		/// <param name="deviceID">Device identifier.</param>
		/// <param name="handler">ParticleEventHandler to invoke as events are received</param>
		public async Task<Guid> SubscribeToMyDevicesEventsWithPrefixAsync(string eventNamePrefix, string deviceID, ParticleEventHandler handler)//add event handler
		{
			string endpoint;
			if (IsNullOrEmpty(eventNamePrefix))
				endpoint = $"https://api.particle.io/v1/devices/{deviceID}/events/";
			else
				endpoint = $"https://api.particle.io/v1/devices/events/{eventNamePrefix}";

			var eventListenerId = await subscribeToEventWithUrlAsync(endpoint, handler, eventNamePrefix);

			return eventListenerId;
		}
		/// <summary>
		/// Publishes the event to the ParticleCloud.
		/// </summary>
		/// <returns>Either 'ok' or 'Error'</returns>
		/// <param name="eventName">Event name.</param>
		/// <param name="data">Event Data.</param>
		/// <param name="isPrivate">If you wish this event to be publicly visible.</param>
		/// <param name="timeToLive">How long the event should persist.</param>
		public async Task<string> PublishEventWithNameAsync(string eventName, string data, bool isPrivate, int timeToLive)
		{
			string privateString = "";

			if (isPrivate)
				privateString = "true";
			else
				privateString = "false";

			var requestContent = new KeyValuePair<string, string>[] {
				new KeyValuePair<string, string> ("name", eventName),
				new KeyValuePair<string, string> ("data", data),
				new KeyValuePair<string, string> ("ttl", timeToLive.ToString()),
				new KeyValuePair<string, string> ("isPrivate", privateString),
			};

			try
			{
				using (var client = new HttpClient(new NativeMessageHandler()))
				{
					var response = await client.PostAsync(
						"https://api.particle.io/v1/devices/events" + "?access_token=" + AccessToken.Token,
						new FormUrlEncodedContent(requestContent)
					);
					var particleResponse = await response.Content.ReadAsStringAsync();

					if (particleResponse.Contains("\"ok\": true"))
						return "Ok";

					return "Error";
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			return "Error";
		}

		#endregion

		async Task<Guid> subscribeToEventWithUrlAsync(string url, ParticleEventHandler handler, string eventNamePrefix)
		{
			var guid = Guid.NewGuid();
			var source = new EventSource(url, AccessToken.Token, eventNamePrefix);
			source.AddEventListener(guid.ToString(), handler);

			await Task.Factory.StartNew(() => source.StartHandlingEvents().ConfigureAwait(false), TaskCreationOptions.LongRunning);

			SubscibedEvents.Add(guid, source);
			return guid;
		}
	}
}