# Windows Azure Mobile Services

### The Challenge

In this challenge, you will use Windows Azure Mobile Services and Xamarin.iOS to build a scalable, native iOS app in C#.

Login to the Windows Azure portal and create a new Mobile Service and SQL database.  Add the Windows Azure Mobile Services library from the Component Store to your project in Xamarin Studio.    

<b>Prize?</b> The official Windows Azure Mini-Hack badge.

This will work with either a Xamarin.iOS or a Xamarin.Android project.  In this walkthrough, we'll focus on iOS, but you can find more details on using Android with Mobile Services [here](http://www.windowsazure.com/android).

The walkthrough below should help you with the challenge, but you can also get in touch with @jasonallor or @paulbatum with questions -- both of whom are on site at Evolve and happy to help!


#####Bonus Challenge #1
For bonus points--connect your app to blob storage and add a service from the Windows Azure Store to your app.

<b>Prize?</b> A bottle opener for all the craft brews our friends at Xamarin will be providing.

#####Bonus Challenge #2
Add Twitter authentication and configure push notifications using your Apple developer certificate.

<b>Prize?</b> Mobile Services T-shirt. And it's actually pretty sweet.

####Validation…and Prizes!

<b>You can get these challenges validated at the Microsoft booth by Paul or Jason between 10:15 and 1:15 on Tuesday or after 2:30 pm on Wednesday.</b>

Also be sure to catch Paul's session--[Building Connected, Cross-Platform Mobile Apps with Azure](http://evolve2013.sched.org/event/f41b0afed4ebb1fa0c09e489586e9f9a#.UWYB36tARSY)--Tuesday at 5 pm for a proper introduction to building cloud-connected apps.

### Challenge Walkthrough

