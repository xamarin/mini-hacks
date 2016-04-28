using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyDevices.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public BaseViewModel() {
		}

		public INavigation NavInstance { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}