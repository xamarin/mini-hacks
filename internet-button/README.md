Internet Button Mini Hack
===
Welcome to the Internet Button Hack. This simple hack will introduce you to Azure Event Bus which will allow you to easily receive messages from a Particle Internet Button or any lower level device. 

By completing this hack you will have a Azure Event Bus that will receive messages from the SimonSays game interactions. The mobile application will also publish events that can be received through the Event Bus.

Sounds amazing, doesn't it?  We think so too!  

Alright.  With the intro out of the way, let’s get building!

### Mini Hack Requirements ###

Developers will need to setup a Particle account to interact with devices and setup the underlying framework for messages. The mobile application is already built with Xamarin.Forms and supports iOS, Android and UWP. If you would like to check out the source code, it will be made available in the near future. 

Getting Started
===
To get started you'll need to create an Azure account if you don't already have one. Head over to the [Azure Portal](https://portal.azure.com) to setup a [free trial](https://azure.microsoft.com/en-us/pricing/free-trial/). 

You will also need to create a Particle account. Don't worry, it's is completely free to [signup](https://build.particle.io/signup). You can also create an account through the mobile application if you don't want to interact with the Particle website. Once you have signed up, make sure to write down your username and password because we will need it soon. 

Next we need to install the Particle command line tools. We will use this to create the webhook that will send messages to our Azure Event Bus.  

[Particle CLI Windows Install Instructions](https://community.particle.io/t/tutorial-particle-cli-on-windows-07-jun-2015/3112)  
[Particle CLI Mac Install Instructions](https://github.com/spark/particle-cli#installing)

###Ready?

Azure account created? Check.  
Particle account created? Check.  
Particle CLI or Sample installed? Check.  

Let's get started then!

#The Mobile Application
As mentioned earlier, you can create a Particle account through the application:

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/readme/SignUp.png?token=AIPtRrG7qxDNyUGyLJSTvDW_L90yZ0-vks5XJjNPwA%3D%3D" width="600"/>

Make sure you write down your username and password, you will need this to setup your webhook and login to the Particle cloud. Now you can login to the application.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/readme/Login.png?token=AIPtRnUyeKb8RXW2CFuc3JeKw9RRzjAuks5XJjZAwA%3D%3D" width="600"/>

Once logged into the application, you will land on the "Scan Device Page". This page allows us to scan the barcode on our InternetButton and take control of it.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/readme/ScanDevice.png?token=AIPtRumtOnoOOGOsZkBoYk1KLbj6yCL1ks5XJjaxwA%3D%3D" width="600"/>

We now control out Internet Button and we will land on the device mission control page.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/readme/SimonSays_Overview.png?token=AIPtRmVs71JpUPVRIKoUUkXfzgccASdAks5XJjcuwA%3D%3D" width="600"/>


Let's setup our Azure stuff before we go any further in the application.

#Azure Event Bus
Login to your Azure Portal or [start a free trial](https://azure.microsoft.com/en-us/pricing/free-trial/) if you don't have an Azure account. 

Once logged in to our Azure Portal, we are going to create a new Internet of Things resource called Azure Event Bus.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/NewEventHub.png?token=AIPtRsemtRp6hR9cHxtfqALEjXPVDF0lks5XJjfAwA%3D%3D" width="600"/>

Some Internet of Things aren't supported by Azure IoT Hub because it requires the Azure IoT Library installed, but that doesn't mean we can't use Azure to understand our IoT devices better! Really every IoT device should speak to some hub to gather and analyze the data.

After selecting Event Hub, Azure should bring up everything to create a new Event Bus like below:

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/createNewEventHub.png?token=AIPtRkfklbXTyNCi5_LgtXj5vhGk3P_7ks5XJjo3wA%3D%3D" width="600"/>

Now once we have out Event Hub created, we will now need to configure the hub to have a token for our device webhook. Once the Event Bus is created, select it in the overview screen:

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/viewEventHub.png?token=AIPtRmviqVp7jfrwHDH3cjv-8wgfUeBDks5XJj0wwA%3D%3D" width="600"/>

From here we will need to configure a Shared Access Policy for the webhook to use. Select Configure from the top of the Event Hub page:

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/selectConfigure.png?token=AIPtRr0HviM8Fp2PMCNW7pK-It-WAg1uks5XJj3TwA%3D%3D" width="600"/>

Now you can pick any name for the Shared Access Policy and select "Send" from permissions. Once you hit save, the Shared Access Policy that you created will have a primary key that we will need.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/sharedAccessPolicies.png?token=AIPtRioStaDYwB4hG7PiH9qM6iQN7ZNhks5XJj5KwA%3D%3D" width="600"/>

Now we have everything in Azure setup to receive messages! We will copy the Policy Name and Primary Key into our webhook.

#Creating the Webhook
Now we need to create a webhook to send our messages from the Particle Cloud to Azure. We can do this through a raw json file and our Particle CLI. Below is the json text you will need to save to a location somewhere on your drive. Just make sure you can easily point to the file to create the webhook

```
{
	"event": "SimonSays",

	"url": "https://(YOUR SERVICE BUS URL).servicebus.windows.net/YOUR SERVICE BUS NAME/messages",

	"requestType": "POST",

	"json": {

		"gameid": "{{g}}",

		"activity": "{{a}}",

		"value": "{{v}}",

		"timecreated": "{{SPARK_PUBLISHED_AT}}",

		 "guid": "{{SPARK_CORE_ID}}"
	},

	"azure_sas_token": {

		"key_name": "(POLICY NAME)",

		"key": "(Shared Access Primary Key)"

	},

	"mydevices": true
}
```
It is important to make sure that you enter the correct url. If you aren't sure, go back to your Azure Portal and view the Event Bus dashboard. There is a an "Event Hub URL" displayed and that is the link we will use with the addition of "/messages".

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/GetConnectionString.png?token=AIPtRjhTV3wE8y2JqdisxqZDNTUP3gdjks5XJkPUwA%3D%3D" width="600"/>

From here we will need to login to our Particle CLI and create our webhook. Open up a terminal or command prompt to login. Type "particle login" and follow the login process like below:

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/particlelogin.png?token=AIPtRiJxU6SQTblk6Pe3vG2B7E5hP8RCks5XJkUdwA%3D%3D" width="600"/>

Once we are logged into the Particle CLI, we need to navigate to the directory wehere the json file was saved. From here we simply need to create the webhook by typing "particle webhook create webhook.json".

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/particlewebhookcreate.png?token=AIPtRmtaawZdDoBH8jJA_mj9a6e6gMgdks5XJkXDwA%3D%3D" width="600"/>

Now, let's go back to the application to play around!

#Playing a game of Simon Says
Open the mobile application up and flash the "Simon Says" application to the Internet Button. Once the flash is complete (application loading indicator stops spinning and Internet Button is breathing a blue color), press "START INTERACTION".

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/readme/SimonSays_Start.png?token=AIPtRi0QKBSZfl6_wXdBjr5X48ac89A-ks5XJka5wA%3D%3D" width="600"/>

Go ahead and play a game. The game will publish the "SimonSays" events that will be received by Azure. After playing with the game, feel free to play around with the application more. There are some additional functionality you can play around with, like controlling the LED colors on the Internet Button.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/readme/rgbled.png?token=AIPtRn3x3hOGlmchoTocKTWtTd0-Ng2zks5XJk6hwA%3D%3D" width="600"/>

It could take a few minutes for messages to start popping up in the Event Hub. Looking at Total Requests will show up sooner than Incoming Messages because Azure will have to process the json into a message. Show us your portal view with messages in the Event Bus to finish this mini-hack. We will accept Total Requests or Incoming Messages.

<img src="https://raw.githubusercontent.com/michael-watson/InternetButtonEvolve2016/master/images/azure/receivedMessages.png?token=AIPtRuNlRRldjhN4WHusJaAXg_TD9JlSks5XJk8VwA%3D%3D" width="600"/>

