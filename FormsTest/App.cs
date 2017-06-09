using Xamarin.Forms;

namespace FormsTest
{
	public class App : Application
	{
		public App()
		{
			var demoPage = new DemoPage();

			var tester = new AppTester {
				Page = demoPage,
			};

			MainPage = new NavigationPage(tester.ResultPage);

			tester.RunTest();
		}
	}
}
