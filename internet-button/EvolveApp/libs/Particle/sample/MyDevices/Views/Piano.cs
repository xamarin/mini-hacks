using System;

using Xamarin.Forms;

namespace MyDevices.Views
{
	public class Piano : AbsoluteLayout
	{
		public ContentView cKey, dKey, eKey, fKey, gKey, aKey, bKey;
		public Button upOctive, downOctive;
		Label octiveLabel;

		public Piano()
		{
			WidthRequest = App.ScreenWidth;
			HeightRequest = App.ScreenHeight;

			octiveLabel = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, Rotation = 270 };
			upOctive = new Button { Text = "+", FontSize = 20 };
			downOctive = new Button { Text = "-", FontSize = 20, Rotation = 90};

			var keyHeight = (App.ScreenHeight - 140) / 7;
			var keyWidth = (App.ScreenWidth * 0.75);

			bKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "B", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};
			aKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "A", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};
			gKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "G", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};
			fKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "F", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};
			eKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "E", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};
			dKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "D", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};
			cKey = new ContentView { 
				Content = new Label { 
					TextColor = Color.Black, 
					Text = "C", 
					HorizontalOptions = LayoutOptions.End, 
					VerticalOptions = LayoutOptions.Center, 
					Rotation = 270 
				},
				Padding = new Thickness (0,0,10,0),
				BackgroundColor = Color.White,
				HeightRequest = keyHeight,
				WidthRequest = keyWidth
			};

			Children.Add(bKey, new Point(App.ScreenWidth * 0.25 - 20, 10));
			Children.Add(aKey, new Point(App.ScreenWidth * 0.25 - 20, 10 + keyHeight + 10));
			Children.Add(gKey, new Point(App.ScreenWidth * 0.25 - 20, 10 + 2 * (keyHeight + 10)));
			Children.Add(fKey, new Point(App.ScreenWidth * 0.25 - 20, 10 + 3 * (keyHeight + 10)));
			Children.Add(eKey, new Point(App.ScreenWidth * 0.25 - 20, 10 + 4 * (keyHeight + 10)));
			Children.Add(dKey, new Point(App.ScreenWidth * 0.25 - 20, 10 + 5 * (keyHeight + 10)));
			Children.Add(cKey, new Point(App.ScreenWidth * 0.25 - 20, 10 + 6 * (keyHeight + 10)));

			Children.Add(octiveLabel, new Point(10, App.ScreenHeight * 0.7));
			Children.Add(upOctive, new Rectangle(15, App.ScreenHeight * 0.5, 40, 40));
			Children.Add(downOctive, new Rectangle(15, App.ScreenHeight * 0.5 - 40, 40, 40));

			octiveLabel.SetBinding(Label.TextProperty, "OctiveText");
		}	
	}
}