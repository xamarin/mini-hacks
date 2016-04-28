using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using Particle;

using MyDevices.ViewModels;

namespace MyDevices.Pages
{
	public class DeviceDetailPage : BasePage
	{
		bool isInitialized;
		DeviceDetailViewModel ViewModel;
		ActivityIndicator indicator;
		Grid functionVariableGrid;
		AbsoluteLayout layout;
		Label nameLabel;

		public DeviceDetailPage (ParticleDevice device)
		{
			ViewModel = new DeviceDetailViewModel (device);
			indicator = new ActivityIndicator ();	

			nameLabel = new Label { 
				HorizontalOptions = LayoutOptions.Center, 
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
			};
			var width = (App.ScreenWidth - 20) / 2;

			functionVariableGrid = new Grid {
				ColumnDefinitions = new ColumnDefinitionCollection {
					new ColumnDefinition { Width = new GridLength(10,GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(width) },
					new ColumnDefinition { Width = new GridLength(width) },
					new ColumnDefinition { Width =  new GridLength(10,GridUnitType.Star) }
				},
				RowSpacing = 0
			};

			layout = new AbsoluteLayout {
				WidthRequest = App.ScreenWidth,
				HeightRequest = App.ScreenHeight
			};

			layout.Children.Add(nameLabel, new Rectangle(10, 5, App.ScreenWidth, 40));
			layout.Children.Add(indicator);
			layout.Children.Add(functionVariableGrid);

			Content = layout;
			BindingContext = ViewModel;

			if (device.Name == "InternetButton")
			{
				var flashNewProgram = new ToolbarItem { Text = "Flash" };
				flashNewProgram.Clicked += async(object sender, EventArgs e) =>
				{
					var result = await DisplayActionSheet("Pick File to Flash", "Cancel", null, "Tinker", "RGB LED", "Shake LED", "Sound Check", "Simon Says", "Follow me LED");
					var success = await ViewModel.TryFlashFileAsync(result);
					if (!success)
						await DisplayAlert("Error", "Problem flashing file, please try again", "Ok");

					if (result != "Cancel")
						await Navigation.PopAsync();
				};
				ToolbarItems.Add(flashNewProgram);
			}

			nameLabel.SetBinding (Label.TextProperty, "Device.Name");
			indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			AbsoluteLayout.SetLayoutBounds(indicator, new Rectangle(0, 0, width, height));
			AbsoluteLayout.SetLayoutBounds(functionVariableGrid, new Rectangle(10, 40, width - 20, height));

			base.LayoutChildren(x, y, width, height);
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			CreateGrid();
		}

		async Task CreateGrid()
		{
			ViewModel.IsBusy = true;

			functionVariableGrid.Children.Clear ();
			await ViewModel.RefreshDeviceAsync ();

			if (ViewModel.Device == null)
			{
				await Navigation.PopAsync();
				return;
			}

			var functions = ViewModel.Device.Functions;
			var variables = ViewModel.Variables;
			int rowCount = 1;

			functionVariableGrid.Children.Add (
				new Label { Text = functions.Count + " Functions Registered" }, 0, 3, 0, 1 
			);

			foreach (var function in functions) {

				if (function == "leds") {
					var command = new Command (o => Navigation.PushAsync (new ChangeLEDColorPage (ViewModel.Device, ViewModel.Variables)));
					functionVariableGrid.Children.Add (new Button {
						Text = function, Command = command,
						BackgroundColor = Color.Gray, 
						FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
					}, 1, 2, rowCount, rowCount + 1);
				} else if(function == "note"){
					var command = new Command (o => Navigation.PushAsync (new NotePage (ViewModel.Device)));
					functionVariableGrid.Children.Add (new Button {
						Text = function, Command = command,
						BackgroundColor = Color.Gray,
						FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
					}, 1, 2, rowCount, rowCount + 1);
				}else if(function == "startSimon"){
					var command = new Command (o => Navigation.PushAsync (new SimonSaysPage (ViewModel.Device)));
					functionVariableGrid.Children.Add (new Button {
						Text = function, Command = command, 
						BackgroundColor = Color.Gray,
						FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
					}, 1, 2, rowCount, rowCount + 1);
				}else {
					functionVariableGrid.Children.Add (new Label {
						Text = function, HorizontalTextAlignment = TextAlignment.Center
					}, 1, 2, rowCount, rowCount + 1);
				}
				rowCount++;
			}

			functionVariableGrid.Children.Add (
				new Label { Text = variables.Count + " Variables Registered", HeightRequest = 30 }, 0, 3, rowCount, rowCount + 1 
			);
			rowCount++;

			foreach (var variable in variables) {
				functionVariableGrid.Children.Add (new Label {
					Text = variable.Key, HorizontalTextAlignment = TextAlignment.Center
				}, 1, 2, rowCount, rowCount + 1);

				functionVariableGrid.Children.Add (new Label {
					Text = variable.Value, HorizontalTextAlignment = TextAlignment.Center
				}, 2, 3, rowCount, rowCount + 1);

				rowCount++;
			}

			functionVariableGrid.RowDefinitions = new RowDefinitionCollection();
			for (var x = 0; x < rowCount; x++)
				functionVariableGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
			
			layout.Children.Add (functionVariableGrid);

			ViewModel.IsBusy = false;
		}
	}
}