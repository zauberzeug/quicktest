using Xamarin.Forms;

namespace DemoApp
{
	public class DemoPage : NavigationPage
	{
		public DemoPage() : base(new ContentPage())
		{
			var label = new Label {
				Text = "Label",
			};
			label.GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => OpenMessagePage("Label tapped")),
			});

			var button = new Button {
				Text = "Button",
				Command = new Command(o => OpenMessagePage("Button tapped")),
			};

			var stackLayout = new StackLayout {
				Children = {
					new Label {
						Text = "Nested label",
					},
				},
			};
			stackLayout.GestureRecognizers.Add(new TapGestureRecognizer {
				Command = new Command(o => OpenMessagePage("StackLayout tapped")),
			});

			(CurrentPage as ContentPage).Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					label,
					button,
					stackLayout,
				},
			};

			(CurrentPage as ContentPage).ToolbarItems.Add(new ToolbarItem {
				Text = "ToolbarItem",
				Command = new Command(o => OpenMessagePage("ToolbarItem tapped")),
			});
		}

		void OpenMessagePage(string message)
		{
			Navigation.PushAsync(new ContentPage {
				Content = new Label {
					Text = message,
				},
			});
		}
	}
}
