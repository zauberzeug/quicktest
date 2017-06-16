using System;
using Xamarin.Forms;

namespace DemoApp
{
	public class MenuPage : ContentPage
	{
		public MenuPage()
		{
			Title = "Menu";

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new DemoLabel("Menu"),
					CreateMenuButton("Navigation", () => new NavigationDemoPage()),
					CreateMenuButton("Elements", () => new ElementDemoPage()),
					CreateMenuButton("ListViews", () => new ListViewDemoPage()),
					new DemoButton("Alert") { Command = new Command(o => Application.Current.MainPage.DisplayAlert("Alert", "Message", "Ok")) },
				},
			};
		}

		DemoButton CreateMenuButton(string title, Func<ContentPage> pageCreator)
		{
			return new DemoButton(title) {
				Command = new Command(o => {
					var mainPage = (Application.Current.MainPage as MasterDetailPage);
					mainPage.Detail = new NavigationPage(pageCreator.Invoke());
					mainPage.IsPresented = false;
				}),
			};
		}
	}
}
