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

		public static void ShowMessage(string title, string message)
		{
			Current.MainPage.DisplayAlert(title, message, "Ok");
		}
	}
}
