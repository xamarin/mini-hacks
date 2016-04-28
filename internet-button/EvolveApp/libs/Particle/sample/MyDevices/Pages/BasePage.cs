using Xamarin.Forms;

namespace MyDevices.Pages
{
	public class BasePage : ContentPage
	{
		public BasePage()
		{
			BackgroundColor = Color.FromHex("#2980b9");

			var labelStyle = new Style(typeof(Label))
			{
				Setters = {
					new Setter { Property = Label.TextColorProperty, Value = Color.White },
					new Setter { Property = Label.FontFamilyProperty, Value = Device.OnPlatform("AppleSDGothicNeo-Light", "Droid Sans Mono", "Comic Sans MS") },
				}
			};

			var buttonStyle = new Style(typeof(Button))
			{
				Setters = {
					new Setter { Property = Button.TextColorProperty, Value = Color.White },
					new Setter { Property = Button.FontFamilyProperty, Value = Device.OnPlatform("AppleSDGothicNeo-Light", "Droid Sans Mono", "Comic Sans MS") },
				}
			};
        	Resources = new ResourceDictionary();
			Resources.Add(labelStyle);
			Resources.Add(buttonStyle);
		}
	}
}