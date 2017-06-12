using Xamarin.Forms;

namespace DemoApp
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new DemoPage());
		}

		public static void PushMessagePage(string message)
		{
			(Current.MainPage as NavigationPage).PushAsync(new ContentPage {
				Title = "Message page",
				Content = new Label {
					Text = message,
				},
			});
		}
	}
}
