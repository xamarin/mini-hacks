using System;
using System.Security.Cryptography;
using System.Text;
namespace Console
{
	public static class Extensions
	{
		public static long ToUnixTime(this DateTime date)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return Convert.ToInt64((date - epoch).TotalSeconds);
		}
	}
}