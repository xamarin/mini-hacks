using System;
using System.ComponentModel;

using Amqp;
using Amqp.Framing;
using Amqp.Types;

using System.Threading.Tasks;

namespace SmartCoffee
{
	public class SmartCoffeeService : IDisposable
	{
		static ConnectionFactory connectionFactory;

		Connection connection;
		Session session;

		private SmartCoffeeService(Connection connection)
		{
			this.connection = connection;
			this.session = new Session(connection);
		}

		public static async Task<SmartCoffeeService> GetCoffeeServiceApi()
		{
			// TODO #4: Create a connection with Azure IoT Hub.

			return null;
		}

		public async Task SendMessageToDevice(string messageToDevice)
		{
			var sender = new SenderLink(session, "sender-link", "/messages/devicebound");

			var message = new Message(System.Text.Encoding.UTF8.GetBytes(messageToDevice));
			message.Properties = new Properties();
			message.Properties.To = $"/devices/{Constants.DeviceId}/messages/devicebound";
			message.Properties.MessageId = Guid.NewGuid().ToString();
			message.ApplicationProperties = new ApplicationProperties();
			message.ApplicationProperties["iothub-ack"] = "full";

			await sender.SendAsync(message);
			await sender.CloseAsync();
		}

		public async Task<bool> ReceiveMessageFromDevice()
		{
			var receiver = new ReceiverLink(session, "receive-link",
								 "/messages/servicebound/feedback");
			while (true)
			{
				var message = await receiver.ReceiveAsync();
				if (message == null)
					continue;
				
				receiver.Accept(message);

				return true;
			}
		}

		async static Task<bool> PutCbsToken(Connection connection, string host, string shareAccessSignature, string audience)
		{
			bool result = true;
			Session session = new Session(connection);

			string cbsReplyToAddress = "cbs-reply-to";
			var cbsSender = new SenderLink(session, "cbs-sender", "$cbs");
			var cbsReceiver = new ReceiverLink(session, cbsReplyToAddress, "$cbs");

			// construct the put-token message
			var request = new Message(shareAccessSignature);
			request.Properties = new Properties();
			request.Properties.MessageId = Guid.NewGuid().ToString();
			request.Properties.ReplyTo = cbsReplyToAddress;
			request.ApplicationProperties = new ApplicationProperties();
			request.ApplicationProperties["operation"] = "put-token";
			request.ApplicationProperties["type"] = "azure-devices.net:sastoken";
			request.ApplicationProperties["name"] = audience;
			await cbsSender.SendAsync(request);

			// receive the response
			var response = await cbsReceiver.ReceiveAsync();
			if (response == null || response.Properties == null || response.ApplicationProperties == null)
			{
				result = false;
			}
			else {
				int statusCode = (int)response.ApplicationProperties["status-code"];
				string statusCodeDescription = (string)response.ApplicationProperties["status-description"];
				if (statusCode != (int)202 && statusCode != (int)200) // !Accepted && !OK
				{
					result = false;
				}
			}

			// the sender/receiver may be kept open for refreshing tokens
			await cbsSender.CloseAsync();
			await cbsReceiver.CloseAsync();
			await session.CloseAsync();

			return result;
		}

		public async void Dispose()
		{
			await session.CloseAsync();
			await connection.CloseAsync();
		}
	}
}
