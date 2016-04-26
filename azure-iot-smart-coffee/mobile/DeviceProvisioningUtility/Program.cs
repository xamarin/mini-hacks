using System;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace DeviceProvisioningUtility
{
	class MainClass
	{
		static RegistryManager registryManager;

		// TODO #1: Insert your own IoT Hub here.
		static string iotHubUrl = ""; // Example: "PierceHub.azure-devices.net"
		static string policyName = ""; // Example: "iothubowner"
		static string policyKey = ""; // Example: SPHff77vetNDgrZahijsTMhGqZd0MllkgG0JyLzfz2E=
		static string connectionString = ""; // Example: HostName=PierceHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=SPHff77vetNDgrZahijsTMhGqZd0MllkgG0JyLzfz2E=

		public static void Main(string[] args)
		{
			Console.WriteLine("Device Provisioning Tool");
			Console.WriteLine("Creating a unique device identity for device: coffeeMaker");
			registryManager = RegistryManager.CreateFromConnectionString(connectionString);
			AddDeviceAsync().Wait();
			Console.WriteLine("Creating a unique Shared Access Signature for your account:");

			var signature = new SharedAccessSignatureBuilder()
			{
				KeyName = policyName,
				Key = policyKey,
				Target = iotHubUrl,
				TimeToLive = TimeSpan.FromDays(365)
			}.ToSignature();

			Console.WriteLine(signature);
			Console.ReadLine();
		}

		static async Task AddDeviceAsync()
		{
			Device device;
			try
			{
				device = await registryManager.AddDeviceAsync(new Device("coffeeMaker"));
			}
			catch (DeviceAlreadyExistsException)
			{
				device = await registryManager.GetDeviceAsync("coffeeMaker");
			}

			Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
		}
	}
}
