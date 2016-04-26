using System;

using Xamarin.Social;
using Xamarin.Social.Services;

namespace XamarinSampleApp
{
	public interface ISocialController
	{
		void TweetWithItem(Item item);
	}
}

