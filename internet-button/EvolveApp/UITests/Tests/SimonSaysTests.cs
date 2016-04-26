using System;
using Xamarin.UITest;
using UITests.Pages;

namespace UITests.Tests
{
	public class SimonSaysTests : AbstractSetup
	{
		public SimonSaysTests(Platform platform)
			: base(platform)
		{
		}

		public override void BeforeEachTest()
		{
			base.BeforeEachTest();
		}

		public void GetToSimonGame()
		{
			app.WaitFor(x => x.Marked("scanDeviceButton"));

			if (app is Xamarin.UITest.iOS.iOSApp)
				app.Invoke("skipScanner:", "");


		}
	}
}