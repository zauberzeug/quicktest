using Xamarin.Forms;

namespace DemoApp
{
	public class DemoModalPage : ContentPage
	{
		public DemoModalPage()
		{
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new DemoLabel("Modal page pushed"),
					new DemoButton("Close") {
						Command = new Command(obj => Navigation.PopModalAsync()),
					},
				},
			};
		}
	}
}
