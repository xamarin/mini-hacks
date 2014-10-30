using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EstimoteSdk;
using Java.Util.Concurrent;

namespace aBeaconMiniHack
{
    [Activity (Label = "aBeaconMiniHack", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, BeaconManager.IServiceReadyCallback
    {
        static readonly string beaconId = "myEstimoteBeacon";
        static readonly string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";

        BeaconManager beaconManager;
        Region region;

        public void OnServiceReady ()
        {
            region = new Region(beaconId, uuid, null, null);
            beaconManager.StartRanging (region);
            beaconManager.StartMonitoring (region);
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            SetContentView (Resource.Layout.Main);

            beaconManager = new BeaconManager (this);

            beaconManager.SetBackgroundScanPeriod (TimeUnit.Seconds.ToMillis (1), 0);

            beaconManager.EnteredRegion += (sender, e) => {
                Console.WriteLine ("entered region");
            };
            beaconManager.ExitedRegion += (sender, e) => {
                Console.WriteLine ("exited region");        
            };

            beaconManager.Ranging += (object sender, BeaconManager.RangingEventArgs e) => {

                foreach (var beacon in e.Beacons) {
                    var proximity = Utils.ComputeProximity (beacon);

                    if (proximity == Utils.Proximity.Immediate) {
                        Console.WriteLine ("{0} immediate", beacon.Name);

                    } else if (proximity == Utils.Proximity.Near) {
                        Console.WriteLine ("{0} near", beacon.Name);

                    } else if (proximity == Utils.Proximity.Far) {
                        Console.WriteLine ("{0} far", beacon.Name);

                    } else if (proximity == Utils.Proximity.Unknown) {
                        Console.WriteLine ("{0} unknown", beacon.Name);
                    }
                }
            };
        }

        protected override void OnResume ()
        {
            base.OnResume ();
            beaconManager.Connect (this);
        }

        protected override void OnDestroy ()
        {
            beaconManager.Disconnect ();
            base.OnDestroy ();
        }
    }
}