using Xamarin.Forms;

namespace DemoApp
{
	public class DemoDisAppearing : Button
	{
		public DemoDisAppearing()
		{
			Text = "DemoDisAppearing";

			Command = new Command(o => App.PushPage(new DisAppearingPage()));
		}
	}

	public class DisAppearingPage : ContentPage
	{
		public DisAppearingPage()
		{
			Title = "(Dis)Appearing page";

			Content = new Label {
				Text = "Constructed",
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Content = new Label {
				Text = "Appeared",
			};
		}

		protected override void OnDisappearing()
		{
			Application.Current.MainPage.DisplayAlert("Alert", "Disappearing", "Ok");

			base.OnDisappearing();
		}
	}
}
