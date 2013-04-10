Parse is an instant backend server for your mobile app. Working with
Parse is as natural as working with normal C# objects.  We provide Parse
objects that can be stored and retrieved from our platform. You'll also
be able to view and edit this data in real time using our web interface.

## Initialize the API

First, you'll need to sign up for a Parse app ID at
https://parse.com/apps/new. Once you have an ID, you can
initialize your Parse client:

```csharp
using ParseTouch;
...

public override bool FinishedLaunching (UIApplication app, NSDictionary options)
{
	...
	Parse.SetAppId ("YOUR_APP_ID", "YOUR_CLIENT_ID");
	...
}
```

## Saving an Object

Here's all it takes to save an object to Parse:

```csharp
void MakePost ()
{
	var post = new ParseObject ("Post");
	post ["title"] = new NSString ("I love Parse with Xamarin");
	post ["body"] = new NSString ("Need I say more?");
	post.SaveAsync ((success, error) => {
		Console.WriteLine ("Success : {0}", success);
	});
}
```

## User Management

Adding user accounts to your app has never been easier.  Here is all you
have to add to create an account for Mario:

```csharp
void CreateUser ()
{
	var user = new ParseUser { Username = "Mario", Password = "peach" };
	user.SignUp ();
}
```

### Documentation

- Technical Docs: https://parse.com/docs
- Tutorials: https://parse.com/docs/tutorials
- Terms & Conditions: https://www.parse.com/about/terms

### Contact

- Contact: https://www.parse.com/about/contact
- Blog: http://blog.parse.com/
- Twitter: https://twitter.com/#!/parseit

*This component requires iOS 6.*
