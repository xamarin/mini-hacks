using System;
using EvolveApp.UITests.Pages;
using System.Threading.Tasks;
namespace UITests.Pages
{
	public enum IoTapp
	{
		SimonSays,
		RGBLED,
		ShakeLED,
		FollowMeLED
	}

	public class DeviceLandingPage : BasePage
	{
		public DeviceLandingPage(IApp app, Platform platform)
			: base(app, platform)
		{
		}

		public void StartInteraction()
		{
			app.WaitThenTap(x => x.Marked("startInteractionButton"), "Pressed 'START INTERACTION' button");
		}

		public void FlashFileAsync(IoTapp iotapp)
		{
			app.WaitThenTap(x => x.Marked("flashBinaryButton"), "Pressed the 'FLASH NEW APP' button");
			app.Repl();

			Task.Delay(TimeSpan.FromSeconds(30));
			app.Screenshot("Flash Results");

			app.Repl();

		}


	}
}

