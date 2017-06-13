using Xamarin.Forms;

namespace DemoApp
{
	public class DemoModalPage : Button
	{
		public DemoModalPage()
		{
			Text = "DemoModalPage";

			Command = new Command(o => Application.Current.MainPage.Navigation.PushModalAsync(new ContentPage {
				Content = new StackLayout {
					Children = {
						new Label {
							Text = "Modal page pushed",
						},
						new Button {
							Text = "Close",
							Command = new Command(obj => Application.Current.MainPage.Navigation.PopModalAsync()),
						},
					},
				},
			}));
		}
	}
}
