using System;
using Xamarin.Forms;

namespace DemoApp
{
	public class NavigationDemoPage : ContentPage
	{
		public NavigationDemoPage(string title = "Navigation demo")
		{
			Title = title;

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new DemoLabel("Navigation stack:"),
					new DemoButton("PushAsync") {
						Command = new Command(o => App.PushAsync(new NavigationDemoPage(title + " >"))),
					},
					new DemoButton("PopAsync") {
						Command = new Command(obj => App.PopAsync()),
					},
					new DemoButton("PopToRootAsync") {
						Command = new Command(obj => App.PopToRootAsync()),
					},
					new DemoLabel("Modal stack:"),
					new DemoButton("PushModalAsync") {
						Command = new Command(o => App.PushModalAsync(new NavigationDemoPage(title + " ^"))),
					},
					new DemoButton("PopModalAsync") {
						Command = new Command(obj => App.PopModalAsync()),
					},
					App.Log,
				},
			};

			if (title.EndsWith("^", StringComparison.Ordinal))
				(Content as StackLayout).Children.Insert(0, new DemoLabel(title));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			App.Log.Text += " Appeared";
		}

		protected override void OnDisappearing()
		{
			App.Log.Text += " Disappeared";

			base.OnDisappearing();
		}
	}
}
