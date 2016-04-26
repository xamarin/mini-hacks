using System;
using System.Collections.Generic;
using Q42.HueApi;
using Xamarin.Forms;

namespace HuesMiniHack.Views
{
    public partial class LightPage : ContentPage
    {
        public LightPage(Light selectedLight)
        {
            InitializeComponent();

            var ViewModel = new ViewModels.LightViewModel(selectedLight);
            BindingContext = ViewModel;
        }
    }
}

