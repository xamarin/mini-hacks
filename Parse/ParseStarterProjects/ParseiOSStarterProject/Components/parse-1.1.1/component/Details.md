Parse is an instant backend server for your mobile app. Working with
Parse is as natural as working with normal C# objects.  We provide Parse
objects that can be stored and retrieved from our platform. You'll also
be able to view and edit this data in real time using our web interface.

## Saving an Object

Here's all it takes to save an object to Parse:

```csharp
using ParseTouch;
...

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

Parse also provides user account management and push
notifications. Learn more about Parse [here](https://parse.com/about/index).
 
*This component requires iOS 6.*
