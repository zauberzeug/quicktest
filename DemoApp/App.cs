using Xamarin.Forms;

namespace DemoApp
{
	public class App : Application
	{
		public static string PageLog;

		public App()
		{
			PageLog = "";

			ToggleMasterDetail();
		}

		public void ToggleMasterDetail()
		{
			if ((MainPage as MasterDetailPage) == null)
				MainPage = new MasterDetailPage {
					Master = new MenuPage(),
					Detail = new NavigationPage(new NavigationDemoPage()),
				};
			else
				MainPage = new NavigationPage(new NavigationDemoPage());
		}

		public static NavigationPage CurrentNavigationPage {
			get {
				return (Current.MainPage as NavigationPage) ?? ((Current.MainPage as MasterDetailPage).Detail as NavigationPage);
			}
		}

		public static void PushAsync(Page page)
		{
			CurrentNavigationPage.PushAsync(page);
		}

		public static void PopAsync()
		{
			CurrentNavigationPage.PopAsync();
		}

		public static void PopToRootAsync()
		{
			CurrentNavigationPage.PopToRootAsync();
		}

		public static void PushModalAsync(Page page)
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