* Login to your Windows Azure account at manage.windowsazure.com.  (If you don't have a Windows Azure account, you can obtain a free 90-day trial at www.windowsazure.com.)

* Click New --> Compute --> Mobile Service --> Create.  Then specify URL and database login/password order to create a new Mobile Service and the associated SQL database.

*  Go to the 'DATA' tab then click the 'Create' button at the bottom to add a new table called 'TodoItem' to your SQL database.

* On the dashboard, click 'Manage Keys' and copy your Application and Master Keys to a text file.

* Fire up Xamarin Studio and using the built-in Component Store (Project --> Get More Components), add the free Azure Mobile Services component to your project. (If you aren't working on an app at the moment, we recommend grabbing the [Tasky](http://docs.xamarin.com/samples/Tasky) sample app from Xamarin for this challenge.)

* Copy over the following code into your AppDelegate.cs file to connect your Xamarin Studio project to the Mobile Service you created in the Windows Azure portal:

```CSharp
using Microsoft.WindowsAzure.MobileServices;
...

public static MobileServiceClient MobileService = new MobileServiceClient(
"https://yourMobileServiceName.azure-mobile.net/", 
"YOUR_APPLICATION_KEY"
);
```

* Now that you've connected your project to Mobile Services, store data in the table you created earlier using the following code snippet:

```CSharp
public class TodoItem
{
    public int Id { get; set; }
    [DataMember (Name = "text")]
    public string Text { get; set; }
    [DataMember (Name = "complete")]
    public bool Complete { get; set; }
}

...

this.table = MobileService.GetTable<TodoItem>();
this.table.Where (ti => !ti.Complete).ToListAsync()
    .ContinueWith (t => { this.items = t.Result; }, scheduler);
```

* In iOS, add a Rounded Rect Button to the MainView.cs file and create an outlet for the UIbutton named 'btn.'

* Add a handler to the button using the snippet below:

```CSharp
btnAdd.TouchUpInside += (sender, e) => 
	{
		var todo = new TodoItem();
		todo.Text = txtTask.Text; // hard code it or use a form.
		todo.Complete = swComplete.On; // same
		MobileService.GetTable<TodoItem>().InsertAsync(todo);
	};
```

* If you head back to the [Windows Azure Portal](manage.windowsazure.com), you'll see that items you add to the list are now stored in your SQL database.

* Get fired up that you're up and running with Mobile Services and go get verified with the Microsoft team to secure your badge.

#####Bonus Challenge #1 Walkthrough

Blob Storage

* To access blob storage from your Mobile Services application, go to the 'DATA' tab and select TodoItem table.  

* On the 'SCRIPT' tab, select the whichever operation you'd like and copy in the following code to use the “azure” module within the Windows Azure SDK for Node.js.  Once you obtain that reference, you can query or insert data to that blob. 


```CSharp
var azure = require('azure');
var blobService = azure.createBlobService("<< account name >>",
                                            "<< access key >>");
```

Windows Azure Store

* The Windows Azure Store contains services and data sets that can be useful in your app.

* Click New --> Store --> SendGrid (or the service of your choice) --> Free.

* Select ChosenSendGridName from the Dashboard and then hit 'Connection Info' and note that information to your Notepad.

* Navigate to your Mobile service then click 'DATA' --> the 'ToDoItem' table --> 'SCRIPT.'

* On the 'Insert' script, replace the Insert function with the below code in order to trigger an email each time a new item is inserted to the 'ToDoItem' table.

```js
var SendGrid = require('sendgrid').SendGrid;


function insert(item, user, request) {    
    request.execute({
        success: function() {
            // After the record has been inserted, send the response immediately to the client
            request.respond();
            // Send the email in the background
            sendEmail(item);
        }
    });


function sendEmail(item) {
    var sendgrid = new SendGrid('**username**', '**password**');       

    sendgrid.send({
        to: '**email-address**',
        from: '**from-address**',
        subject: 'New to-do item',
        text: 'A new to-do was added: ' + item.text
    }, function(success, message) {
        // If the email failed to send, log it as an error so we can investigate
        if (!success) {
            console.error(message);
        }
    });
}


}
```


#####Bonus Challenge #2 Walkthrough


* The next step is to get set up with Twitter authentication.  Follow the steps on [this page](http://www.windowsazure.com/en-us/develop/mobile/how-to-guides/register-for-twitter-authentication/) to register your app for Twitter authentication with Mobile Services.

* Copy over your Consumer Key Secret from Twitter into the appropriate slots in the 'IDENTITY' tab. Hit save.

* Back in Xamarin Studio, add the following code to a view controller.  This code wires login up to an iOS action such as clicking a login button.

```CSharp
    partial void Login()
    {
        AppDelegate.MobileService.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter).ContinueWith(t => 
        {
            BeginInvokeOnMainThread (() => 
            {
            	var user = t.Result;
            	var alert = new UIAlertView("Welcome!",  
                                	"You are now logged in and your user id is " 
                                	+ user.UserId, null, "OK");                                                       
                alert.Show ();
            });
        });
    }
```

* To get started with push notifications, head to the 'PUSH' tab and upload your Apple Developer Certificate. (More details on getting an Apple Developer Certificate can be found [here](http://www.windowsazure.com/en-us/develop/mobile/tutorials/get-started-with-push-ios/).)

* Once you have an Apple developer certificate, add the following to your AppDelegate.cs file.  The insert code will be able to use the AppDelegate.DeviceToken property and include that in the insert item so that the script can use it to send the push notification.

```CSharp
public static string DeviceToken { get; privateset; }
 
public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		application.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound);
                        returntrue;
     }
               
public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
	{
                        Console.WriteLine(deviceToken.Description);
                        DeviceToken = deviceToken.Description.Replace("<","").Replace(">","");
    }
               
public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
	{
	Console.WriteLine(error);
	}
               
public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
	{
	Console.WriteLine(userInfo);
	}
 
```

* Head back to the 'Todo Item' table you created earlier under the 'DATA' tab

* Hit 'SCRIPT' and replace the existing function under 'Insert' with the below in order to send a push notification every time a new record is added to the table.  Don't forget to hit save!

```js
function insert(item, user, request) {
    request.execute();
    // Set timeout to delay the notification, to provide time for the 
    // app to be closed on the device to demonstrate toast notifications
    setTimeout(function() {
        push.apns.send(item.deviceToken, {
            alert: "Toast: " + item.text,
            payload: {
                inAppMessage: "Hey, a new item arrived: '" + item.text + "'"
            }
        });
    }, 2500);
}
```




