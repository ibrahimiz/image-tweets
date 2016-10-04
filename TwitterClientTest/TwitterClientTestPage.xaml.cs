using Xamarin.Forms;
using System.Collections.Generic;

namespace TwitterClientTest
{
	public partial class TwitterClientTestPage : ContentPage
	{
		public TwitterClientTestPage()
		{
			InitializeComponent();

			var service = new TwitterService();

			TiwterList.ItemsSource = service.GetImageTweets("Burj Khalifa");

		}
	}


}
