using Xamarin.Forms;

namespace DemoApp
{
	public class DisAppearingDemoPage : ContentPage
	{
		public DisAppearingDemoPage()
		{
			Title = "(Dis)Appearing demo";

			Content = new DemoLabel("Constructed");

			Appearing += (sender, e) => (Content as Label).Text += "!";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Content = new DemoLabel("Appeared");
		}

		protected override void OnDisappearing()
		{
			App.ShowMessage("Disappeared", "Page just disappeared");

			base.OnDisappearing();
		}
	}
}
