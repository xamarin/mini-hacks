
Twilio Mini Hack
===
Welcome, fine adventurer to the Twilio Mini Hack.  This simple hack will introduce you to Twilio Client which allows you to easily embed Voice over IP communication directly into your native iOS and Android applications.  

By completing this hack you will have a mobile app that will make a VoIP phone call from inside your iOS or Android application to a phone such as your your cell phone.

Sounds amazing, doesn't it?  We think so too!  

We think you'll be able to complete this mini hack in 20 minutes.  If you get stuck or have any questions, no problem.  Head over to the Twilio booth and we'll be happy to walk through some code with you.

Alright.  With the intro out of the way, let’s get building!

### Mini Hack Requirements ###

Developers using Macs can build either the iOS or Android versions of this hack.  You'll need to make sure you have the following installed:

- a copy of Xamarin Studio with either Xamarin.iOS or Xamarin.Android

Windows developers will only be able to build the Android version of the hack.  You'll need to make sure you have the following:

- Visual Studio 2013
- Xamarin for Windows with Xamarin.Android
- Administrator access to your development machine.

Of course if you have any questions about your specific development environment, head over to the Twilio booth and we'll be happy to help you out.

Getting Started
===
To get started you'll need to set up a bit of infrastructure.

As you might expect, the first thing you'll need is a Twilio account.  Don't worry, trial accounts are free, so if you don't already have one, [head on over to the Twilio website](https://www.twilio.com/try-twilio) and sign up.  I'll wait right here while you do it.

You're back! Fantastic. Let’s move on to the second thing you'll need.  

Now, we know you can't wait to make your phone ring (we can't either!) so to get you moving fast we've set up a really simple starter solution for you.  If you're reading this that means you've already found the Github repo, so go ahead and clone it to your machine or download the ZIP.

In the solution you'll find three projects:

