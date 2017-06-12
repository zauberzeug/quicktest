using Xamarin.Forms;

namespace DemoApp
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new DemoPage());
		}
	}
}
