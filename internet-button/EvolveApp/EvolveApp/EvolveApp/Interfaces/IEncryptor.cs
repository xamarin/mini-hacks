using System;
namespace EvolveApp.Interfaces
{
	public interface IEncryptor
	{
		string Sign(string requestString, string key);
	}
}