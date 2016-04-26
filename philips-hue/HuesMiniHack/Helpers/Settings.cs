using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace HuesMiniHack.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string appKey = "app_key";
        private static readonly string appKeyDefault = string.Empty;

        private const string bridgeIPKey = "bridgeIP_key";
        private static readonly string bridgeIPDefault = string.Empty;

        #endregion

        public static string DefaultBridgeIP
        {
            get { return AppSettings.GetValueOrDefault<string>(bridgeIPKey, bridgeIPDefault); }
            set { AppSettings.AddOrUpdateValue<string>(bridgeIPKey, value); }
        }


        public static string AppKey
        {
            get { return AppSettings.GetValueOrDefault<string>(appKey, appKeyDefault); }
            set { AppSettings.AddOrUpdateValue<string>(appKey, value); }
        }

    }
}


