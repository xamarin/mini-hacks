using System;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Nito.AsyncEx;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Console;
using static System.Net.WebUtility;

namespace Console
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			AsyncContext.Run(() => MainAsync(args));
		}

		static async void MainAsync(string[] args)
		{
			string uri = "InternetButtonHub.azure-devices.net";
			string key = "Ee21Q4xWy+M0XXfq2PPUW1Vd1eY0u7JQ4ijQcC1Q8QM=";
			//string keyName = "iothubowner";
			string expiry = DateTime.Now.Add(TimeSpan.FromHours(72)).ToUnixTime().ToString();
			string uriStringEncoded = System.Net.WebUtility.UrlEncode(uri);
			string stringToSign = uriStringEncoded + "\n" + expiry;

			WriteLine("String to sign: " + stringToSign);

			string signature = "";

			using (HMACSHA256 hMACSHA = new HMACSHA256(Convert.FromBase64String(key)))
			{
				signature = Convert.ToBase64String(hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
			}

			WriteLine("Signature: " + signature);

			string formData =
				"{" +
					"\"gameid\": \"{{g}}\"," +
					"\"activity\": \"{{a}}\"," +
					"\"user\": \"{{u}}\"," +
					"\"value\": \"{{v}}\"," +
					"\"timecreated\": \"{{SPARK_PUBLISHED_AT}}\"," +
					"\"guid\": \"{{SPARK_CORE_ID}}\"" +
				"}";

			WriteLine(formData);

			//Created from Device Explorer
			string deviceId = "deviceeaa589217f034e06990ca83d92af3583";
			//Primary Key: Ee21Q4xWy+M0XXfq2PPUW1Vd1eY0u7JQ4ijQcC1Q8QM=
			//Connection String: HostName=InternetButtonHub.azure-devices.net;DeviceId=deviceeaa589217f034e06990ca83d92af3583;SharedAccessKey=Ee21Q4xWy+M0XXfq2PPUW1Vd1eY0u7JQ4ijQcC1Q8QM=;GatewayHostName=ssl://InternetButtonHub.azure-devices.net:8883
			//Documentation for SAS Token
			//https://azure.microsoft.com/en-us/documentation/articles/iot-hub-sas-tokens/
			//var sasToken = String.Format(CultureInfo.InvariantCulture,
			//	 $"SharedAccessSignature sig={UrlEncode(signature)}&se={expiry}&sr={uri}");//&skn={keyName}

			var sasToken = "HostName=InternetButtonHub.azure-devices.net;DeviceId=deviceeaa589217f034e06990ca83d92af3583;SharedAccessSignature=SharedAccessSignature sr=InternetButtonHub.azure-devices.net%2fdevices%2fdeviceeaa589217f034e06990ca83d92af3583&sig=XmMwMXvVYcp3QSSOD%2fwTiwH7Nak%2fzJIYoSpYaFWiH5U%3d&se=1460992358";

			WriteLine(sasToken);

			//sb://iothub-ns-internetbu-30095-cf492eeb30.servicebus.windows.net/InternetButtonHub/messages
			var requestContent = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string> ("event", "SimonSays"),
				new KeyValuePair<string, string> ("url", $"https://InternetButtonHub.azure-devices.net/devices/{deviceId}/messages/events"),
				new KeyValuePair<string, string> ("auth", sasToken),
				new KeyValuePair<string, string> ("json", formData),
				//new KeyValuePair<string, string> ("azure_sas_token", "{\"key_name\": \"iothubowner\", \"key\": \"jav+H0wjsg+HCMwKoFIDueUXu9O87pcU9bjAV5ulFbw=\"}"),
				new KeyValuePair<string, string> ("access_token", "7770f37e1e767f9eb4a72ca81a933b4026957e02"),
				new KeyValuePair<string, string> ("mydevices","true"),
				new KeyValuePair<string, string> ("nodefaults","true"),
			});

			try
			{
				using (var client = new HttpClient())
				{
					var response = await client.PostAsync(
						"https://api.particle.io/v1/webhooks",
						//"http://requestb.in/t6ia1yt6",
						requestContent
					);

					var particleResponse = await response.Content.ReadAsStringAsync();
					WriteLine(particleResponse);
				}
			}
			catch (Exception e)
			{
				WriteLine(e.Message);
			}

			Read();
		}
	}
}