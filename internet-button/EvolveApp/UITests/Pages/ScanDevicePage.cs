using System;
using EvolveApp.UITests.Pages;
namespace UITests.Pages
{
	public class ScanDevicePage : BasePage
	{
		public ScanDevicePage(IApp app, Platform platform)
			: base(app, platform)
		{
		}

		public void PressScanDeviceButton()
		{
			app.WaitThenTap(x => x.Marked("scanDeviceButton"), "Press 'SCAN DEVICE' button");
		}
	}
}