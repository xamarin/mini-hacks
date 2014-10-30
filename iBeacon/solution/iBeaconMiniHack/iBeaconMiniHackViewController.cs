using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreLocation;

namespace iBeaconMiniHack
{
    public partial class iBeaconMiniHackViewController : UIViewController
    {
        static readonly string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
        static readonly string beaconId = "myEstimoteBeacon";

        CLLocationManager locationManager;
        CLProximity previousProximity;
        string message;

        public iBeaconMiniHackViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            var beaconUUID = new NSUuid (uuid);
            var beaconRegion = new CLBeaconRegion (beaconUUID, beaconId);

            beaconRegion.NotifyEntryStateOnDisplay = true;
            beaconRegion.NotifyOnEntry = true;
            beaconRegion.NotifyOnExit = true;

            locationManager = new CLLocationManager ();

            locationManager.RequestWhenInUseAuthorization ();

            locationManager.DidStartMonitoringForRegion += (object sender, CLRegionEventArgs e) => {
                locationManager.RequestState (e.Region);
            };

            locationManager.RegionEntered += (object sender, CLRegionEventArgs e) => {
                if (e.Region.Identifier == beaconId) {
                    Console.WriteLine ("beacon region entered");
                }
            };

            locationManager.DidDetermineState += (object sender, CLRegionStateDeterminedEventArgs e) => {

                switch (e.State) {
                case CLRegionState.Inside:
                    Console.WriteLine ("region state inside");
                    break;
                case CLRegionState.Outside:
                    Console.WriteLine ("region state outside");
                    break;
                case CLRegionState.Unknown:
                default:
                    Console.WriteLine ("region state unknown");
                    break;
                }
            };

            locationManager.DidRangeBeacons += (object sender, CLRegionBeaconsRangedEventArgs e) => {
                if (e.Beacons.Length > 0) {

                    CLBeacon beacon = e.Beacons [0];

                    switch (beacon.Proximity) {
                    case CLProximity.Immediate:
                        message = "Immediate";
                        break;
                    case CLProximity.Near:
                        message = "Near";
                        break;
                    case CLProximity.Far:
                        message = "Far";
                        break;
                    case CLProximity.Unknown:
                        message = "Unknown";
                        break;
                    }

                    if (previousProximity != beacon.Proximity) {
                        Console.WriteLine (message);
                    }
                    previousProximity = beacon.Proximity;
                }
            };

            locationManager.StartMonitoring (beaconRegion);
            locationManager.StartRangingBeacons (beaconRegion);
        }
    }
}