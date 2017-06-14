using Xamarin.Forms;

namespace DemoApp
{
	public class App : Application
	{
		public static string PageLog;

		public App()
		{
			PageLog = "";

			MainPage = new MasterDetailPage {
				Master = new MenuPage(),
				Detail = new NavigationPage(new NavigationDemoPage()),
			};
		}

		public static void Open(ContentPage page)
		{
			(Current.MainPage as MasterDetailPage).Detail = new NavigationPage(page);
			(Current.MainPage as MasterDetailPage).IsPresented = false;
		}

		public static void PushAsync(ContentPage page)
		{
			((Current.MainPage as MasterDetailPage).Detail as NavigationPage).PushAsync(page);
		}

		public static void PopAsync()
		{
			((Current.MainPage as MasterDetailPage).Detail as NavigationPage).PopAsync();
		}

		public static void PopToRootAsync()
		{
			((Current.MainPage as MasterDetailPage).Detail as NavigationPage).PopToRootAsync();
		}

		public static void PushModalAsync(ContentPage page)
		{
			Current.MainPage.Navigation.PushModalAsync(page);
		}

		public static void PopModalAsync()
		{
			Current.MainPage.Navigation.PopModalAsync();
		}

		public static void ShowMessage(string title, string message)
		{
			Current.MainPage.DisplayAlert(title, message, "Ok");
		}
	}
}
