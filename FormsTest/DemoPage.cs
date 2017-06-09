using Xamarin.Forms;

namespace FormsTest
{
	public class DemoPage : NavigationPage
	{
		public DemoPage() : base(new ContentPage())
		{
			(CurrentPage as ContentPage).Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label {
						Text = "Label",
					},
					new Button {
						Text = "Button",
						Command = new Command(o => Navigation.PushAsync(new ContentPage { Content = new Label{ Text = "Ok" } })),
					},
				},
			};
		}
	}
}
