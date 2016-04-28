using System.IO;

using Foundation;

using Xamarin.Forms;

using MyDevices.iOS;
using MyDevices.Interfaces;

[assembly: Dependency (typeof (iOSDirectory))]
namespace MyDevices.iOS
{
	public class iOSDirectory : IDirectory
	{
		public byte[] GetByteArrayFromFile (string fileName)
		{
			var filePath = NSBundle.MainBundle.PathForResource (fileName, "bin");

			return File.ReadAllBytes (filePath);
		}
	}
}