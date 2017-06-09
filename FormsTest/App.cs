using System;
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

			MainPage = new MasterDetailPage {
				Master = new ContentPage {
					Title = " ",
					Icon = Device.OS == TargetPlatform.iOS ? "menu.png" : null,
					Content = new StackLayout {
						VerticalOptions = LayoutOptions.CenterAndExpand,
						Children = {
							CreatePageOpener("Demo page", () => new DemoPage()),
							CreatePageOpener("Test result",() => new NavigationPage(tester.ResultPage)),
						},
					},
				},
				Detail = new NavigationPage(tester.ResultPage),
			};

			tester.TryRunTest();
		}

		Button CreatePageOpener(string text, Func<NavigationPage> pageCreator)
		{
			return new Button {
				Text = text,
				Command = new Command(o => {
					(MainPage as MasterDetailPage).Detail = pageCreator.Invoke();
					(MainPage as MasterDetailPage).IsPresented = false;
				}),
			};
		}
	}
}
