using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HuesMiniHack.Views
{
    public partial class LightsPage : ContentPage
    {
        public LightsPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.LightsViewModel();
        }
    }
}

