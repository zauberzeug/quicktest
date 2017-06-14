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

			Appearing += (sender, e) => (Content as Label).Text += "!";
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
			Application.Current.MainPage.DisplayAlert("Disappeard", "Page just disappeard", "Ok");

			base.OnDisappearing();
		}
	}
}
