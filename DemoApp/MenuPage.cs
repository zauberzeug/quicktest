using System;
using Xamarin.Forms;

namespace DemoApp
{
	public class MenuPage : ContentPage
	{
		public MenuPage()
		{
			Title = nameof(MenuPage);

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new DemoLabel("Menu"),
					CreateMenuButton("Navigation", () => new NavigationDemoPage()),
					CreateMenuButton("Elements", () => new ElementDemoPage()),
					CreateMenuButton("ListViews", () => new ListViewDemoPage()),
					CreateMenuButton("Dis-/Appearing", () => new DisAppearingDemoPage()),
					new DemoButton("Alert") { Command = new Command(o => Application.Current.MainPage.DisplayAlert("Alert", "Message", "Ok")) },
				},
			};
		}

		DemoButton CreateMenuButton(string title, Func<ContentPage> pageCreator)
		{
			return new DemoButton(title) {
				Command = new Command(o => App.Open(pageCreator.Invoke())),
			};
		}
	}
}
