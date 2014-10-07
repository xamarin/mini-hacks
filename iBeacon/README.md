# iBeacon Mini-Hack

## Challenge

Create an application that detects proximity to an Estimote Beacon.

NOTE: To perform this hack requires an iOS device or Android device that supports Bluetooth LE, with Bluetooth turned on, and an Estimote Beacon.

## Walkthrough (iOS)

1. Create a new iPhone Single View Application named iBeaconMiniHack.
2. In the ``iBeaconMiniHackViewController`` add a using statement for CoreLocation and add the following class variables:

        static readonly string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";
        static readonly string beaconId = "myEstimoteBeacon";
    
        CLLocationManager locationManager;
        CLProximity previousProximity;
        string message;

3. In ``ViewDidLoad`` create an ``NSUuid`` and a ``CLBeaconRegion`` for the beacon and initialize the CLBeaconRegion.

        var beaconUUID = new NSUuid (uuid);
        var beaconRegion = new CLBeaconRegion (beaconUUID, beaconId);
    
        beaconRegion.NotifyEntryStateOnDisplay = true;
        beaconRegion.NotifyOnEntry = true;
        beaconRegion.NotifyOnExit = true;

4. Create a ``CLLocationManager`` and call ``RequestWhenInUseAuthorization``.

        locationManager = new CLLocationManager ();
        locationManager.RequestWhenInUseAuthorization ();

5. Implement ``DidStartMonitoringForRegion``:

        locationManager.DidStartMonitoringForRegion += (object sender, CLRegionEventArgs e) => {
            locationManager.RequestState (e.Region);
        };

6. Implement ``RegionEntered``.

        locationManager.RegionEntered += (object sender, CLRegionEventArgs e) => {
            if (e.Region.Identifier == beaconId) {
                Console.WriteLine ("beacon region entered");
            }
        };

7. Implement ``DidDetermineState``.

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

8. Implement ``DidRangeBeacons``.

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

9. Call ``StartMonitoring`` and ``StartRangingBeacons``.

        locationManager.StartMonitoring (beaconRegion);
        locationManager.StartRangingBeacons (beaconRegion);
        
10. Add the ``NSLocationWhenInUseUsageDescription`` to the Info.plist

        <key>NSLocationWhenInUseUsageDescription</key>
        <string>iBeaconMiniHack for Evolve 2014</string>

11. Debug the application on an iOS device and you should see the messages in the Application Output.

## Walkthrough (Android)

1. Create a new Android Ice Cream Sandwich application named aBeaconMiniHack.
2. Change the target framework to 18.
3. Add the Estimote SDK Xamarin component.
4. In the MainActivity delete the code added in OnCreate by the template.
5. Add the following using statements:

        using EstimoteSdk;
        using Java.Util.Concurrent;

6. Add the following class variables:

        static readonly string beaconId = "myEstimoteBeacon";
        static readonly string uuid = "B9407F30-F5F8-466E-AFF9-25556B57FE6D";

        BeaconManager beaconManager;
        Region region;

7. Implement ``BeaconManager.IServiceReadyCallback`` in ``MainActivity``.

        public void OnServiceReady ()
        {
            region = new Region(beaconId, uuid, null, null);
            beaconManager.StartRanging (region);
            beaconManager.StartMonitoring (region);
        }

8. In ``OnCreate`` create a ``BeaconManager`` and set the background scan period.

        beaconManager = new BeaconManager (this);
        beaconManager.SetBackgroundScanPeriod (TimeUnit.Seconds.ToMillis (1), 0);

9. Handle the ``EnteredRegion`` and ``ExitedRegion`` events of the ``BeaconManager``.

        beaconManager.EnteredRegion += (sender, e) => {
            Console.WriteLine ("entered region");
        };
        beaconManager.ExitedRegion += (sender, e) => {
            Console.WriteLine ("exited region");        
        };

10. Handle the ``Ranging`` event.

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

11. In ``OnResume`` connect to the ``BeaconManager``.

        protected override void OnResume ()
        {
            base.OnResume ();
            beaconManager.Connect (this);
        }

12. In ``OnDestroy`` disconnect from the ``BeaconManager``.

        protected override void OnDestroy ()
        {
            beaconManager.Disconnect ();
            base.OnDestroy ();
        }

13. Update the AndroidManaifest.xml as follows:

        <?xml version="1.0" encoding="utf-8"?>
        <manifest xmlns:android="http://schemas.android.com/apk/res/android" android:versionCode="1" android:versionName="1.0" package="aBeaconMiniHack.aBeaconMiniHack">
            <uses-sdk android:minSdkVersion="15" android:targetSdkVersion="18"/>
            <application android:label="aBeaconMiniHack">
                <service android:name="com.estimote.sdk.service.BeaconService" android:exported="false" />
            </application>
            <uses-permission android:name="android.permission.BLUETOOTH" />
            <uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />
            <uses-feature android:name="android.hardware.bluetooth_le" android:required="true" />
        </manifest>

14. Debug the application on an Android device (with Bluetooth LE) and you should see the messages in the Application Output.
