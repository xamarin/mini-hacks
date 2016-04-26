using System;
using EvolveApp.UITests.Pages;
using System.Threading.Tasks;
using System.Threading;
namespace UITests.Pages
{
	public class SimonSaysPage : BasePage
	{
		public SimonSaysPage(IApp app, Platform platform)
			: base(app, platform)
		{
		}
		//using Olive as tester
		public string SimonMove { get; set; }
		int playerMoveCount;

		public void StartGame()
		{
			playerMoveCount = 0;
			app.Tap(x => x.Marked("actionButton"), "Submit Correct Move");
		}

		public void PlayCorrectMove()
		{
			playerMoveCount++;

			for (var x = 0; x < playerMoveCount; x++)
			{
				var move = SimonMove.Substring(x - 1, 1);
				switch (move)
				{
					case "r":
						PressRedButton();
						break;
					case "b":
						PressBlueButton();
						break;
					case "y":
						PressYellowButton();
						break;
					case "g":
						PressGreenButton();
						break;
				}

				//Sleep because there is a delay in how fast we allow the button to be pressed
				//A mobile phone can move a lot faster than low-level IoT device
				Thread.Sleep(250);
			}

			app.Tap(x => x.Marked(""), "Submit Correct Move");
		}

		public void PlayIncorrectMove()
		{
			app.WaitForThenTap(x => x.Marked("clearMoveButton"));
			app.Tap(x => x.Marked("actionButton"), "Submit Correct Move");
		}

		public void ClearPlayerEntry()
		{
			app.WaitThenTap(x => x.Marked("clearMoveButton"), "Clear current entry");
		}

		void PressRedButton()
		{
			app.WaitThenTap(x => x.Marked("red"), "Press Red Button");
		}

		void PressBlueButton()
		{
			app.WaitThenTap(x => x.Marked("blue"), "Press Blue Button");
		}

		void PressGreenButton()
		{
			app.WaitThenTap(x => x.Marked("green"), "Press Green Button");
		}

		void PressYellowButton()
		{
			app.WaitThenTap(x => x.Marked("yellow"), "Press Yellow Button");
		}
	}
}

