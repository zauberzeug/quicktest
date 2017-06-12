using System.Collections.Generic;
using Xamarin.Forms;

namespace DemoApp
{
	public class DemoPage : ContentPage
	{
		public DemoPage()
		{
			Title = "Demo page";

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

			var listView1 = new ListView {
				ItemsSource = new List<string> { "A1", "B1", "C1" },
				ItemTemplate = new DataTemplate(typeof(TextCell)),
			};
			listView1.ItemTemplate.SetBinding(TextCell.TextProperty, ".");
			listView1.ItemTapped += (sender, e) => OpenMessagePage(e.Item + " tapped");

			var listView2 = new ListView {
				ItemsSource = new List<string> { "A2", "B2", "C2" },
				ItemTemplate = new DataTemplate(typeof(CustomCell)),
			};
			listView2.ItemTapped += (sender, e) => OpenMessagePage(e.Item + " tapped");

			Content = new StackLayout {
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					label,
					button,
					stackLayout,
					listView1,
					listView2,
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
				Title = "Message page",
				Content = new Label {
					Text = message,
				},
			});
		}
	}

	public class CustomCell : ViewCell
	{
		public CustomCell()
		{
			var label = new Label();
			label.SetBinding(Label.TextProperty, ".");
			View = new StackLayout {
				Children = { label },
			};
		}
	}
}
