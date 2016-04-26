using System;

using Xamarin.Forms;

namespace HuesMiniHack
{
    public class App : Application
    {

        //TODO 1: Set App Name & Device Name
        public static string AppName = "";

        //Philips Hue asks that all published apps use the name of their app as the device name. 
        public static string DeviceName = "";

        public App()
        {
            // The root page of your application
            MainPage = new Views.RootPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

