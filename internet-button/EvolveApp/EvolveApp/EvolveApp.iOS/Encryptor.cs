using System;
using EvolveApp.Interfaces;
using System.Security.Cryptography;
using System.Text;
using EvolveApp.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(Encryptor))]
namespace EvolveApp.iOS
{
	public class Encryptor : IEncryptor
	{
		public Encryptor() { }

		public string Sign(string requestString, string key)
		{
			string result = "";

			using (HMACSHA256 hMACSHA = new HMACSHA256(Convert.FromBase64String(key)))
			{
				result = Convert.ToBase64String(hMACSHA.ComputeHash(Encoding.UTF8.GetBytes(requestString)));
			}

			return result;
		}
	}
}