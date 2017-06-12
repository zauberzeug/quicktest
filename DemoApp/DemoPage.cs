using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
	public class DemoPage : ContentPage
	{
		public DemoPage()
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

			var list = new List<string> { "A", "B", "C" };
			var listView = new ListView {
				ItemsSource = list,
				ItemTemplate = new DataTemplate(typeof(TextCell)),
			};
			listView.ItemTemplate.SetBinding(TextCell.TextProperty, ".");
			listView.ItemTapped += (sender, e) => OpenMessagePage(e.Item + " tapped");

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					label,
					button,
					stackLayout,
					listView,
				},
			};

			ToolbarItems.Add(new ToolbarItem {
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
