using System;
using Xamarin.Forms;

namespace HuesMiniHack.Views
{
    public class RootPage : TabbedPage
    {
        public RootPage()
        {
            Children.Add(new NavigationPage(new BridgePage { Title = "Bridges" }){Title = "Bridges", Icon="tabbar_bridge.png"});
            Children.Add(new NavigationPage(new LightsPage { Title = "Lamps" }){ Title = "Lights", Icon = "tabbar_light.png" });
        }
    }
}

