// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// Source: https://github.com/Azure/azure-iot-sdks/blob/e06ad8be39e4c3bb0abe3e6ebaeed7e3bdb4edd2/csharp/service/Microsoft.Azure.Devices/Common/Security/SharedAccessSignatureBuilder.cs
namespace DeviceProvisioningUtility
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Security.Cryptography;
	using System.Text;
	using System.Net;

	public sealed class SharedAccessSignatureBuilder
	{
		string key;

		public SharedAccessSignatureBuilder()
		{
			this.TimeToLive = TimeSpan.FromMinutes(20);
		}

		public string KeyName { get; set; }

		public string Key
		{
			get
			{
				return this.key;
			}

			set
			{
				this.key = value;
			}
		}

		public string Target { get; set; }

		public TimeSpan TimeToLive { get; set; }

		public string ToSignature()
		{
			return BuildSignature(this.KeyName, this.Key, this.Target, this.TimeToLive);
		}

		static string BuildSignature(string keyName, string key, string target, TimeSpan timeToLive)
		{
			string expiresOn = BuildExpiresOn(timeToLive);
			string audience = WebUtility.UrlEncode(target);
			List<string> fields = new List<string>();
			fields.Add(audience);
			fields.Add(expiresOn);

			// Example string to be signed:
			// dh://myiothub.azure-devices.net/a/b/c?myvalue1=a
			// <Value for ExpiresOn>

			string signature = Sign(string.Join("\n", fields), key);

			// Example returned string:
			// SharedAccessSignature sr=ENCODED(dh://myiothub.azure-devices.net/a/b/c?myvalue1=a)&sig=<Signature>&se=<ExpiresOnValue>[&skn=<KeyName>]

			var buffer = new StringBuilder();
			buffer.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}={2}&{3}={4}&{5}={6}",
				"SharedAccessSignature",
				"sr", audience,
			    "sig", WebUtility.UrlEncode(signature),
			    "se", WebUtility.UrlEncode(expiresOn));

			if (!string.IsNullOrEmpty(keyName))
			{
				buffer.AppendFormat(CultureInfo.InvariantCulture, "&{0}={1}",
					"skn", WebUtility.UrlEncode(keyName));
			}

			return buffer.ToString();
		}

		static string BuildExpiresOn(TimeSpan timeToLive)
		{
			DateTime expiresOn = DateTime.UtcNow.Add(timeToLive);
			TimeSpan secondsFromBaseTime = expiresOn.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0));
			long seconds = Convert.ToInt64(secondsFromBaseTime.TotalSeconds, CultureInfo.InvariantCulture);
			return Convert.ToString(seconds, CultureInfo.InvariantCulture);
		}

		static string Sign(string requestString, string key)
		{
			using (var hmac = new HMACSHA256(Convert.FromBase64String(key)))
			{
				return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(requestString)));
			}
		}
	}
}