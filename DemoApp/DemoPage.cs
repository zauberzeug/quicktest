using Xamarin.Forms;

namespace DemoApp
{
	public class DemoPage : ContentPage
	{
		public DemoPage()
		{
			Title = "Demo page";

			Content = new ScrollView {
				Content = new StackLayout {
					Children = {
						new DemoButton("Button"),
						new DemoLabel("Label").WithGestureRecognizer(),
						new DemoStack(),
						new DemoGrid(),
						new DemoButton("ListViews") { Command = new Command(o => Navigation.PushAsync(new DemoListViewPage())) },
						new DemoButton("Alert") { Command = new Command(o => Application.Current.MainPage.DisplayAlert("Alert", "Message", "Ok")) },
						new DemoButton("Modal page") { Command = new Command(o => Navigation.PushModalAsync(new DemoModalPage())) },
						new DemoButton("Dis-/Appearing") { Command = new Command(o => Navigation.PushAsync(new DisAppearingPage())) },
					},
				},
			};

			ToolbarItems.Add(new DemoToolbarItem());
		}
	}
}
