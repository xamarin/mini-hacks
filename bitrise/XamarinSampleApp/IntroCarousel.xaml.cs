using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Xamarin.Social;
using Xamarin.Social.Services;

using System.Threading.Tasks;

namespace XamarinSampleApp
{
	public partial class IntroCarousel : CarouselPage
	{
		public IntroCarousel ()
		{
			InitializeComponent ();

			TourButton.Clicked += (object sender, EventArgs e) => {
				this.CurrentPage = this.Children [1];
			};

			TweetButton.Clicked += (object sender, EventArgs e) => {
				var item = new Item { Text = "Just finished the Bitrise Hackaton at #Xamarin Evolve!" };

				DependencyService.Get<ISocialController> ().TweetWithItem (item);
			};

			this.CurrentPageChanged += (object sender, EventArgs e) => {
				if (this.CurrentPage == this.Children[0]) {
					DependencyService.Get<IAppearance> ().UpdateBackground (0.231, 0.764, 0.639);
				} else {
					DependencyService.Get<IAppearance> ().UpdateBackground (0.505, 0.317, 0.658);
				}
			};

			ShowScrollOption (1500, 40);
		}

		protected void ShowScrollOption (double after, double delay)
		{
			// if it looks stupid but works it ain't stupid
			Device.StartTimer (TimeSpan.FromMilliseconds (after), () => {
				this.CurrentPage = this.Children [1];

				Device.StartTimer (TimeSpan.FromMilliseconds (delay), () => {
					this.CurrentPage = this.Children [0];
					return false;
				});
				return false;
			});
		}
	}
}