- **TwilioMiniHackStarter.Web** - An ASP.NET website which you will use to generate a Capability Token (we'll talk more about that in a minute)
- **TwilioMiniHackStarter.iOS** - A standard iPhone Single View application project
- **TwilioMiniHackStarter.Android** - A standard Android Application project

You'll use at least two of these projects for the hack, maybe all three if you're feeling especially ambitious.


TwiML and TwiML Applications
===
Brilliant.  Now that you've got the infrastructure for this hack in place, you're just about ready to make your first VoIP call to Twilio.  Let's take a minute to understand what happens when you make an inbound call to Twilio.

For all inbound communication to Twilio, whether voice, VoIP or Messaging, Twilio uses a set of XML commands called [TwiML](https://www.twilio.com/docs/api/twiml) to provide an experience to the inbound caller, or respond to the inbound message.

Twilio gets that TwiML by making an HTTP request to a URL that you have associated with either your [Twilio phone number](https://www.twilio.com/user/account/phone-numbers/incoming), or in the case of a VoIP call using Twilio Client (which does not use phone numbers), a [TwiML application](https://www.twilio.com/user/account/apps).

![Twilio Request](http://i.imgur.com/vLo21E8.png)

![Twilio Response](http://i.imgur.com/KpBHXp8.png)

Awesome.  Now that you've got a basic understanding of how Twilio works, let's get to the code.

Configuring Twilio Capabilities
===
A Twilio Client application actually starts with a bit of server-side code that generates a [Capability Token](https://www.twilio.com/docs/client/capability-tokens).  

A Capability Token is an encrypted string that lets an instance of Twilio Client authenticate with Twilio and tell Twilio if it is allowed to make outbound or receive inbound calls.

To generate the capability token you'll use the `TwilioMiniHackStarter.Web` project that is included in this starter solution:

<div style="background:#FAFAD2;">
<p><strong>Note: If you are building this application using Visual Studio and IIS Express, please start Visual Studio as 'Administrator'.</strong></p>
</div>

![ASP.NET Website Project](http://i.imgur.com/yMA6ZLY.png)

In that project, open up the `ClientController.cs` file.  You'll see we've already added the code needed for generating a Capability Token using the [Twilio Client helper library](https://www.nuget.org/packages/Twilio.Client/) which we added via NuGet.  

    public ActionResult Token()
    {
		var capabilities = new TwilioCapability (
			                   "[REPLACE_WITH_YOUR_TWILIO_ACCOUNT_SID]", 
			                   "[REPLACE_WITH_YOUR_TWILIO_AUTH_TOKEN]");

		capabilities.AllowClientIncoming ("[REPLACE_WITH_A_CLIENT_NAME]");
		capabilities.AllowClientOutgoing ("[REPLACE_WITH_YOUR_TWIML_APPLICATION_SID]");
		var token = capabilities.GenerateToken ();

        return Content (token);
    }
        
As you can see there are some strings you'll need to replace.

You can find your Twilio Account Sid and Auth Token in your [Twilio dashboard](https://www.twilio.com/user/account):

![Twilio Dashboard](http://i.imgur.com/GRXNNg5.png)

The `AllowClientIncoming` method will give your mobile app using this capability token a unique ID that can be used to call it later - like a phone number for each user of the app:

    capabilities.AllowClientIncoming ("JohnSmith");

Finally, drop in a TwiML Application Sid in the `AllowClientOutgoing` method.  

You can create a new TwiML Application Sid by heading over to the [TwiML Application tab in the Twilio Dashboard](https://www.twilio.com/user/account/apps).  Configure your TwiML Applications Voice Request URL with the following:

`http://twiliominihack.azurewebsites.net/Client/Bridge`

(you can leave the Message Request URL empty)

Curious what that URL does?  It generates TwiML for Twilio to execute.  Loading the URL in a browser lets you see exactly what it's generating:  

    <Response>
        <Dial callerId="[SOURCE]">[TARGET]</Dial>
    </Response>
    
This TwiML uses the [`<Dial>`](https://www.twilio.com/docs/api/twiml/dial) verb to tell Twilio to connect the incoming VoIP call from your mobile application to an outbound phone call made to another phone number, like your mobile phone.

Right now the phone number to dial is represented by the `[TARGET]` placeholder.  In the next section, you'll learn how to pass that phone number to Twilio dynamically from your mobile app.

Once you've saved your TwiML application, the Sid will be displayed:

![TwiML Application](http://i.imgur.com/a2gQgAQ.png)

Awesome!  Thats all there is to generating a capability token.

<div style="background:#FAFAD2;">

<div style="margin:20px;">
<h2>IIS Express Configuration</h2>

<p>Are you building this project using Visual Studio and IIS Express?  Before you run the project there's an extra step you'll need to take in order to make sure the Android emulator can successfully make an HTTP request to your IIS Express server.</p>

<p>Open the IIS Express configuration file, which is located in:</p>

<pre>[Documents]\IISExpress\config\applicationhost.config</pre>

<p>In the config file find the configuration section for the `TwilioMiniHackStarter.Web` site.  Within the `<bindings>` element add an additional binding:</p>

<pre>&lt;bindings&gt;
    &lt;binding protocol="http" bindingInformation="*:2357:localhost" /&gt;
		
    &lt;!-- add an additional binding to allow the Android emulator to reach the server --&gt;
    &lt;binding protocol="http" bindingInformation="*:2357:" /&gt;
&lt;/bindings&gt;</pre>

<p>By adding this configuration, IIS Express will now answer to any host name (not just localhost) for this specific site, allowing the Android emulator to reach the website and retrieve a Capability Token.</p>
</div></div>


Go ahead and run the website project from Xamarin Studio or Visual Studio.  When the browser loads you should see the capability token being output in the browser: 

![Capability Token loaded in the browser](http://i.imgur.com/6u2etir.png)

Once it is working for you, leave the website running with the debugger and move on to the next section of the hack where you will build your mobile app.

Building a VoIP App
===
Now that you're generating a capability token, you're ready to start building a mobile application that can make an outbound VoIP call.

Because that application needs the Capability Token, you will need to keep the website running in order for the application to request it.  

If you are using Windows and Visual Studio the easiest way to do this is to open the `TwilioMiniHackStarter` solution in a second instance of Visual Studio.

Using a Mac and Xamarin Studio?  There are several options, including:

1. Deploy the site to a local web server or a remote web host
2. Use Xamarin Studio's built in web server.  Using this option will require you to open a second instance of Xamarin studio which you can do using this [Launcher App](http://redth.codes/Xamarin-Studio-Launcher-v3/), or from the Terminal:

        open -n /Applications/Xamarin\ Studio.app

Once you get that second instance of Visual Studio or Xamarin Studio running, open up the mini hack starter kit solution and jump to the mobile platform you want to build your app in:

- [iOS](#ios)
- [Android](#android)

iOS
====
<a name="ios"></a>Alright iOS ninja, are you ready to build a VoIP enabled application?  Fantastic!

Start by finding the iOS project we've provided in the hack solution:

![iOS project highlight](http://i.imgur.com/Vt6jZve.png)

There is nothing special about this project (yet), it's literally just the default project template.  Start to make it more awesome by adding the Twilio Client for iOS component.  Check out [this blog post for step-by-step installation instructions](https://www.twilio.com/blog/2014/08/twilio-client-for-xamarin-part-1-introduction.html).

Once you add the component, open up the `TwilioMiniHackStarter.iPhoneViewController.cs` and add a new using statement:

    using TwilioClient.iOS;

and a couple of class-level variables:  

	TCDevice _device;
	TCConnection _connection;

Now you are ready to initialize your instance of Twilio Client by creating a new instance of the `TCDevice` object.  Its constructor accepts a Capability Token, which you can retrieve from the web site created earlier:

	public async override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
			
		// Create an HTTPClient object and use it to fetch
		// a capability token from our website. 
		var client = new HttpClient ();
		var token = await client.GetStringAsync("http://localhost:8080/Client/Token");

		// Create a new TCDevice object passing in the token.
		_device = new TCDevice (token, null);
	}

Once you initialize Twilio Client, it's ready to make an outbound call to Twilio.  

Add a button to the application UI.  In its TouchUpInside event call the `TCDevice` instances `Connect` method.  The `Connect` method accepts a dictionary of parameters which you can use to pass arbitrary parameters to your TwiML Application.  

	partial void Call_TouchUpInside (UIButton sender)
	{
		var parameters = NSDictionary.FromObjectsAndKeys(
			new object[] { "[REPLACE_WITH_YOUR_MOBILE_PHONE_NUMBER]", "[REPLACE_WITH_YOUR_TWILIO_PHONE_NUMBER]" },
			new object[] { "Target", "Source"}
		);

		_connection = _device.Connect(parameters, null);
	}
		
The snippet above passes in two parameters, `Target` and `Source`.  `Target` represents the phone number you want Twilio to connect this instance of Twilio Client to, for example your mobile phone number.  

`Source` represents the the phone number that you want Twilio to use as the Caller ID when it calls the Target.  The Source number must be either [a phone number you've purchased from Twilio](https://www.twilio.com/user/account/phone-numbers/incoming), or [a phone number that has been verified by Twilio](https://www.twilio.com/user/account/phone-numbers/verified).

Now that your app is calling the `Connect` method, it's a great time to give it a test.  Launch the app in the iOS simulator and give that button a tap.  You should hear Twilio Client connect to Twilio and, if you are using an upgraded Twilio account, your mobile phone should start to ring.  Go ahead and answer that call and enjoy your first VoIP to Voice phone call using Twilio Client.

If, however, you are using a Twilio trial account, Twilio will prompt you to press a key to continue the call.  Obviously, since this is a custom application there are no keys to press, so you need to make one.

Add another button to your application UI and in its `TouchUpInside` event, call the `Connection` instances `SendDigits` method, passing a string containing a number from 1 to 9:

	partial void SendDtmf_TouchUpInside (UIButton sender)
	{
		if (_connection != null && _connection.State == TCConnectionState.Connected) {
			_connection.SendDigits("1");
		}
	}

`SendDigits` lets you tell Twilio client to simulate pressing keys on a phones dial pad.  

Go ahead and give your app a second test run, and this time when Twilio prompts you to press a key, tap that second button and wait for your mobile phone to ring.

Amazing.  In just a few minutes you've used Twilio and Twilio Client to build a custom iOS application with embedded VoIP capabilities.  

Head over to the Twilio booth and give us a show.  Make our phones ring and we'll gladly hand over a really great prize.

Android
====
<a name="android"></a>OK Android warrior, lets build a VoIP enabled app!

Start by finding the Android project we've provided in the hack solution:

![Android project highlight](http://i.imgur.com/htEztPp.png)

There is nothing special about this project (yet), it's literally just the default project template.

Begin making it more awesome by enabling the permissions needed by Twilio Client.  Locate the `AndroidManifest.xml` file in the project and open it.  In Xamarin Studio, navigate to the `Properties` folder:

![Setting Android Permissions](http://i.imgur.com/xPiJ3hy.png)

In Visual Studio, open the Project Properties for the `TwilioMiniHackStarter.Android` project and select the the Android Manifest tab:

![Android Manifest tab](http://i.imgur.com/gDps3uo.png)

In the Required Permissions listbox enable the following:

- AccessNetworkState
- AccessWifiState
- ModifyAudioSettings
- RecordAudio

Next, edit the AndroidManifest XML to declare the Twilio Client Service.  On a Mac switch to the Source view:

![AndroidManifest Source View](http://i.imgur.com/DJIJEZQ.png)

In Visual Studio locate the AndroidManfest file under the Properties folder:

![Visual Studio Manifest](http://i.imgur.com/v4e4KPy.png)

In the Application Manifest XML, declare the Twilio Client Service inside of the `<application>` element:

	<application android:label="TwilioMiniHackStarter.Android">
		<service android:name="com.twilio.client.TwilioClientService" android:exported="false" />
	</application>

Now you're ready to add the Twilio Client for Android component.  Check out [this blog post for step-by-step installation instructions](https://www.twilio.com/blog/2014/08/twilio-client-for-xamarin-part-1-introduction.html).

Once you've got the component added, open up the `MainActivity.cs` and add a new using statement:

    using TwilioClient.Android;

and a couple of class-level variables:  

	private Device _device;
	private IConnection _connection;
	private const string TAG = "TwilioMiniHack";
	
Next, implement the `Twilio.IInitListener` interface on your Activity class.  This interface requires implementing two methods, `OnInitialized` and `OnError`.

	public class MainActivity : Activity, Twilio.IInitListener
	{
		public void OnError (Java.Lang.Exception p0)
        {
        }
            
   		public async void OnInitialized ()
		{
		}
    }

Now you are ready to initialize your instance of Twilio Client by calling the static `Initialize` method on the `Twilio` class.

    protected override void OnCreate (Bundle bundle)
	{
		base.OnCreate (bundle);

		// Set our view from the "main" layout resource
		SetContentView (Resource.Layout.Main);

		Twilio.Initialize (this.ApplicationContext, this);
	}

Calling `Initialize` gets Twilio Client ready to be used in your application.  

Once initialized, Twilio Client will call the `OnInitialized` method.  This method gives you the opportunity to retrieve a Capability Token from the website you created earlier and then create a new `TCDevice` object:

	public async void OnInitialized ()
	{
		try {
			var client = new HttpClient ();
			var token = await client.GetStringAsync ("http://10.0.2.2:8080/Client/Token");

			_device = Twilio.CreateDevice(token, null);
			
		} catch (Exception ex) {
			Log.Info(TAG, "Error: " + ex.Message);
		}
	}

**Note: The IP address used in the snippet above is a magic IP set up by the default Android emulator to bridge to your host machine.  If you are using the Genymotion emulator, the IP address will be 10.0.3.2.**

**Developers using Visual Studio and IIS Express may need to update the port used in the URL to match the port selected by IIS Express**

Using the TCDevice you can now make an outbound call to Twilio.  

Add a button to the application UI and in its Click event call the `TCDevice` instances `Connect` method.  The Connect method accepts a dictionary of parameters which you can use to pass arbitrary parameters to your TwiML Application.  

	// Get our button from the layout resource,
	// and attach an event to it
	Button call = FindViewById<Button> (Resource.Id.callButton);
	call.Click += delegate {
		var parameters = new Dictionary<string, string> () {
			{ "Target", "[REPLACE_WITH_YOUR_MOBILE_PHONE_NUMBER]"},
			{ "Source", "[REPLACE_WITH_YOUR_TWILIO_PHONE_NUMBER]"}			};
				
		_connection = _device.Connect (parameters, null);
	};	
		
The snippet above passes in two parameters, `Target` and `Source`.  `Target` represents the phone number you want to Twilio to connect this instance of Twilio Client to, for example your mobile phone number.  

`Source` represents the phone number that you want Twilio to use as the Caller ID when it calls the Target.  The Source number must be either [a phone number you've purchased from Twilio](https://www.twilio.com/user/account/phone-numbers/incoming), or [a phone number that has been verified by Twilio](https://www.twilio.com/user/account/phone-numbers/verified).

Now that your app is calling the `Connect` method, it's a great time to give it a test.  Launch the app in the Android simulator and give that button a tap.  You should hear Twilio Client connect to Twilio and, if you are using an upgraded Twilio account, your mobile phone should start to ring.  Go ahead and answer that call and enjoy knowing you've made your first VoIP to Voice phone call using Twilio Client.

**Note: The Twilio Client component currently does not support L Preview or the ART runtime. Make sure you are using an API Level 19 emulator using the Dalvik runtime for this MiniHack.**

If, however, you are using a Twilio trial account, Twilio will prompt you to press a key to continue the call.  Obviously, since this is a custom application there are no keys to press, so you need to make one.

Add another button to your application UI and in its `Click` event, call the `Connection` instances `SendDigits` method, passing a string containing a number from 1 to 9:

	Button sendDtmf = FindViewById<Button> (Resource.Id.sendDtmf);
	sendDtmf.Click += delegate {
		if (_connection != null && _connection.State == ConnectionState.Connected)
		{
			_connection.SendDigits("1");
		}
	};

`SendDigits` lets you tell Twilio client to simulate pressing keys on a phones dial pad.  

Go ahead and give your app a second test run, and this time when Twilio prompts you to press a key, tap that second button and wait for your mobile phone to ring.

Amazing.  In just a few minutes you've used Twilio and Twilio Client to build a custom Android application with embedded VoIP capabilities.  

Head over to the Twilio booth and give us a show.  Make our phones ring and we'll gladly hand over a really great prize.
