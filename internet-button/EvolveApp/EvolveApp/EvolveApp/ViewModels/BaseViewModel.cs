using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace EvolveApp.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public BaseViewModel()
		{
		}

		public INavigation NavInstance { get; set; }

		bool isBusy;
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				isBusy = value;
				OnPropertyChanged("IsBusy");
			}
		}

		public void SetPrivateBusy(bool valueToSet)
		{
			isBusy = valueToSet;
		}

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