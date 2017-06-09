using Xamarin.Forms;

namespace FormsTest
{
	public class DemoPage : NavigationPage
	{
		public DemoPage() : base(new ContentPage())
		{
			var label = new Label {
				Text = "Label",
			};
			label.GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => Navigation.PushAsync(new ContentPage { Content = new Label { Text = "Label tapped" } })),
			});

			var button = new Button {
				Text = "Button",
				Command = new Command(o => Navigation.PushAsync(new ContentPage { Content = new Label { Text = "Button clicked" } })),
			};

			(CurrentPage as ContentPage).Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					label,
					button,
				},
			};
		}
	}
}
