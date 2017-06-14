using System;
using Xamarin.Forms;

namespace DemoApp
{
	public class DemoNavigationPage : ContentPage
	{
		INavigation CurrentNavigation { get { return Application.Current.MainPage.Navigation; } }

		public DemoNavigationPage(string title = "Navigation page")
		{
			Title = title;

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new DemoButton("PushAsync") {
						Command = new Command(o => CurrentNavigation.PushAsync(new DemoNavigationPage(title + " >"))),
					},
					new DemoButton("PushModalAsync") {
						Command = new Command(o => CurrentNavigation.PushModalAsync(new DemoNavigationPage(title + " ^"))),
					},
					new DemoButton("PopAsync") {
						Command = new Command(obj => CurrentNavigation.PopAsync()),
					},
					new DemoButton("PopModalAsync") {
						Command = new Command(obj => CurrentNavigation.PopModalAsync()),
					},
					new DemoButton("PopToRootAsync") {
						Command = new Command(obj => CurrentNavigation.PopToRootAsync()),
					},
				},
			};

			if (title.EndsWith("^", StringComparison.Ordinal))
				(Content as StackLayout).Children.Insert(0, new DemoLabel(title));
		}
	}
}
