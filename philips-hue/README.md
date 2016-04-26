# Hue-MiniHack

Amazing Philip Hue: [Turn On Living video on YouTube] (https://www.youtube.com/watch?v=lCv4r3wgsPQ) 

Before you start, you'll want to signup to the [Philips Hue Developer](http://www.developers.meethue.com) program which unlocks detailed API information and wealth or resources t for expanding your knowledge of Hue. 

> You'll find TODOs within the solution which numbers match this readme. You can find the Task pad in Xamarin Studio by clicking View > Pads > Tasks 

## The Challenge
You'll need to control Hue lamps from a Xamarin.Forms app. 

> You'll want to ensure that you've restored the Nuget packages before trying to build the project. You may also need to configure Android deployment within the configuration manager in VS to deploy to a simulator or device (this isn't applicable for those of you using Xamarin Studio). 

### Whitelisting and permission to access the bridge

1. #### Set your Apps name and Device name. 
    You should create a unique name (Mini-Hack is probably already taken) along with either a unique DeviceName or match your AppName. 

2. #### Locate the Hue Bridge
   Locate the BridgeViewModel class and add the following starting at line 60.
  ```csharp
   IBridgeLocator locator = new HttpBridgeLocator();
   IEnumerable<string> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

   BridgeIps.Clear();
   foreach (var ip in bridgeIPs)
   {
       BridgeIps.Add(ip);
   }
   ```
3. #### Register your app
  Locate the BridgeRegisterViewModel class and add the following starting at line 30.
  
  ```csharp
   ILocalHueClient client = new LocalHueClient(SelectedBridgeIP);
   
   if(string.IsNullOrEmpty(Helpers.Settings.AppKey))
       Helpers.Settings.AppKey = await client.RegisterAsync(App.AppName, App.DeviceName);
       
   client.Initialize(Helpers.Settings.AppKey);
   ```   
4. #### Create LocalHueClient
  Locate the LightsViewModel class and add the following starting at line 63.
  
  ```csharp
   ILocalHueClient client = new LocalHueClient(Helpers.Settings.DefaultBridgeIP);
   client.Initialize(Helpers.Settings.AppKey);
   ```
5. #### Discover all Hue lamps connected to bridge
  Staying in the LightsViewModel class, add the following after the client.Initialize method.
  
  ```csharp
   IEnumerable<Light> lights = await client.GetLightsAsync();
   Lights.Clear();
   foreach (var light in lights)
   {
      Lights.Add(light);
   }
   ```   
6. #### Turn a lamp on
  Locate the LightViewModel class and add the following starting at line 90.
  
  ```csharp
   var command = new LightCommand();
   command.TurnOn();

   var lights = new List<string> { SelectedLight.Id };
   await client.SendCommandAsync(command, lights);
   ```   
   
7. #### Turn a lamp off
  Staying in the LightViewModel class, add the following starting at line 121.
  
  ```csharp
   var command = new LightCommand();
   command.TurnOff();

   var lights = new List<string> { SelectedLight.Id };
   await client.SendCommandAsync(command, lights);
   ```   
8. #### Set lamp to 'Alert'
  Staying in the LightViewModel class, add the following starting at line 152.
  
  ```csharp
   var command = new LightCommand();
   command.Alert = Alert.Once;

   var lights = new List<string> { SelectedLight.Id };
   await client.SendCommandAsync(command, lights);
   ```  
9. #### Start color effect on lamp
  Staying in the LightViewModel class, add the following starting at line 184.
  
  ```csharp
   var command = new LightCommand();
   command.Effect = Q42.HueApi.Effect.ColorLoop;

   var lights = new List<string> { SelectedLight.Id };
   await client.SendCommandAsync(command, lights);
   ```  
## Win a Hue Starter Kit
Take the mini-hack and extended to do something unique that demonstrates why Hue is a great platform to developer and be entered to win a starter kit (5 available). 
