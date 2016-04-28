using System;
using Xamarin.Forms;
using Particle;
using System.Threading.Tasks;
using Particle.Helpers;
namespace MyDevices.Pages
{
	public class TestPage : ContentPage
	{
		Guid id1, id2;
		Label results;

		public TestPage()
		{
			Button subscribeButton = new Button { Text = "Subscribe" };
			Button unsubscribeButton = new Button { Text = "Unsubscribe" };
			results = new Label();

			Content = new StackLayout
			{
				VerticalOptions = LayoutOptions.Center,
				Children = {
					subscribeButton,
					unsubscribeButton,
					results
				}
			};

			subscribeButton.Clicked += async (object sender, EventArgs e) =>
			{
				//var test = await ParticleCloud.SharedInstance.PublishEventWithName("test", "Data", false, 1000);
				//id1 = await ParticleCloud.SharedInstance.SubscribeToAllEventsWithPrefixAsync("test", WriteMessageToLine);
				id2 = await ParticleCloud.SharedInstance.SubscribeToMyDevicesEventsWithPrefixAsync("button-press", "380028000847343337373738", WriteMessageToLine);
			};

			unsubscribeButton.Clicked += async (object sender, EventArgs e) =>
			{
				//await ParticleCloud.SharedInstance.UnsubscribeFromEventWithIdAsync(id1);
				await ParticleCloud.SharedInstance.UnsubscribeFromEventWithIdAsync(id2);
			};

		}

		public void WriteMessageToLine(object sender, ParticleEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() => { results.Text = e.EventData.Data; });
			System.Diagnostics.Debug.WriteLine(e.EventData.Event);
			System.Diagnostics.Debug.WriteLine(e.EventData.Data);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ParticleCloud.SharedInstance.CreateOAuthClientAsync(App.Token, "xamarin");
			var response = await ParticleCloud.SharedInstance.LoginWithUserAsync("michael.watson@xamarin.com", "Da2188MW");

			//await StartPublish().ConfigureAwait(false);
		}

		async Task StartPublish()
		{
			bool keepRunning = true;

			while (keepRunning)
			{
				await ParticleCloud.SharedInstance.PublishEventWithNameAsync("MyExpensesTest", "beep", false, 60);
				await Task.Delay(1000);
			}
		}
	}
}

