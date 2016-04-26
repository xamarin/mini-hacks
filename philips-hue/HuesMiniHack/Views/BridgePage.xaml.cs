using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace HuesMiniHack.Views
{
    public partial class BridgePage : ContentPage
    {
        public BridgePage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.BridgeViewModel();
        }
    }
}

