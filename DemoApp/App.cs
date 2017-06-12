using Xamarin.Forms;

namespace DemoApp
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new DemoPage());
		}

		public static void PushPage(ContentPage page)
		{
			(Current.MainPage as NavigationPage).PushAsync(page);
		}

		public static void PushMessagePage(string message)
		{
			PushPage(new ContentPage {
				Title = "Message page",
				Content = new Label {
					Text = message,
				},
			});
		}
	}
}
